﻿using Microsoft.Identity.Client;
using Microsoft.Win32;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace WindowsFormsApplication
{
    public class Nfield
    {
        private IPublicClientApplication ClientApp { get; init; }
        private string[] Scopes { get; } = new[] { CreateScope("user_impersonation"), CreateScope("Survey.Read") };
        public string DomainName { get; }

        private INfieldConnectionV2 NfieldConnection { get; set; }

        public Nfield(string domainName)
        {
            ClientApp = CreateClientApp();
            DomainName = domainName;

            NfieldConnection = NfieldConnectionFactory.Create(ApplicationConfiguration.Current.NfieldPublicApiEndpoint);
            NfieldConnection.RegisterTokenProvider(domainName, ProvideTokenAsync);
        }

        private IPublicClientApplication CreateClientApp()
        {
            var client = PublicClientApplicationBuilder.Create(ApplicationConfiguration.Current.ClientId)
                .WithRedirectUri("http://localhost")
                .WithAuthority(AzureCloudInstance.AzurePublic, ApplicationConfiguration.Current.TenantId)
                .Build();

            client.UserTokenCache.SetBeforeAccess(BeforeAccessNotification);
            client.UserTokenCache.SetAfterAccess(AfterAccessNotification);

            return client;
        }

        private async Task<string> ProvideTokenAsync()
        {
            Action<string> noop = (status) => { }; // don't report status for each and every API call

            var result = await AuthenticateAsync(noop);
            return result.AccessToken;
        }

        private static string CreateScope(string scope) => $"{ApplicationConfiguration.Current.NfieldPublicApiApplicationId}/{scope}";

        internal async Task LogoutAsync()
        {
            var accounts = (await ClientApp.GetAccountsAsync()).ToList();
            while (accounts.Any())
            {
                await ClientApp.RemoveAsync(accounts.First());
                accounts = (await ClientApp.GetAccountsAsync()).ToList();
            }
        }

        private byte[] RestoreTokenCacheFromRegistry()
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\NIPO Samples"))
            {
                var value = key.GetValue("TokenCache");
                if (value is byte[] data)
                {
                    return ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
                }
            }

            return null;
        }

        private void SaveTokenCacheInRegistry(byte[] state)
        {
            var data = ProtectedData.Protect(state, null, DataProtectionScope.CurrentUser);
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\NIPO Samples"))
            {
                key.SetValue("TokenCache", data, RegistryValueKind.Binary);
            }
        }

        public void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            args.TokenCache.DeserializeMsalV3(RestoreTokenCacheFromRegistry());
        }

        public void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            if (args.HasStateChanged)
            {
                SaveTokenCacheInRegistry(args.TokenCache.SerializeMsalV3());
            }
        }

        public async Task<AuthenticationResult> TryAuthenticateSilentAsync()
        {
            var accounts = await ClientApp.GetAccountsAsync();
            var account = accounts.FirstOrDefault();

            try
            {
                return await ClientApp.AcquireTokenSilent(Scopes, account).ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                // In a real app at least log this
                return null;
            }
        }

        public async Task<AuthenticationResult> AuthenticateAsync(Action<string> statusCallback)
        {
            var accounts = await ClientApp.GetAccountsAsync();
            var account = accounts.FirstOrDefault();

            try
            {
                return await ClientApp.AcquireTokenSilent(Scopes, account).ExecuteAsync();
            }
            catch (MsalUiRequiredException mure)
            {
                // Either we are not logged in or we are required to consent on giving the app permission
                statusCallback(mure.Message);

                // fall through to interactive login
            }

            var authResult = await ClientApp.AcquireTokenInteractive(Scopes)
                .WithAccount(account)
                .WithPrompt(Prompt.SelectAccount)
                .ExecuteAsync();

            return authResult;
        }

        public async Task<IEnumerable<Survey>> ListSurveysAsync()
        {
            var surveysService = NfieldConnection.GetService<INfieldSurveysService>();
            var surveys = await surveysService.QueryAsync();
            return surveys;
         }
    }
}

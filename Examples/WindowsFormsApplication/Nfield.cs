using Microsoft.Identity.Client;
using Microsoft.Win32;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SSO_AAD
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

            NfieldConnection = NfieldConnectionFactory.Create(ApplicationConfiguration.Current.NfieldApiUri);
            NfieldConnection.RegisterTokenProvider(domainName, ProvideTokenAsync);
        }

        private IPublicClientApplication CreateClientApp()
        {
            var client = PublicClientApplicationBuilder.Create(ApplicationConfiguration.Current.ClientId)
                .WithRedirectUri("http://localhost")
                .WithAuthority(AzureCloudInstance.AzurePublic, ApplicationConfiguration.Current.Tenant)
                .Build();

            client.UserTokenCache.SetBeforeAccess(BeforeAccessNotification);
            client.UserTokenCache.SetAfterAccess(AfterAccessNotification);

            return client;
        }

        private async Task<string> ProvideTokenAsync()
        {
            var result = await AuthenticateAsync((status) => { });
            return result.AccessToken;
        }

        private static string CreateScope(string scope) => $"{ApplicationConfiguration.Current.NfieldApiApplicationId}/{scope}";

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
                if (value is byte[] state)
                    return state;
            }

            return null;
        }

        private void SaveTokenCacheInRegistry(byte[] state)
        {
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\NIPO Samples"))
            {
                key.SetValue("TokenCache", state, RegistryValueKind.Binary);
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
                statusCallback(mure.Message);
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

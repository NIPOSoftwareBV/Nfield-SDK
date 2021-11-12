using Microsoft.Extensions.Configuration;
using System;

namespace WindowsFormsApplication
{
    public class ApplicationConfiguration
    {
        public string ClientId { get; private set; }
        public string TenantId { get; private set; }
        public string NfieldPublicApiApplicationId { get; private set; }
        public Uri NfieldPublicApiEndpoint { get; internal set; }
        public string DomainName { get; internal set; }

        public static ApplicationConfiguration Current { get; } = InitializeConfiguration();

        private ApplicationConfiguration()
        {
        }

        private static ApplicationConfiguration InitializeConfiguration()
        {
            var root = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .Build();

            return new ApplicationConfiguration
            {
                ClientId = root["Application:ClientId"],
                TenantId = root["Application:TenantId"],
                NfieldPublicApiApplicationId = root["Nfield:PublicApiApplicationId"],
                NfieldPublicApiEndpoint = new Uri(root["Nfield:PublicApiEndpoint"]),
                DomainName = root["Nfield:DomainName"]
            };
        }
    }
}

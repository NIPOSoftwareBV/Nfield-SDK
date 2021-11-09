using Microsoft.Extensions.Configuration;
using System;

namespace SSO_AAD
{
    public class ApplicationConfiguration
    {
        public string ClientId { get; private set; }
        public string Tenant { get; private set; }
        public string NfieldApiApplicationId { get; private set; }
        public Uri NfieldApiUri { get; internal set; }
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
                ClientId = root["Nfield:ClientId"],
                Tenant = root["Nfield:Tenant"],
                NfieldApiApplicationId = root["Nfield:ApiApplicationId"],
                NfieldApiUri = new Uri(root["Nfield:ApiUri"]),
                DomainName = root["Nfield:DomainName"]
            };
        }
    }
}

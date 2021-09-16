namespace Nfield.Models
{
    public class DomainPasswordSettings 
    {
        /// <summary>
        /// The default 'AgeWarnThreshold' password settings
        /// </summary>
        public int AgeWarnThreshold { get; set; }

        /// <summary>
        /// The default 'EnforceTwoFactorAuthentication' password settings
        /// </summary>
        public bool EnforceTwoFactorAuthentication { get; set; }

        /// <summary>
        /// The default 'MaxPasswordAge' password settings
        /// </summary>
        public int MaxPasswordAge { get; set; }

        /// <summary>
        /// The default 'MinCharsetsInPassword' password settings
        /// </summary>
        public int MinCharsetsInPassword { get; set; }

        /// <summary>
        /// The default 'MinPasswordLength' password settings
        /// </summary>
        public int MinPasswordLength { get; set; }

        /// <summary>
        /// The default 'PasswordHistoryLength' password settings
        /// </summary>
        public int PasswordHistoryLength { get; set; }
    }
}

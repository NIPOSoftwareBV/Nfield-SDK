namespace Nfield.Models
{
    /// <summary>
    /// The password settings of the domain
    /// </summary>
    public class DomainPasswordSettings
    {
        /// <summary>
        /// Name of the password setting that determines age warning threshold for a domain        
        /// </summary>
        public int AgeWarnThreshold { get; set; }

        /// <summary>
        /// Name of the password setting that determines enforce two factor authentication is enabled for a domain
        /// </summary>
        public bool EnforceTwoFactorAuthentication { get; set; }

        /// <summary>
        /// Name of the password setting that determines maximum password age for a domain
        /// </summary>
        public int MaxPasswordAge { get; set; }

        /// <summary>
        /// Name of the password setting that determines minimum charsets in password for a domain
        /// </summary>
        public int MinCharsetsInPassword { get; set; }

        /// <summary>
        /// Name of the password setting that determines minimum password length for a domain
        /// </summary>
        public int MinPasswordLength { get; set; }

        /// <summary>
        /// Name of the password setting that determines password history length for a domain
        /// </summary>
        public int PasswordHistoryLength { get; set; }

    }
}

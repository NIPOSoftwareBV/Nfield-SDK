namespace Nfield.Models
{
    /// <summary>
    /// The password settings of the domain
    /// </summary>
    public class DomainPasswordSettings
    {
        /// <summary>
        /// Name of the password setting that determines warn when password expires in x days for a domain
        /// The valid values for the AgeWarnThreshold from 0 to 10
        /// </summary>
        public int? AgeWarnThreshold { get; set; }

        /// <summary>
        /// Name of the password setting that indicates if two-factor authentication is enforced for a domain
        /// </summary>
        public bool? EnforceTwoFactorAuthentication { get; set; }

        /// <summary>
        /// Name of the password setting that determines maximum password age in days for a domain
        /// The valid values for the maximum password age: 0, 30, 60, 90, 365
        /// </summary>
        public int? MaxPasswordAge { get; set; }

        /// <summary>
        /// Name of the password setting that determines minimum char sets in password for a domain
        /// The default value for the minimum char sets in password is 4
        /// This property cannot be updated
        /// </summary>
        public int? MinCharsetsInPassword { get; set; }

        /// <summary>
        /// Name of the password setting that determines minimum password length for a domain
        /// The default value for the minimum password length is 12
        /// This property cannot be updated
        /// </summary>
        public int? MinPasswordLength { get; set; }

        /// <summary>
        /// Name of the password setting that number of passwords kept in password history (new password must be different then these) for a domain
        /// The valid values for the password history Length from 0 to 10
        /// </summary>
        public int? PasswordHistoryLength { get; set; }

    }
}

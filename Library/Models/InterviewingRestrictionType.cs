namespace Nfield.SDK.Models
{
    /// <summary>
    /// Specifies what interviews are allowed to continue when the fieldwork is stopped.
    /// </summary>
    public enum InterviewingRestrictionType
    {
        /// <summary>
        /// Not set
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Block all interviews
        /// </summary>
        BlockEverything = 1,

        /// <summary>
        /// Allow only active interviews
        /// </summary>
        AllowOnlyActives = 2,

        /// <summary>
        /// Allow only active and resume interviews
        /// </summary>
        AllowActivesAndResumes = 3,

        /// <summary>
        /// Allow all interviews
        /// </summary>
        AllowEverything = 4,
    }
}

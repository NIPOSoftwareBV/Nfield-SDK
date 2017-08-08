namespace Nfield.Models
{
    namespace NipoSoftware.Nfield.Manager.Api.Models
    {
        /// <summary>
        /// DTO for returning the result of a SampleData block operation
        /// </summary>
        public class SampleBlockStatus
        {
            /// <summary>
            /// Total number of records blocked
            /// </summary>
            public int BlockedCount { get; set; }
        }
    }
}

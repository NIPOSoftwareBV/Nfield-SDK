using System;

namespace Nfield.Models
{
    /// <summary>
    /// Model for Startping fieldwork
    /// </summary>
    public class StopFieldworkModel
    {
        /// <summary>
        /// Indication to terminate running interviews
        /// </summary>
        public bool TerminateRunningInterviews { get; set; }

        /// <summary>
        /// Indication to make 'FieldworkAllowed' false
        /// </summary>
        public bool ResetFieldworkAllowed { get; set; }
    }
}

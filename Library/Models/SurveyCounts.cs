using System;

namespace Nfield.Models
{
    /// <summary>
    /// Object to hold the survey counts
    /// </summary>
    public class SurveyCounts
    {

        /// <summary>
        /// Number of successfully completed interviews
        /// </summary>
        public int? SuccessfulCount { get; set; }

        /// <summary>
        /// Number of the interviews ended with *ENDST or #ENDNGB
        /// </summary>
        public int? ScreenedOutCount { get; set; }

        /// <summary>
        /// Number of dropped out interviews
        /// </summary>
        public int? DroppedOutCount { get; set; }

        /// <summary>
        /// Number of rejected interviews
        /// </summary>
        public int? RejectedCount { get; set; }

        /// <summary>
        /// Number of currently active live interviews
        /// </summary>
        public int? ActiveLiveCount { get; set; }

        /// <summary>
        /// Number of currently active test interviews
        /// </summary>
        public int? ActiveTestCount { get; set; }

        /// <summary>
        /// The detailed counts per quota cell for surveys with quota
        /// </summary>
        public QuotaLevelWithCounts QuotaCounts { get; set; }

    }
}

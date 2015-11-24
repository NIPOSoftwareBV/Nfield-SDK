using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nfield.Models
{
    /// <summary>
    /// Object to hold the survey counts
    /// </summary>
    public class SurveyCounts
    {

        /// <summary>
        /// Number of successfully completed interviews for this interviewer
        /// </summary>
        public int? SuccessfulCount { get; set; }

        /// <summary>
        /// Number of the interviews ended with *ENDST or #ENDNGB
        /// </summary>
        public int? ScreenedOutCount { get; set; }

        /// <summary>
        /// Number of dropped out interviews for this interviewer
        /// </summary>
        public int? DroppedOutCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? RejectedCount { get; set; }

        /// <summary>
        /// The detailed counts per quota cell for surveys with quota
        /// </summary>
        public IEnumerable<SurveyQuotaCountsModel> QuotaCounts { get; set; }

    }
    /// <summary>
    /// Counts per quota level
    /// </summary>
    public class SurveyQuotaCountsModel
    {
        /// <summary>
        /// The quota level
        /// </summary>
        public string QuotaLevelId { get; set; }
        /// <summary>
        /// The count of interviews successfully completed
        /// </summary>
        public int? SuccessfulCount { get; set; }

        /// <summary>
        /// The count of interviews that ended with *ENDST or #ENDNGB
        /// </summary>
        public int? ScreenedOutCount { get; set; }

        /// <summary>
        /// Number of currently timed-out non-test interviews
        /// </summary>
        public int? DroppedOutCount { get; set; }

        /// <summary>
        /// The count of interviews successfully completed
        /// </summary>
        public int? RejectedCount { get; set; }


    }
}

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

    }
}

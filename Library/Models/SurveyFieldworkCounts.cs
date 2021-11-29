//    This file is part of Nfield.SDK.
//
//    Nfield.SDK is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Nfield.SDK is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public License
//    along with Nfield.SDK.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;

namespace Nfield.Models
{
    /// <summary>
    /// Holds the survey fieldwork counts.
    /// </summary>
    public class SurveyFieldworkCounts
    {
        /// <summary>
        /// Survey Identifier
        /// </summary>
        public string SurveyId { get; set; }
        /// <summary>
        /// Survey target
        /// </summary>
        public int? Target { get; set; }
        /// <summary>
        /// Number of successful interviews
        /// </summary>
        public int Successful { get; set; }
        /// <summary>
        /// Number of successful interviews in last 24 hours
        /// </summary>
        public int SuccessfulLast24Hours { get; set; }
        /// <summary>
        /// Number of screened out  interviews
        /// </summary>
        public int ScreenedOut { get; set; }
        /// <summary>
        /// Number of dropped out interviews
        /// </summary>
        public int DroppedOut { get; set; }
        /// <summary>
        /// Number of rejected interviews
        /// </summary>
        public int Rejected { get; set; }
        /// <summary>
        /// Number of successful interviews deleted
        /// </summary>
        public int SuccessfulDeleted { get; set; }
        /// <summary>
        /// Number of screened out interviews deleted
        /// </summary>
        public int ScreenedOutDeleted { get; set; }
        /// <summary>
        /// Number of dropped out interviews deleted
        /// </summary>
        public int DroppedOutDeleted { get; set; }
        /// <summary>
        /// Number of reject interviews deleted
        /// </summary>
        public int RejectedDeleted { get; set; }
        /// <summary>
        /// Number of active interviews
        /// </summary>
        public int ActiveInterviews { get; set; }
        /// <summary>
        /// True if the survey has quota
        /// </summary>
        public bool HasQuota { get; set; }
        /// <summary>
        /// Overview of screen out interviews. Returns counts for each response code.
        /// </summary>
        public IEnumerable<ResponseCodeCount> ScreenedOutOverview { get; set; }
        /// <summary>
        /// Class for the screen out interviews overview
        /// </summary>
        public class ResponseCodeCount
        {
            public int ResponseCode { get; set; }
            public int Count { get; set; }
        }
    }
}

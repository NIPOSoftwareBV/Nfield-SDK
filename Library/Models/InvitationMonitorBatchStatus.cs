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


using System;

namespace Nfield.Models
{
    /// <summary>
    /// Represent the status of an invitation batch
    /// </summary>
    public class InvitationMonitorBatchStatus
    {
        /// <summary>
        /// Name of the Batch
        /// </summary>
        public string BatchName { get; set; }

        /// <summary>
        /// Id of the relative survey
        /// </summary>
        public string SurveyId { get; set; }

        /// <summary>
        /// Batch status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Date and time of when the batch has been scheduled for
        /// </summary>
        public DateTime ScheduledFor { get; set; }

        /// <summary>
        /// Total number of respondents invited with the current batch
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Number of respondents for which an invitation is currently scheduled in the current batch
        /// </summary>
        public int ScheduledCount { get; set; }

        /// <summary>
        /// Number of respondents for which an invitation is currently pending in the current batch
        /// </summary>
        public int PendingCount { get; set; }

        /// <summary>
        /// Number of invitations that haven't been sent in the current batch
        /// </summary>
        public int NotSentCount { get; set; }

        /// <summary>
        /// Number of invitations that resulted in an error in the current batch
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// Number of invitations that have already been sent in the current batch
        /// </summary>
        public int SentCount { get; set; }

        /// <summary>
        /// Number of invitations that have been opened in the current batch
        /// </summary>
        public int OpenedCount { get; set; }

        /// <summary>
        /// Number of invitations that have been clicked in the current batch
        /// </summary>
        public int ClickedCount { get; set; }

        /// <summary>
        /// Number of respondent that have unsubscribed in the current batch
        /// </summary>
        public int UnsubscribedCount { get; set; }

        /// <summary>
        /// Number of respondent that have been reported in the current batch
        /// </summary>
        public int AbuseReportCount { get; set; }

        /// <summary>
        /// Number of invitations with unknown status in the current batch
        /// </summary>
        public int UnknownCount { get; set; }
    }
}

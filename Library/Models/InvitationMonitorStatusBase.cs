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
    /// Base class to monitor the invitation status
    /// </summary>
    public class InvitationMonitorStatusBase
    {
        /// <summary>
        /// Id of the survey
        /// </summary>
        public string SurveyId { get; set; }

        /// <summary>
        /// Total number of respondents invited
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// Number of respondents for which an invitation is currently scheduled
        /// </summary>
        public int ScheduledCount { get; set; }

        /// <summary>
        /// Number of respondents for which an invitation is currently pending
        /// </summary>
        public int PendingCount { get; set; }

        /// <summary>
        /// Number of invitations that haven't been sent
        /// </summary>
        public int NotSentCount { get; set; }

        /// <summary>
        /// Number of invitations that resulted in an error
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// Number of invitations that have already been sent
        /// </summary>
        public int SentCount { get; set; }

        /// <summary>
        /// Number of invitations that have been opened
        /// </summary>
        public int OpenedCount { get; set; }

        /// <summary>
        /// Number of invitations that have been clicked
        /// </summary>
        public int ClickedCount { get; set; }

        /// <summary>
        /// Number of respondent that have unsubscribed
        /// </summary>
        public int UnsubscribedCount { get; set; }

        /// <summary>
        /// Number of respondent that have been reported
        /// </summary>
        public int AbuseReportCount { get; set; }

        /// <summary>
        /// Number of invitations with unknown status
        /// </summary>
        public int UnknownCount { get; set; }
    }
}

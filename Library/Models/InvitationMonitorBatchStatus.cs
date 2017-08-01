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
        public string Name { get; set; }

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
        /// Total number of respondents invited
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Number of respondents for which an invitation is currently scheduled
        /// </summary>
        public int Scheduled { get; set; }

        /// <summary>
        /// Number of respondents for which an invitation is currently pending
        /// </summary>
        public int Pending { get; set; }

        /// <summary>
        /// Number of invitations that haven't been sent
        /// </summary>
        public int NotSent { get; set; }

        /// <summary>
        /// Number of invitations that resulted in an error
        /// </summary>
        public int Error { get; set; }

        /// <summary>
        /// Number of invitations that have already been sent
        /// </summary>
        public int Sent { get; set; }

        /// <summary>
        /// Number of invitations that have been opened
        /// </summary>
        public int Opened { get; set; }

        /// <summary>
        /// Number of invitations that have been clicked
        /// </summary>
        public int Clicked { get; set; }

        /// <summary>
        /// Number of respondent that have unsubscribed
        /// </summary>
        public int Unsubscribed { get; set; }

        /// <summary>
        /// Number of respondent that have been reported
        /// </summary>
        public int AbuseReport { get; set; }

        /// <summary>
        /// Number of invitations with unknown status
        /// </summary>
        public int Unknown { get; set; }
    }
}
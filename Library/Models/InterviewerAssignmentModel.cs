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
using System.Collections.Generic;

namespace Nfield.Models
{
    /// <summary>
    /// Model for interviewer assignment
    /// </summary>
    public class InterviewerAssignmentModel
    {
        /// <summary>
        /// Survey name
        /// </summary>
        public string SurveyName { get; set; }
        /// <summary>
        /// Survey Id
        /// </summary>
        public string SurveyId { get; set; }
        /// <summary>
        /// Interviewer user  name
        /// </summary>
        public string Interviewer { get; set; }
        /// <summary>
        /// Interviewer Id
        /// </summary>
        public string InterviewerId { get; set; }
        /// <summary>
        /// Discriminator
        /// </summary>
        public string Discriminator { get; set; }
        /// <summary>
        /// Assigned or not
        /// </summary>
        public bool IsAssigned { get; set; }
        /// <summary>
        /// Active or not
        /// </summary>
        public bool IsActive { get; set; }
        // Is group assignment
        public bool? IsGroupAssignment { get; set; }
        /// <summary>
        /// Assigned target
        /// </summary>
        public int? AssignedTarget { get; set; }
        /// <summary>
        /// Sampling point target
        /// </summary>
        public int? AssignedSamplingPointTarget { get; set; }
        /// <summary>
        /// Successful count
        /// </summary>
        public int SuccessfulCount { get; set; }
        /// <summary>
        /// Unsuccessful count
        /// </summary>
        public int UnsuccessfulCount { get; set; }
        /// <summary>
        /// Screened out count 
        /// </summary>
        public int ScreenedOutCount { get; set; }
        /// <summary>
        /// Dropped out count
        /// </summary>
        public int DroppedOutCount { get; set; }
        /// <summary>
        /// Rejected count
        /// </summary>
        public int RejectedCount { get; set; }
        /// <summary>
        /// Last sync date
        /// </summary>
        public DateTime? LastSyncDate { get; set; }
        /// <summary>
        /// Is full synced
        /// </summary>
        public bool? IsFullSynced { get; set; }
        /// <summary>
        /// Is last sync successful
        /// </summary>
        public bool? IsLastSyncSuccessful { get; set; }
    }
}

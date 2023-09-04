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
    public class CapiInterviewer
    {
        /// <summary>
        /// Unique Id of Interviewer
        /// </summary>
        public string InterviewerId { get; set; }

        /// <summary>
        /// The interviewer's username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// First name of the interviewer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the interviewer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email Address of the interviewer
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// TelephoneNumber of the interviewer
        /// </summary>
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Property indicating weather the interviewer is a supervisor or not.  
        /// Supervisors have special powers e.g. they can enter and modify the 
        /// device ID in the CAPI client application in the Settings page.
        /// </summary>
        public bool IsSupervisor { get; set; }

        /// <summary>
        /// The InterviewerId as it is used on the client
        /// </summary>
        public string ClientInterviewerId { get; set; }

        /// <summary>
        /// Time the password was last changed
        /// </summary>
        public virtual DateTime? LastPasswordChangeTime { get; set; }

        /// <summary>
        /// The date and time of the last sync
        /// </summary>
        public virtual DateTime? LastSyncDate { get; set; }

        /// <summary>
        /// interviewer full-sync status: true if fully synced, false if not fully synced
        /// </summary>
        public bool IsFullSynced { get; set; }

        /// <summary>
        /// true if last sync was successful, false if not
        /// </summary>
        public bool IsLastSyncSuccessful { get; set; }
      
    }
}

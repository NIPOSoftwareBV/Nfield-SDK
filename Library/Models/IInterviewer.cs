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
    /// This interface represents a user of the Nfield CAPI 
    /// system who conducts interviews in the field
    /// </summary>
    public interface IInterviewer
    {
        /// <summary>
        /// The interviewer unique id
        /// </summary>
        string InterviewerId { get; }

        /// <summary>
        /// The interviewer id as it is used on the CAPI client
        /// </summary>
        string ClientId { get; set; }

        /// <summary>
        /// The username used by the interviewer to signs in to the CAPI client
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// The first name of the interviewer
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// The last name of the interviewer
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// The email address of the interviewer
        /// </summary>
        string EmailAddress { get; set; }

        /// <summary>
        /// The telephone number of the interviewer
        /// </summary>
        string TelephoneNumber { get; set; }

        /// <summary>
        /// The password used by the interviewer to signs in to the CAPI client
        /// </summary>
        string Password { set; }

        /// <summary>
        /// Number of successfully completed interviews for this interviewer
        /// </summary>
        int SuccessfulCount { get; }

        /// <summary>
        /// Number of unsuccessfully completed interviews for this interviewer
        /// </summary>
        int UnsuccessfulCount { get; }

        /// <summary>
        /// Number of dropped out interviews for this interviewer
        /// </summary>
        int DroppedOutCount { get; }

        /// <summary>
        /// The date and time of the last synchronization by the interviewer from CAPI client
        /// </summary>
        DateTime? LastSyncDate { get; }

        /// <summary>
        /// Last time the interviewer's password was changed
        /// </summary>
        DateTime? LastPasswordChangeTime { get; }
    }
}

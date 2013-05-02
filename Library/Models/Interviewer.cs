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
using Newtonsoft.Json;

namespace Nfield.Models
{
    /// <summary>
    /// Holds all properties of an interviewer
    /// </summary>
    public class Interviewer
    {
        /// <summary>
        /// Unique id of the interviewer
        /// </summary>
        [JsonProperty]
        public string InterviewerId { get; internal set; }

        /// <summary>
        /// Public id of the interviewer, is unique within a domain
        /// </summary>
        public string ClientInterviewerId { get; set; }

        /// <summary>
        /// User name interviewer uses to sign in
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email address of interviewer
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Telephone number of interviewer
        /// </summary>
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Password of interviewer, only used when creating an interview
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Total number of successful interviews the interviewer has done
        /// </summary>
        [JsonProperty]
        public int SuccessfulCount { get; internal set; }

        /// <summary>
        /// Total number of unsuccessful interviews the interviewer has done
        /// </summary>
        [JsonProperty]
        public int UnsuccessfulCount { get; internal set; }

        /// <summary>
        /// Total number interviews the interviewer has started and abandoned, eg timeout out
        /// </summary>
        [JsonProperty]
        public int DroppedOutCount { get; internal set; }

        /// <summary>
        /// Last time the device of the interviewer was synchronized with the server
        /// </summary>
        [JsonProperty]
        public DateTime? LastSyncTime { get; internal set; }

        /// <summary>
        /// Last time the password of the interviewer was changed
        /// </summary>
        [JsonProperty]
        public DateTime? LastPasswordChangeTime { get; internal set; }
    }
}
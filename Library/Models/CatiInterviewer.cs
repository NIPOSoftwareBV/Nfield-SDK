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

using Newtonsoft.Json;
using System;

namespace Nfield.Models
{
    public class CatiInterviewer
    {
        /// <summary>
        /// Unique id of the cati interviewer
        /// </summary>
        [JsonProperty]
        public string InterviewerId { get; internal set; }

        /// <summary>
        /// User name cati interviewer uses to sign in
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
        /// Email address of the interviewer
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Telephone number of the interviewer
        /// </summary>
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Password of cati interviewer, only used when creating a cati interviewer
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Last time the password of the cati interviewer was changed
        /// </summary>
        [JsonProperty]
        public DateTime? LastPasswordChangeTime { get; internal set; }
    }
}

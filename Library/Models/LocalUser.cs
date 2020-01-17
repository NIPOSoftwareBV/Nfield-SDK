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
    /// <summary>
    /// Model representing the settable values in on a local user.
    /// </summary>
    public class LocalUserValues
    {
        /// <summary>
        /// Name of user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// First name of user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Role of the user
        /// </summary>
        public string UserRole { get; set; }
    }

    /// <summary>
    /// Model representing the settable values in a survey group.
    /// </summary>
    public class NewLocalUser : LocalUserValues
    {
        /// <summary>
        /// Password of the new user
        /// </summary>
        public string Password { get; set; }
    }

    /// <summary>
    /// Model representing a local user.
    /// </summary>
    public class LocalUser : LocalUserValues
    {
        /// <summary>
        /// The Id of the local user.
        /// </summary>
        [JsonProperty]
        public string Id { get; internal set; }

        /// <summary>
        /// LastLogonDate of the local user to the Nfield System
        /// </summary>
        [JsonProperty]
        public DateTime? LastLogonDate { get; internal set; }
    }
}

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
    /// Represents the response model returned after importing a landing page file.
    /// </summary>
    public class LandingPageUploadStatusResponseModel
    {
        /// <summary>
        /// The ID of the activity associated with the landing page upload.
        /// </summary>
        public string ActivityId { get; set; }

        /// <summary>
        /// The ID of the user who started the activity.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The current status of the activity.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The date and time when the activity was created.
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// The date and time when the activity was started.
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// The date and time when the activity was completed.
        /// </summary>
        public DateTime? FinishTime { get; set; }
    }
}
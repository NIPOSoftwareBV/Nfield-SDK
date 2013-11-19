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
    /// Holds the basic properties of a background task
    /// </summary>
    public class BackgroundTask
    {

        /// <summary>
        /// The unique id of the background task
        /// </summary>
        [JsonProperty]
        public string Id { get; internal set; }


        /// <summary>
        /// Information about the task
        /// This data is always represented with JSON
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// The time the task is created
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// The time the task is finished
        /// </summary>
        public DateTime? FinishTime { get; set; }

        /// <summary>
        /// The url to the blob storage location that holds the result files
        /// </summary>
        public string ResultUrl { get; set; }

        /// <summary>
        /// The status of the task
        /// </summary>
        public BackgroundTaskStatus Status { get; set; }

        /// <summary>
        /// The type of background task
        /// </summary>
        public BackgroundTaskType TaskType { get; set; }

    }

}

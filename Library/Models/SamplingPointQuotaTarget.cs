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
    /// Holds all properties of a sampling point quota target
    /// </summary>
    public class SamplingPointQuotaTarget
    {
        /// <summary>
        /// The Id of the Quota Level
        /// </summary>
        [JsonProperty]
        public string LevelId { get; internal set; }

        /// <summary>
        /// Actual target of the level
        /// </summary>
        public int? Target { get; set; }

        /// <summary>
        /// Number of successfully completed interviews
        /// </summary>
        [JsonProperty]
        public int SuccessfulCount { get; internal set; }

        /// <summary>
        /// Number of not successfully completed interviews
        /// </summary>
        [JsonProperty]
        public int UnsuccessfulCount { get; internal set; }

        /// <summary>
        /// Number of dropped out interviews
        /// </summary>
        [JsonProperty]
        public int DroppedOutCount { get; internal set; }
    }
}

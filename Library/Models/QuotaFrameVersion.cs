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
    /// This model is used to query survey quota frame versions.
    /// </summary>
    public class QuotaFrameVersion
    {
        /// <summary>
        /// The unique identifier for a quota frame version
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The version of the quota frame
        /// </summary>
        [JsonProperty(PropertyName = "eTag")]
        public string ETag { get; set; }

        /// <summary>
        /// The timestamp for when the quota frame was published
        /// </summary>
        public DateTime PublishedDate { get; set; }
    }
}
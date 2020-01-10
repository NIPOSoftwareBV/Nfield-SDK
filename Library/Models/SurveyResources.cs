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
    /// Holds the properties of survey resources
    /// </summary>
    public class SurveyResources : SurveyBase
    {
        [JsonProperty]
        public DateTime? CreationDate { get; internal set; }

        [JsonProperty]
        public string ClientName { get; internal set; }

        [JsonProperty]
        public string Owner { get; internal set; }

        [JsonProperty]
        public DateTime? LastDataDownloadDate { get; internal set; }

        [JsonProperty]
        public DateTime? LastDataCollectionDate { get; internal set; }

        [JsonProperty]
        public long? Size { get; internal set; }

        [JsonProperty]
        public SurveyChannel Channel { get; internal set; }

        [JsonProperty]
        public SurveyStatus State { get; internal set; }

    }
}

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

namespace Nfield.Models
{
    /// <summary>
    /// Holds all properties of a survey
    /// </summary>
    public class Survey
    {
        /// <summary>
        /// Survey ID
        /// </summary>
        [JsonProperty]
        public string SurveyId { get; internal set; }

        /// <summary>
        /// Name of the survey
        /// </summary>
        public string SurveyName { get; set; }

        /// <summary>
        /// The name of the client to which the survey belongs 
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// The description of the survey
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Maximum number of successful interviews that can be done for the survey
        /// </summary>
        public int? Target { get; set; }

        /// <summary>
        /// original ODIN script
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        /// Hierarchical Quota structure of the Survey for CAPI.
        /// This data is always represented with JSON
        /// </summary>
        public string CapiQuota { get; set; }
    }
}

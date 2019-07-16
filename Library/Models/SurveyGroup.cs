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
    /// Model representing the settable values in a survey group.
    /// </summary>
    public class SurveyGroupValues
    {
        /// <summary>
        /// The name of the survey group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description of the survey group.
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// Model representing a survey group.
    /// </summary>
    public class SurveyGroup : SurveyGroupValues
    {
        /// <summary>
        /// The Id of the survey group.
        /// </summary>
        [JsonProperty]
        public int SurveyGroupId { get; internal set; }

        /// <summary>
        /// The date on which the survey group was created (in UTC).
        /// </summary>
        [JsonProperty]
        public DateTime CreationDate { get; internal set; }
    }
}

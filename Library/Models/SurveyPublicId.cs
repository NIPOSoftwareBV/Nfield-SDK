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

namespace Nfield.Models
{
    /// <summary>
    /// Holds a survey public id
    /// </summary>
    public class SurveyPublicId
    {
        /// <summary>
        /// The public id that is used to link to the survey
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The link type.
        /// </summary>
        public string LinkType { get; set; }

        /// <summary>
        /// Whether this id is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Start interview URL
        /// </summary>
        public string Url { get; set; }
    }
}

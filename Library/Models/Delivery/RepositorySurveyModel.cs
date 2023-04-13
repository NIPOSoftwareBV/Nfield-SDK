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

namespace Nfield.SDK.Models.Delivery
{
    /// <summary>
    /// Describes the main properties of a repository survey. 
    /// </summary>
    public class RepositorySurveyModel
    {
        /// <summary>
        /// The id of the survey
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The Id of the survey on the nfield system
        /// </summary>
        public string NfieldSurveyId { get; set; }

        /// <summary>
        /// The date when the survey was added to the repository
        /// </summary>
        public DateTime AddedOn { get; set; }

        /// <summary>
        /// The status of the survey
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Last time the survey was synced
        /// </summary>
        public DateTime? LastSyncedAt { get; set; }
    }
}

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
    /// The repository activity log model.
    /// </summary>
    public class RepositoryActivityLogModel
    {
        /// <summary>
        /// The Id of the survey on the nfield system
        /// </summary>
        public string NfieldSurveyId { get; set; }

        /// <summary>
        /// The survey name
        /// </summary>
        public string SurveyName { get; set; }

        /// <summary>
        /// The activity being logged
        /// </summary>
        public string Activity { get; set; }

        /// <summary>
        /// The timestamp of the activity
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// The name of the user triggering the activity
        /// </summary>
        public string Username { get; set; }
    }
}
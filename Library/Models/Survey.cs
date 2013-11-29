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
    /// Holds the basic properties of a survey
    /// </summary>
    public class Survey
    {
        /// <summary>
        /// Survey Constructor
        /// </summary>
        /// <param name="surveyType">Type of the survey</param>
        public Survey(SurveyType surveyType)
        {
            SurveyType = surveyType;
        }

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
        /// Type of the survey.
        /// </summary>
        public SurveyType SurveyType { get; internal set; }

        /// <summary>
        /// Name of the survey client
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// The description of the survey
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The default interviewer instruction of a survey
        /// </summary>
        public string InterviewerInstruction { get; set; }
    }
}

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
using System.Collections.Generic;

namespace Nfield.Models
{
    /// <summary>
    /// Encapsulation of the parts of a survey that make up a package.
    /// </summary>
    public class SurveyPackage
    {
        /// <summary>
        /// The name of the survey when this package was published
        /// </summary>
        public string SurveyName { get; set; }

        /// <summary>
        /// The Etag for the package
        /// </summary>
        public long ETag { get; set; }

        /// <summary>
        /// The description of the survey when this package was published
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The client of the survey when this package was published
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// The owner of the survey when this package was published
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// The instruction text of the survey when this package was published
        /// </summary>
        public string InterviewerInstructionText { get; set; }

        /// <summary>
        /// The target for online surveys
        /// </summary>
        public int? OnlineTarget { get; set; }

        /// <summary>
        /// Survey response codes.
        /// </summary>
        public IEnumerable<SurveyPackageResponseCode> ResponseCodes { get; set; }

        /// <summary>
        /// Survey languages.
        /// </summary>
        public IEnumerable<SurveyPackageLanguage> Languages { get; set; }

        /// <summary>
        /// Survey relocations.
        /// </summary>
        public IEnumerable<SurveyPackageRelocation> Relocations { get; set; }

        /// <summary>
        /// Survey settings.
        /// </summary>
        public IEnumerable<SurveyPackageSetting> Settings { get; set; }


        /// <summary>
        /// The interviewer instructions file info for CAPI surveys
        /// </summary>
        public SurveyPackageBinaryFile InterviewerInstructionsFile { get; set; }

        /// <summary>
        /// The media files that are associated with the survey
        /// </summary>
        public IEnumerable<SurveyPackageBinaryFile> MediaFiles { get; set; }

        /// <summary>
        /// The md5 of the questionnaire when this package was published
        /// </summary>
        public string QuestionnaireMd5 { get; set; }


    }
}
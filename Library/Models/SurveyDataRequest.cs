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

namespace Nfield.Models
{
    /// <summary>
    /// Fields required to create a survey data download request
    /// </summary>
    public class SurveyDataRequest
    {
        /// <summary>
        /// Optional. Name of the file. The default is the surveyName
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Optional. The start date for the data requested.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Optional. The end date for the data requested.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Optional. The survey version (Etag)
        /// </summary>
        public string SurveyVersion { get; set; }

        /// <summary>
        /// Include in the download the successful interviews
        /// Previously: DownloadSuccessfulLiveInterviewData
        /// </summary>
        public bool IncludeSuccessful { get; set; }

        /// <summary>
        /// Include in the download the screen out interviews
        /// Previously: DownloadNotSuccessfulLiveInterviewData
        /// </summary>
        public bool IncludeScreenOut { get; set; }

        /// <summary>
        /// Include in the download the dropped out interviews
        /// Previously: DownloadSuspendedLiveInterviewData
        /// </summary>
        public bool IncludeDroppedOut { get; set; }

        /// <summary>
        /// Include in the download the rejected interviews
        /// Previously: DownloadRejectedLiveInterviewData
        /// </summary>
        public bool IncludeRejected { get; set; }

        /// <summary>
        /// Include in the download test interviews
        /// Previously: DownloadTestInterviewData
        /// </summary>
        public bool IncludeTestData { get; set; }

        /// <summary>
        /// Include in the download the closed answer data of the interviews
        /// Previously: DownloadClosedAnswerData
        /// </summary>
        public bool IncludeClosedAnswers { get; set; }

        /// <summary>
        /// Include in the download the open answer data of the interviews
        /// Previously: DownloadOpenAnswerData
        /// </summary>
        public bool IncludeOpenAnswers { get; set; }

        /// <summary>
        /// Include in the download the para data of the interviews
        /// Previously: DownloadParaData
        /// </summary>
        public bool IncludeParaData { get; set; }

        /// <summary>
        /// Include in the download the captured media files during the interviews
        /// Previously: DownloadCapturedMedia
        /// </summary>
        public bool IncludeCapturedMediaFiles { get; set; }

        /// <summary>
        /// Include in the download the variables file of the interviews
        /// Previously: DownloadVarFile
        /// </summary>
        public bool IncludeVarFile { get; set; }

        /// <summary>
        /// Include in the download the questionnaire script
        /// Previously: DownloadQuestionnaireScript
        /// </summary>
        public bool IncludeQuestionnaireScript { get; set; }

        /// <summary>
        /// Include in the download the audit log file
        /// </summary>
        public bool IncludeAuditLog { get; set; }
    }
}

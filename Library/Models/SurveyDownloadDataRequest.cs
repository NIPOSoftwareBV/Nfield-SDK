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
    /// Class to specify downloads
    /// </summary>
    public class SurveyDownloadDataRequest
    {
        /// <summary>
        /// The id of the survey
        /// </summary>
        public string SurveyId { get; set; }

        /// <summary>
        /// Download data of test interviews
        /// </summary>
        public bool DownloadTestInterviewData { get; set; }

        /// <summary>
        /// Download data of successful live interviews
        /// </summary>
        public bool DownloadSuccessfulLiveInterviewData { get; set; }

        /// <summary>
        /// Download data of rejected live interviews that were once successful
        /// </summary>
        public bool DownloadRejectedLiveInterviewData { get; set; }

        /// <summary>
        /// Download data of not successful live interviews
        /// </summary>
        public bool DownloadNotSuccessfulLiveInterviewData { get; set; }

        /// <summary>
        /// Download data of suspended live interviews
        /// </summary>
        public bool DownloadSuspendedLiveInterviewData { get; set; }

        /// <summary>
        /// Download the para data for the interviews
        /// </summary>
        public bool DownloadParaData { get; set; }

        /// <summary>
        /// Download the captured media files during the interviews
        /// </summary>
        public bool DownloadCapturedMedia { get; set; }

        /// <summary>
        /// Download the closed answer data for the interviews
        /// </summary>
        public bool DownloadClosedAnswerData { get; set; }

        /// <summary>
        /// Download the open answer data for the interviews
        /// </summary>
        public bool DownloadOpenAnswerData { get; set; }

        /// <summary>
        /// The name specified by the user for the download file name
        /// </summary>
        public string DownloadFileName { get; set; }

        /// <summary>
        /// The start date for the data requested by user.
        /// May be null or empty if no start date is specified.
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// The end date for the data requested by user.
        /// May be null or empty if no end date is specified.
        /// </summary>
        public string EndDate { get; set; }
    }
}

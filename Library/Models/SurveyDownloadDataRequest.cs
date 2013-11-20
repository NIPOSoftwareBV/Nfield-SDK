using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

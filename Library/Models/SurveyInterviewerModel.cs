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
    /// State of interviewer's assignment to a survey
    /// </summary>
    public class SurveyInterviewerModel
    {
        /// <summary>
        /// InterviewerId
        /// </summary>
        public string InterviewerId { get; set; }

        /// <summary>
        /// Whether the interviewer is assigned to this survey
        /// </summary>
        public bool IsAssigned { get; set; }

        /// <summary>
        /// Whether the interviewer has downloaded the survey
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Number of successfully completed interviews for this interviewer on this survey
        /// </summary>
        public int SuccessfulCount { get; set; }

        /// <summary>
        /// Number of unsuccessfully completed interviews for this interviewer on this survey
        /// </summary>
        public int UnsuccessfulCount { get; set; }

        /// <summary>
        /// Number of dropped out interviews for this interviewer on this survey
        /// </summary>
        public int DroppedOutCount { get; set; }

        /// <summary>
        /// Number of rejected interviews for this interviewer on this survey
        /// </summary>
        public int RejectedCount { get; set; }
    }
}
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
    /// Model used for querying interviewer assignments to sampling points
    /// </summary>
    public class InterviewerSamplingPointAssignmentModel
    {
        /// <summary>
        /// ID of the Interviewer
        /// </summary>
        public string InterviewerId { get; set; }

        /// <summary>
        /// UserName of the Interviewer
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// First name of the interviewer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the interviewer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Whether this interviewer is assigned or not
        /// </summary>
        public bool Assigned { get; set; }

        /// <summary>
        /// Whether this interviewer is active
        /// </summary>
        public bool Active { get; set; }
    }
}
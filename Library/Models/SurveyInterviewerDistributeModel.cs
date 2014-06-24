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

using System.Collections.Generic;

namespace Nfield.Models
{
    /// <summary>
    /// The target and the interviewers to distribute.
    /// </summary>
    public class SurveyInterviewerDistributeModel
    {
        /// <summary>
        /// The gross target to distribute
        /// </summary>
        public int SurveyGrossTargetToDistribute { get; set; }

        /// <summary>
        /// The id's of the interviewers to distribute the target to
        /// </summary>
        public IEnumerable<string> InterviewerIds { get; set; }

        /// <summary>
        /// Description of this work package distribution. May be null.
        /// </summary>
        public string Description { get; set; }
    }
}
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
    /// Model for interviewer assignment
    /// </summary>
    public class SurveyInterviewerAssignmentModel
    {  
        /// <summary>
        /// Assignment description
        /// </summary>
        public string Description { get; set; }        
        /// <summary>
        /// Target to distributedescription
        /// </summary>
        public int TargetToDistribute { get; set; }
        /// <summary>
        /// Assignment type
        /// </summary>
        public string AssignmentType { get; set; }
        /// <summary>
        /// Sampling points filter
        /// </summary>
        public IEnumerable<FilterWithOr> SamplingPointsFilter { get; set; }
    }
}
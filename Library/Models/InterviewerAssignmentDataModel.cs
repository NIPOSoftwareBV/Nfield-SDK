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
    /// Model for interviewer assignment
    /// </summary>
    public class InterviewerAssignmentDataModel
    {
        public string SurveyName { get; set; }
        public string SurveyId { get; set; }
        public string Interviewer { get; set; }
        public string InterviewerId { get; set; }

        public string Discriminator { get; set; }
        public bool? Assigned { get; set; }
        public bool? Active { get; set; }

        public bool? IsGroupAssignment { get; set; }
        public int? AssignedTarget { get; set; }
        public int? AssignedSamplingPointTarget { get; set; }

        public int Successful { get; set; }
        public int ScreenedOut { get; set; }
        public int DroppedOut { get; set; }
        public int Rejected { get; set; }

        public DateTime? LastSyncDate { get; set; }
        public bool? IsFullSynced { get; set; }
        public bool? IsLastSyncSuccessful { get; set; }
    }
}

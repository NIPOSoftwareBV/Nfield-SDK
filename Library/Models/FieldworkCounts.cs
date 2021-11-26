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
    /// Holds all properties of a template
    /// </summary>
    public class FieldworkCounts
    {

        public string Id { get; set; }
        public int? Target { get; set; }
        public int Successful { get; set; }
        public int SuccessfulToday { get; set; }
        public int ScreenedOut { get; set; }
        public int DroppedOut { get; set; }
        public int Rejected { get; set; }
        public int SuccessfulDeleted { get; set; }
        public int ScreenedOutDeleted { get; set; }
        public int DroppedOutDeleted { get; set; }
        public int RejectedDeleted { get; set; }
        public int ActiveInterviews { get; set; }
        public bool HasQuota { get; set; }
        public IEnumerable<ResponseCodeCount> ScreenedOutOverview { get; set; }

        public class ResponseCodeCount
        {
            public int ResponseCode { get; set; }
            public int Count { get; set; }
        }
    }
}

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
    /// Object to hold the survey counts
    /// </summary>
    public class SurveyCounts
    {

        /// <summary>
        /// Number of successfully completed interviews
        /// </summary>
        public int? SuccessfulCount { get; set; }

        /// <summary>
        /// Number of the interviews ended with *ENDST or #ENDNGB
        /// </summary>
        public int? ScreenedOutCount { get; set; }

        /// <summary>
        /// Number of dropped out interviews
        /// </summary>
        public int? DroppedOutCount { get; set; }

        /// <summary>
        /// Number of rejected interviews
        /// </summary>
        public int? RejectedCount { get; set; }

        /// <summary>
        /// Number of currently active live interviews
        /// </summary>
        public int? ActiveLiveCount { get; set; }

        /// <summary>
        /// Number of currently active test interviews
        /// </summary>
        public int? ActiveTestCount { get; set; }

        /// <summary>
        /// The detailed counts per quota cell for surveys with quota
        /// </summary>
        public QuotaLevelWithCounts QuotaCounts { get; set; }

    }
}

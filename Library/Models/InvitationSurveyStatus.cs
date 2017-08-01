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
    /// 
    /// </summary>
    public class InvitationSurveyStatus
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SurveyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool InvitationsBlocked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastActivity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Scheduled { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Pending { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int NotSent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Error { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Sent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Opened { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Clicked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Unsubscribed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int AbuseReport { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Unknown { get; set; }
    }
}

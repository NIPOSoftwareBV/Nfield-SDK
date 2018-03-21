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
    /// Represent an overview of the status of all the invitation batches for a survey
    /// </summary>
    public class InvitationMonitorSurveyStatus : InvitationMonitorStatusBase
    {
        /// <summary>
        /// Name of the survey
        /// </summary>
        public string SurveyName { get; set; }

        /// <summary>
        /// True if invitations are blocked
        /// </summary>
        public bool InvitationsBlocked { get; set; }

        /// <summary>
        /// Survey's batches latest activity (most recent ScheduledFor value for the batches)
        /// </summary>
        public DateTime? LastActivity { get; set; }
    }
}

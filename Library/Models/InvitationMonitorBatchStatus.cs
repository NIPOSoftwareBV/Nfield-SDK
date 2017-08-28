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
    /// Represent the status of an invitation batch
    /// </summary>
    public class InvitationMonitorBatchStatus : InvitationMonitorStatusBase
    {
        /// <summary>
        /// Name of the Batch
        /// </summary>
        public string BatchName { get; set; }

        /// <summary>
        /// Batch status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Date and time of when the batch has been scheduled for
        /// </summary>
        public DateTime ScheduledFor { get; set; }
    }
}

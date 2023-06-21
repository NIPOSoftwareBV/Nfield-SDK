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

namespace Nfield.SDK.Models.Delivery
{
    /// <summary>
    /// Model used in Delivery API operations to describe the Repository Subscriptions Logs
    /// </summary>
    public class RepositorySubscriptionLogModel
    {
        /// <summary>
        /// The id of the repository subscription plan
        /// </summary>
        public long PlanId { get; set; }

        /// <summary>
        /// The name of the repository subscription plan
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// The start datetime on the reposity subscription log
        /// </summary>
        public DateTime StartedAt { get; set; }

        /// <summary>
        /// The end datetime on the reposity subscription log
        /// </summary>
        public DateTime? EndedAt { get; set; }

        /// <summary>
        /// The username on the reposity subscription log
        /// </summary>
        public string Username { get; set; }
    }
}

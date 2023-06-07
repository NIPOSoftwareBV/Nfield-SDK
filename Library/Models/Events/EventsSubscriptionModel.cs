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

namespace Nfield.SDK.Models.Events
{
    /// <summary>
    /// Describes the properties of an events subscription. 
    /// </summary>
    public class EventsSubscriptionModel
    {
        /// <summary>
        /// The resource identifier
        /// </summary>
        public string ResourceId { get; set; }

        /// <summary>
        /// The topic name
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// The subscription name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The webhook endpoint
        /// </summary>
        public string WebHookUri { get; set; }

        /// <summary>
        ///  The event types that the subscription is about.
        /// </summary>
        public IEnumerable<string> EventTypes { get; set; }
    }
}
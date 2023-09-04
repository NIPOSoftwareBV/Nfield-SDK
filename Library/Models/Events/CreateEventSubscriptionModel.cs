﻿//    This file is part of Nfield.SDK.
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
using System;

namespace Nfield.SDK.Models.Events
{
    /// <summary>
    /// The model used when creating a new event subscription.
    /// </summary>
    public class CreateEventSubscriptionModel
    {
        /// <summary>
		/// The subscription name.
		/// </summary>
		public string EventSubscriptionName { get; set; }

        /// <summary>
        /// The endpoint to use for the subscription.
        /// </summary>
        public Uri Endpoint { get; set; }

        /// <summary>
        /// The event types that will subscribe to.
        /// </summary>
        public IEnumerable<string> EventTypes { get; set; }
    }
}
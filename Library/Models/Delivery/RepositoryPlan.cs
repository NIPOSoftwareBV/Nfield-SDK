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

namespace Nfield.SDK.Models.Delivery
{
    /// <summary>
    /// Represents a subscription plan for Data Repositories.
    /// </summary>
    public class RepositoryPlan
    {
        /// <summary>
        /// The Id for the plan.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The name of the plan.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A short description explaining what's the plan is about.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The monthly price of the plan.
        /// </summary>
        public Amount Price { get; set; }
    }
}
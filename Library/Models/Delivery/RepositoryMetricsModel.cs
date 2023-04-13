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

namespace Nfield.SDK.Models.Delivery
{
    /// <summary>
    /// Model used in Delivery API operations to describe the Repository Metrics
    /// </summary>
    public class RepositoryMetricsModel
    {
        /// <summary>
        /// The DTU values of the respository
        /// </summary>
        public ResourceMetricModel DTU { get; set; }

        /// <summary>
        /// The Storage sizes of the repository
        /// </summary>
        public ResourceMetricModel Storage { get; set; }
    }
}
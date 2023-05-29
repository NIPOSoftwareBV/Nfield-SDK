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

namespace Nfield.Models
{
    /// <summary>
    /// Survey quota level Targets. Used only for Quota ETag requests and responses
    /// </summary>
    public class QuotaFrameLevelTarget
    {
        /// <summary>
        /// The unique identifier of the quota level
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Required quota target for this level
        /// </summary>
        public int? Target { get; set; }

        /// <summary>
        /// Max quota target for this level
        /// </summary>
        public int? MaxTarget { get; set; }

        /// <summary>
        /// Max Overshoot allowed for this level
        /// </summary>
        public int? MaxOvershoot { get; set; }
    }
}

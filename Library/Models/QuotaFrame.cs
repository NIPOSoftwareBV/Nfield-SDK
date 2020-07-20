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

namespace Nfield.SDK.Models
{
    /// <summary>
    /// Represents survey quota frame.
    /// </summary>
    public class QuotaFrame
    {
        /// <summary>
        /// The unique identifier of the quota frame
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Collection of quota frame variables
        /// </summary>
        public IEnumerable<QuotaFrameVariable> Variables { get; set; }

        /// <summary>
        /// Quota target
        /// </summary>
        public int? Target { get; set; }

        /// <summary>
        /// Successful count
        /// </summary>
        public int Successful { get; set; }
    }
}

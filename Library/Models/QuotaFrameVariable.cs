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
using System.Collections.Generic;

namespace Nfield.SDK.Models
{
    /// <summary>
    /// Represents the quota frame variable
    /// </summary>
    public class QuotaFrameVariable
    {
        /// <summary>
        /// The unique identifier of the variable
        /// </summary>        
        public Guid Id { get; set; }

        /// <summary>
        /// The variable name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Indication if it's a multi variable
        /// </summary>
        public bool IsMulti { get; set; }

        /// <summary>
        /// Collection of levels
        /// </summary>
        public IEnumerable<QuotaFrameLevel> Levels { get; set; }
    }
}

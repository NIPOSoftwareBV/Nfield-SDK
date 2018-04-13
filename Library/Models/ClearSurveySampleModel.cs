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

namespace Nfield.Models
{
    /// <summary>
    /// Model that holds the encrypted data and the initialization vector that was used to encrypt it
    /// </summary>
    public class ClearSurveySampleModel
    {
        /// <summary>
        /// [MANDATORY] Filters to be applied for the clear operation
        /// </summary>
        public IEnumerable<SampleFilter> Filters { get; set; }

        /// <summary>
        /// The name of the columns to be cleared
        /// </summary>
        public IEnumerable<string> Columns { get; set; }
    }
}
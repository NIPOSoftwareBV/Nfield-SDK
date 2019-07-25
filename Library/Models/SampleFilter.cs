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
    /// class to define sample filters
    /// </summary>
    public class SampleFilter
    {
        /// <summary>
        /// Name of a field in a sample record
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Operator, supported operators:
        /// "con" : contains
        /// "ncon": not contains
        /// "eq"  : equals
        /// "lt"  : less than
        /// "gt"  : greater than
        /// "lte" : less or equal
        /// "gte" : greater or equal
        /// "neq" : not equal
        /// </summary>
        public string Op { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public string Value { get; set; }
    }
}

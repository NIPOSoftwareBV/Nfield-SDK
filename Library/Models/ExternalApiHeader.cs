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
    /// Headers for an external api
    /// </summary>
    public class ExternalApiHeader
    {
        /// <summary>
        /// The database id of the header
        /// </summary>
        public int HeaderId { get; set; }

        /// <summary>
        /// Header name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Header value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Indication the value holds an obfuscated value.
        /// When false the value is the real value and will be used in
        /// an update call
        /// </summary>
        public bool IsObfuscated { get; set; }
    }
}
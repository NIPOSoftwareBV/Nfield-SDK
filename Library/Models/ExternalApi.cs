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
    /// Configuration for an external api
    /// </summary>
    public class ExternalApi
    {
        /// <summary>
        /// The name used to reference the external api
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of the api
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The endpoint of the api
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Address for help about the api
        /// </summary>
        public string HelpUri { get; set; }

        /// <summary>
        /// The headers for the api
        /// </summary>
        public IEnumerable<ExternalApiHeader> Headers { get; set; }
    }
}
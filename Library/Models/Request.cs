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
    /// Configuration for a request
    /// </summary>
    public class Request
    {
        /// <summary>
        /// The id of the request
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the request used in the *REQUEST command
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Endpoint (URI) of the request
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// The template of the payload
        /// </summary>
        public string PayloadTemplate { get; set; }

        /// <summary>
        /// The method of the request
        /// </summary>
        public RequestHttpMethod RequestHttpMethod { get; set; }

        /// <summary>
        /// Optional description of the request
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Optional help URI towards extensive documentation for the request
        /// </summary>
        public string HelpUri { get; set; }

        /// <summary>
        /// Collection of headers used when posting the request
        /// </summary>
        public ICollection<RequestHeader> Headers { get; set; }
    }
}
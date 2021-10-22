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
    /// Holds the properties of a Domain response code
    /// </summary>
    public class DomainResponseCode
    {
        /// <summary>
        /// User defined response code for the domain
        /// The code is part of the identity of the Entity so once it's assigned it can not changed
        /// </summary>
        public int Id { get; set; }
   
        /// <summary>
        /// User defined description of the response code given
        /// </summary>
        public string Description { get; set; }        

        /// <summary>
        /// Relocation url of the response code
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings",
            Justification = "Entity Framework, out of the box, can't map Uri's to a sql server data type")]
        public string Url { get; set; }

    }
}
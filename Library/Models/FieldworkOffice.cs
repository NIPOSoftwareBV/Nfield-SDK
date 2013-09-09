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
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Nfield.Models
{
    /// <summary>
    /// Holds all properties of an fieldwork office
    /// </summary>
    public class FieldworkOffice
    {
        /// <summary>
        /// Unique id of the fieldwork office
        /// </summary>
        [JsonProperty]
        public string OfficeId { get; internal set; }

        /// <summary>
        /// Fieldwork office name
        /// </summary>
        public string OfficeName { get; set; }

        /// <summary>
        /// Fieldwork office description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Indicates if the FieldworkOffice is Headquerters
        /// </summary>
        [JsonProperty]
        public bool IsHeadquarters { get; internal set; }
    }
}

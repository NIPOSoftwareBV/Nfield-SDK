﻿//    This file is part of Nfield.SDK.
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

namespace Nfield.Models
{
    /// <summary>
    /// Model to hold address data
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Unique id of the address
        /// </summary>
        public string AddressId { get; set; }

        /// <summary>
        /// Details of the address
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Appointment Date of the address
        /// </summary>
        public DateTime? AppointmentDate { get; set; }

        /// <summary>
        /// SampleData variables for Odin
        /// </summary>
        public IEnumerable<AddressSampleData> SampleData { get; set; }
    }
}

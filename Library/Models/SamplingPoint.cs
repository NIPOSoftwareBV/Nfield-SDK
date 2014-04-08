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
    /// Holds all properties of a sampling point
    /// </summary>
    public class SamplingPoint
    {
        /// <summary>
        /// Gets or sets the sampling point unique identifier.
        /// </summary>
        public string SamplingPointId { get; set; }

        /// <summary>
        /// Gets or sets the name of the sampling point.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the instruction link, this is a link to a pdf blob storage.
        /// </summary>
        public string Instruction { get; set; }

        /// <summary>
        /// Gets or sets the associcated fieldwork office id.
        /// </summary>
        public string FieldworkOfficeId { get; set; }

        /// <summary>
        /// Gets or sets the sampling point group id
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Stratum the sampling point belongs to
        /// </summary>
        public string Stratum { get; set; }

        /// <summary>
        /// Indicates the Kind of the sampling point. The only accepted values are "Regular" and "Spare"
        /// </summary>
        public SamplingPointKind Kind { get; set; }

    }
}
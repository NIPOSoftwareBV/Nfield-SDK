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
using System.Collections.ObjectModel;

namespace Nfield.Models
{
    /// <summary>
    /// Class representing a quota attribute
    /// </summary>
    public class QuotaAttribute
    {
        /// <summary>
        /// Ctor that initialize the Levels collection
        /// </summary>
        public QuotaAttribute()
        {
            Levels = new Collection<QuotaLevel>();
        }

        /// <summary>
        /// Name of the QuotaAttribute
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user-defined variable name to represent the value of an attribute in the questionnaire script
        /// </summary>
        public string OdinVariable { get; set; }

        /// <summary>
        /// Indication of whether this Attribute is mandatory or not
        /// When mandatory, a Level within this Attribute must be selected by the Interviewer
        /// </summary>
        public bool IsSelectionOptional { get; set; }

        /// <summary>
        /// Child Levels of the QuotaAttribute
        /// </summary>
        public ICollection<QuotaLevel> Levels { get; set; }

    }
}
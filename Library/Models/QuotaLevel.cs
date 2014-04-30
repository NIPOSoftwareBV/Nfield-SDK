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
    /// Class representing a quota level
    /// </summary>
    public class QuotaLevel
    {
        private const string RootLevelId = "RootLevelId";
        private const string RootLevelName = "Target";

        /// <summary>
        /// Ctor that initialize the Attributes collection
        /// </summary>
        public QuotaLevel()
        {
            Attributes = new Collection<QuotaAttribute>();
        }

        /// <summary>
        /// Constructor that has a default parameter that indicates if it is the root level.
        /// When a root level is created we should pass <c>true</c> as an argument in all other cases leave it empty
        /// </summary>
        public QuotaLevel(bool isRootLevel) : this()
        {
            if(isRootLevel)
            {
                Id = RootLevelId;
                Name = RootLevelName;
            }
        }

        /// <summary>
        /// Unique Id of the QuotaLevel
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Description of the QuotaLevel
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Target for this level
        /// </summary>
        public int? Target { get; set; }

        /// <summary>
        /// Gross target for this level
        /// </summary>
        public int? GrossTarget { get; set; }

        /// <summary>
        /// Child Attributes of the QuotaLevel
        /// </summary>
        public ICollection<QuotaAttribute> Attributes { get; set; }

    }
}
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

namespace Nfield.SDK.Models
{
    /// <summary>
    /// Model for directory identity
    /// </summary>
    public class DirectoryIdentityModel
    {
        /// <summary>
        /// Tenant id
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// Object id
        /// </summary>
        public Guid ObjectId { get; set; }

        /// <summary>
        /// Object type
        /// </summary>
        public AadObjectType ObjectType { get; set; }
    }
}

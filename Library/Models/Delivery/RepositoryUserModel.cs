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

namespace Nfield.SDK.Models.Delivery
{
    /// <summary>
    /// Model used in Delivery API operations to describe the Repository Users
    /// </summary>
    public class RepositoryUserModel
    {
        /// <summary>
        /// The id of the repository user.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The id of the repository that the user refers to.
        /// </summary>
        public long RepositoryId { get; set; }

        /// <summary>
        /// The repository user name. This should follow the Azure SQL Database user name rules.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description for the user.
        /// </summary>
        public string Description { get; set; }
    }
}
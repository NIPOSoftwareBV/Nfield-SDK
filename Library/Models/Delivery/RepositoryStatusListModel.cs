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
    /// Model used in Delivery API for the Repository status.
    /// </summary>
    public class RepositoryStatusListModel
    {
        /// <summary>
        /// The Id for the status.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the status.
        /// </summary>
        public string Name { get; set; }

    }
}
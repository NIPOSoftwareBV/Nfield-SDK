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

namespace Nfield.SDK.Models.Delivery
{
    /// <summary>
    /// Class that holds all the required data to connect to a repository database
    /// </summary>
    public class RepositoryConnectionInfo
    {
        /// <summary>
        /// The user Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The user password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// The database server of the database name of the user
        /// </summary>
        public string DatabaseServer { get; set; }

        /// <summary>
        /// The database name of the user
        /// </summary>
        public string DatabaseName { get; set; }

    }
}
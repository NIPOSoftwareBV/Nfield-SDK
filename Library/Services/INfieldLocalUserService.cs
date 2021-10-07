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

using Nfield.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Service used to list, update, create and delete local users.
    /// </summary>
    public interface INfieldLocalUserService
    {
        /// <summary>
        /// Retrieve a specific local user
        /// </summary>
        /// <param name="identityId"></param>
        Task<LocalUser> GetAsync(string identityId);

        /// <summary>
        /// Lists all local users in the domain.
        /// </summary>
        Task<IEnumerable<LocalUser>> GetAllAsync();

        /// <summary>
        /// Creates a new local user.
        /// </summary>
        /// <param name="model">Details of the user to create.</param>
        Task<LocalUser> CreateAsync(NewLocalUser model);

        /// <summary>
        /// Updates an existing local user. Null values are ignored.
        /// </summary>
        /// <param name="model">The new values for the local user.</param>
        Task<LocalUser> UpdateAsync(string identityId, LocalUserValues model);

        /// <summary>
        /// Deletes the specified local user.
        /// </summary>
        /// <param name="identityId">The id of the user to delete.</param>
        Task DeleteAsync(string identityId);

        /// <summary>
        /// Reset the specified local user password.
        /// </summary>
        Task ResetAsync(string identityId, ResetLocalUser model);

        /// <summary>
        /// Ask for local users logs and get the link
        /// </summary>
        Task<string> LogsAsync(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Ask for local user logs and get the link
        /// </summary>
        Task<string> LogsAsync(LogQueryModel query);
    }
}

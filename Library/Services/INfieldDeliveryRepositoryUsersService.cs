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

using System.Linq;
using System.Threading.Tasks;
using Nfield.SDK.Models.Delivery;

namespace Nfield.Services
{
    /// <summary>
    /// Set of methods to manage the respository users
    /// </summary>
    public interface INfieldDeliveryRepositoryUsersService
    {
        /// <summary>
        /// Returns the list of repository users.
        /// </summary>
        /// <param name="repositoryId">The repository id.</param>
        /// <returns>A list a repository users.</returns>
        Task<IQueryable<RepositoryUserModel>> QueryAsync(long repositoryId);

        /// <summary>
        /// Gets a repository user by specified id.
        /// </summary>
        /// <param name="repositoryId">The repository id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>A repository user.</returns>
        Task<RepositoryUserModel> GetAsync(long repositoryId, long userId);

        /// <summary>
        /// Creates a new repository user based on the given data.
        /// </summary>
        /// <param name="repositoryId">The repository id.</param>
        Task<string> PostAsync(long repositoryId, string repositoryUserName);

        /// <summary>
        /// Resets the repository user password.
        /// </summary>
        /// <param name="repositoryId">The repository id.</param>
        /// <param name="userId">The repository user id.</param>
        Task<string> PostAsync(long repositoryId, long userId);

        /// <summary>
        /// Deletes the repository user.
        /// </summary>
        /// <param name="repositoryId">The repository id.</param>
        /// <param name="userId">The repository user id.</param>
        Task DeleteAsync(long repositoryId, long userId);
    }
}

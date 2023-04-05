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
    /// Set of methods to manage the respositories
    /// </summary>
    public interface INfieldDeliveryRepositoriesService
    {
        /// <summary>
        /// Gets a list of repositories for the domain.
        /// </summary>
        /// <returns>A list of repositories or an empty list if none exists.</returns>
        Task<IQueryable<RepositoryModel>> QueryAsync();

        /// <summary>
        /// Gets the repository for the provided identifier.
        /// </summary>
        /// <param name="id">The repository id.</param>
        /// <returns>The repository associated to the identifier. If the repository doesn't exist, returns a NotFound response.</returns>
        Task<RepositoryModel> GetAsync(long repositoryId);

        /// <summary>
        /// Returns the credentials (including server information) to connect to a repository database.
        /// </summary>
        /// <param name="repositoryId">The repository id.</param>
        /// <returns>The repository database credentials.</returns>
        Task<RepositoryConnectionInfo> GetCredentialsAsync(long repositoryId);

        /// <summary>
        /// Gets performance metrics for the repository
        /// </summary>
        /// <param name="repositoryId">The repository id.</param>
        /// <param name="interval">The time interval type (1: for the last day's metrics, 2: for the last week's metrics).</param>
        /// <returns>The repository metrics.</returns>
        Task<RepositoryMetricsModel> GetMetricsAsync(long repositoryId, int interval);


        /// <summary>
        /// Gets repository subscription logs.
        /// </summary>
        /// <param name="repositoryId">The repository id.</param>
        /// <returns>The subscription logs.</returns>
        Task<IQueryable<RepositorySubscriptionLogModel>> QuerySubscriptionsLogsAsync(long repositoryId);

        /// <summary>
        /// Gets repository activity logs.
        /// </summary>
        /// <param name="repositoryId">The repository id.</param>
        /// <returns>The activity logs.</returns>
        Task<IQueryable<RepositoryActivityLogModel>> QueryActivityLogsAsync(long repositoryId);

        /// <summary>
        /// Provisions a new data repository.
        /// </summary>
        /// <returns>The repository id, if succeeded. The appropriate exception in case of failure.</returns>
        Task<RepositoryModel> PostAsync(CreateRepositoryModel model);

        /// <summary>
        /// Changes the subscription plan of a repository.
        /// </summary>
        /// <param name="repositoryId">The repository Id.</param>
        /// <returns><c>AcceptedResult</c>, if succeeded. The appropriate exception in case of failure. BadRequest in case of error.</returns>
        Task PostRepositorySubscriptionAsync(long repositoryId, CreateRepositorySubscriptionModel model);

        /// <summary>
        /// Deletes an existing data repository.
        /// </summary>
        /// <param name="id">The repository id.</param>
        ///<returns> As this is a long running operation, it returns an <c>Accepted</c> response. The appropriate exception will be thrown in case of failure.</returns>
        Task DeleteAsync(long repositoryId);

    }
}

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
    /// 
    /// </summary>
    public interface INfieldDeliveryRepositoriesService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<RepositoryModel>> QueryAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <returns></returns>
        Task<RepositoryModel> GetAsync(long repositoryId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <returns></returns>
        Task<RepositoryConnectionInfo> GetCredentialsAsync(long repositoryId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        Task<RepositoryMetricsModel> GetMetricsAsync(long repositoryId, int interval);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <returns></returns>
        Task<IQueryable<RepositorySubscriptionLogModel>> QuerySubscriptionsLogsAsync(long repositoryId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <returns></returns>
        Task<IQueryable<RepositoryActivityLogModel>> QueryActivityLogsAsync(long repositoryId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task PostAsync(CreateRepositoryModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task PostRepositorySubscriptionAsync(long repositoryId, CreateRepositorySubscriptionModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <returns></returns>
        Task DeleteAsync(long repositoryId);

    }
}

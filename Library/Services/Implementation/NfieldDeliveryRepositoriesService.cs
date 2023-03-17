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
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.SDK.Models.Delivery;
using Nfield.Services;

namespace Nfield.SDK.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldDeliveryRepositoriesService"/>
    /// </summary>
    internal class NfieldDeliveryRepositoriesService : INfieldDeliveryRepositoriesService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldDeliveryRepositoriesService

        public Task<IQueryable<RepositoryModel>> GetAsync()
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, "Delivery/Repositories");

            return ConnectionClient.Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<RepositoryModel>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        public Task<RepositoryModel> GetAsync(long repositoryId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}");

            return ConnectionClient.Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<RepositoryModel>(stringTask.Result))
                         .FlattenExceptions();
        }

        public Task<RepositoryConnectionInfo> GetCredentialsAsync(long repositoryId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}/Credentials");

            return ConnectionClient.Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<RepositoryConnectionInfo>(stringTask.Result))
                         .FlattenExceptions();
        }

        public Task<RepositoryMetricsModel> GetMetricsAsync(long repositoryId, int interval)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}/Metrics/{interval}");

            return ConnectionClient.Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<RepositoryMetricsModel>(stringTask.Result))
                         .FlattenExceptions();
        }

        public Task<IQueryable<RepositorySubscriptionLogModel>> GetSubscriptionsLogsAsync(long repositoryId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}/Logs/Subscriptions");

            return ConnectionClient.Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<RepositorySubscriptionLogModel>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        public Task<IQueryable<RepositoryActivityLogModel>> GetActivityLogsAsync(long repositoryId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}/Logs/Activities");

            return ConnectionClient.Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<RepositoryActivityLogModel>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        public Task PostAsync(CreateRepositoryModel model)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, "Delivery/Repositories");

            return ConnectionClient.Client.PostAsJsonAsync(uri, model)
                         //.ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         //.ContinueWith(task => JsonConvert.DeserializeObject<CreateRepositoryModel>(task.Result))
                         .FlattenExceptions();
        }

        public Task PostRepositorySubscriptionAsync(long repositoryId, CreateRepositorySubscriptionModel model)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}/Subscriptions");

            return ConnectionClient.Client.PostAsJsonAsync(uri, model)
                         //.ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         //.ContinueWith(task => JsonConvert.DeserializeObject<CreateRepositorySubscriptionModel>(task.Result))
                         .FlattenExceptions();
        }

        public Task DeleteAsync(long repositoryId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}");

            return ConnectionClient.Client.DeleteAsync(uri).FlattenExceptions();
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion
    }
}

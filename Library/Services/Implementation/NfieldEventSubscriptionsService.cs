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
using Nfield.SDK.Models.Events;
using Nfield.Services;

namespace Nfield.SDK.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldEventSubscriptionsService"/>
    /// </summary>
    internal class NfieldEventSubscriptionsService : INfieldEventSubscriptionsService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldEventSubscriptionsService

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Task<IQueryable<EventSubscriptionModel>> QueryAsync()
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, "Events/Subscriptions");

            return ConnectionClient.Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             listTask =>
                             JsonConvert.DeserializeObject<List<EventSubscriptionModel>>(listTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Task<EventSubscriptionModel> GetAsync(string subscriptionName)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Events/Subscriptions/{subscriptionName}");

            return ConnectionClient.Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             eventResultTask =>
                             JsonConvert.DeserializeObject<EventSubscriptionModel>(eventResultTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Task<EventSubscriptionModel> CreateAsync(CreateEventSubscriptionModel model)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, "Events/Subscriptions");

            return ConnectionClient.Client.PostAsJsonAsync(uri, model)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<EventSubscriptionModel>(task.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Task UpdateAsync(string subscriptionName, UpdateEventSubscriptionModel model)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Events/Subscriptions/{subscriptionName}");

            return ConnectionClient.Client
                .PatchAsJsonAsync(uri, model)
                .FlattenExceptions();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Task DeleteAsync(string subscriptionName)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Events/Subscriptions/{subscriptionName}");

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

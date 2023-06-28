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
    /// Implementation of <see cref="INfieldDeliveryRepositoryFirewallRulesService"/>
    /// </summary>
    internal class NfieldDeliveryRepositoryFirewallRulesService : INfieldDeliveryRepositoryFirewallRulesService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldDeliveryRepositoryFirewallRulesService

        public Task<IQueryable<FirewallRuleModel>> QueryAsync(long repositoryId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}/FirewallRules");

            return ConnectionClient.Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<FirewallRuleModel>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        public Task<FirewallRuleModel> GetAsync(long repositoryId, int firewallRuleId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}/FirewallRules/{firewallRuleId}");

            return ConnectionClient.Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<FirewallRuleModel>(stringTask.Result))
                         .FlattenExceptions();
        }

        public Task<FirewallRuleModel> PostAsync(long repositoryId, FirewallRuleModel model)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}/FirewallRules");

            return ConnectionClient.Client.PostAsJsonAsync(uri, model)
                        .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                        .ContinueWith(task => JsonConvert.DeserializeObject<FirewallRuleModel>(task.Result))
                        .FlattenExceptions();
        }

        public Task DeleteAsync(long repositoryId, int firewallRuleId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}/FirewallRules/{firewallRuleId}");

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

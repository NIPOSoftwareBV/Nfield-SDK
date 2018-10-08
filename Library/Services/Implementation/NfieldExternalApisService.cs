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

using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    internal class NfieldExternalApisService : INfieldExternalApisService, INfieldConnectionClientObject
    {

        /// <summary>
        /// See <see cref="INfieldExternalApisService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<ExternalApi>> QueryAsync()
        {
            return Client.GetAsync(ExternalApisApi)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<ExternalApi>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldExternalApisService.AddAsync"/>
        /// </summary>
        public Task<ExternalApi> AddAsync(ExternalApi externalApi)
        {
            if (externalApi == null)
            {
                throw new ArgumentNullException("externalApi");
            }

            return Client.PostAsJsonAsync(ExternalApisApi, externalApi)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<ExternalApi>(task.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldExternalApisService.RemoveAsync"/>
        /// </summary>
        public Task RemoveAsync(ExternalApi externalApi)
        {
            if (externalApi == null)
            {
                throw new ArgumentNullException("externalApi");
            }

            return
                Client.DeleteAsync(new Uri(ExternalApisApi, externalApi.Name))
                      .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldExternalApisService.UpdateAsync"/>
        /// </summary>
        public Task<ExternalApi> UpdateAsync(ExternalApi externalApi)
        {
            if (externalApi == null)
            {
                throw new ArgumentNullException("externalApi");
            }

            return Client.PutAsJsonAsync(ExternalApisApi, externalApi)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<ExternalApi>(task.Result))
                         .FlattenExceptions();
        }


        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri ExternalApisApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri, "externalapis"); }
        }
    }
}

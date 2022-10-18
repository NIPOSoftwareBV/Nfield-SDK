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
    internal class NfieldRequestsService : INfieldRequestsService, INfieldConnectionClientObject
    {
        /// <inheritdoc/>
        public Task<IQueryable<Request>> QueryAsync()
        {
            return Client
                .GetAsync(RequestsApi)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult())
                .ContinueWith(stringTask => JsonConvert.DeserializeObject<List<Request>>(stringTask.GetAwaiter().GetResult()).AsQueryable())
                .FlattenExceptions();
        }

        /// <inheritdoc/>
        public Task<Request> AddAsync(Request request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return Client
                .PostAsJsonAsync(RequestsApi, request)
                .ContinueWith(task => task.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult())
                .ContinueWith(task => JsonConvert.DeserializeObject<Request>(task.GetAwaiter().GetResult()))
                .FlattenExceptions();
        }

        /// <inheritdoc/>
        public Task RemoveAsync(Request request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return Client
                .DeleteAsync(new Uri(RequestsApi, request.Id.ToString()))
                .FlattenExceptions();
        }

        /// <inheritdoc/>
        public Task<Request> UpdateAsync(Request request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return Client
                .PutAsJsonAsync(RequestsApi, request)
                .ContinueWith(task => task.Result.Content.ReadAsStringAsync().GetAwaiter().GetResult())
                .ContinueWith(task => JsonConvert.DeserializeObject<Request>(task.GetAwaiter().GetResult()))
                .FlattenExceptions();
        }

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
            => ConnectionClient = connection;

        #endregion

        private INfieldHttpClient Client => ConnectionClient.Client;

        private Uri RequestsApi => new Uri(ConnectionClient.NfieldServerUri, "requests/");
    }
}

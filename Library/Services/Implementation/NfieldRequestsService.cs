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
        public async Task<IQueryable<Request>> QueryAsync()
        {
            using (var response = await Client.GetAsync(RequestsApi))
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Request>>(result).AsQueryable();
            }
        }

        /// <inheritdoc/>
        public async Task<Request> AddAsync(Request request)
        {
            using (var response = await Client.PostAsJsonAsync(RequestsApi, request ?? throw new ArgumentNullException(nameof(request))))
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Request>(result);
            }
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(Request request)
            => await Client.DeleteAsync(new Uri(RequestsApi, request?.Id.ToString() ?? throw new ArgumentNullException(nameof(request))));

        /// <inheritdoc/>
        public async Task<Request> UpdateAsync(Request request)
        {
            using (var response = await Client.PutAsJsonAsync(RequestsApi, request ?? throw new ArgumentNullException(nameof(request))))
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Request>(result);
            }
        }

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
            => ConnectionClient = connection;

        #endregion

        private INfieldHttpClient Client
            => ConnectionClient.Client;

        private Uri RequestsApi
            => new Uri(ConnectionClient.NfieldServerUri, "requests/");
    }
}

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
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;
using Nfield.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldLocalUserService"/>
    /// </summary>
    internal class NfieldLocalUserService : INfieldLocalUserService, INfieldConnectionClientObject
    {
        public INfieldConnectionClient ConnectionClient { get; internal set; }
        public void InitializeNfieldConnection(INfieldConnectionClient connection) => ConnectionClient = connection;

        /// <summary>
        /// <see cref="INfieldLocalUserService.GetAllAsync"/>
        /// </summary>
        public async Task<IEnumerable<LocalUser>> GetAllAsync()
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, "LocalUsers");

            using (var response = await ConnectionClient.Client.GetAsync(uri))
            {
                return await DeserializeJsonAsync<List<LocalUser>>(response);
            }
        }

        /// <summary>
        /// <see cref="INfieldLocalUserService.GetAsync"/>
        /// </summary>
        public async Task<LocalUser> GetAsync(string identityId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"LocalUsers/{identityId}");

            using (var response = await ConnectionClient.Client.GetAsync(uri))
            {
                return await DeserializeJsonAsync<LocalUser>(response);
            }
        }

        /// <summary>
        /// <see cref="INfieldLocalUserService.CreateAsync"/>
        /// </summary>
        public async Task<LocalUser> CreateAsync(NewLocalUser model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var uri = new Uri(ConnectionClient.NfieldServerUri, "LocalUsers");

            using (var response = await ConnectionClient.Client.PostAsJsonAsync(uri, model))
            {
                var result = await DeserializeJsonAsync<LocalUser>(response);

                return result;
            }
        }

        /// <summary>
        /// <see cref="INfieldLocalUserService.UpdateAsync"/>
        /// </summary>
        public async Task<LocalUser> UpdateAsync(string identityId, LocalUserValues model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"LocalUsers/{identityId}");

            using (var response = await ConnectionClient.Client.PatchAsJsonAsync(uri, model))
            {
                var result = await DeserializeJsonAsync<LocalUser>(response);

                return result;
            }
        }

        /// <summary>
        /// <see cref="INfieldLocalUserService.ResetAsync"/>
        /// </summary>
        public async Task ResetAsync(string identityId, ResetLocalUser model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"LocalUsers/Password/{identityId}");
            using (await ConnectionClient.Client.PatchAsJsonAsync(uri, model).ConfigureAwait(false));
        }

        /// <summary>
        /// <see cref="INfieldLocalUserService.DeleteAsync"/>
        /// </summary>
        public async Task DeleteAsync(string identityId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"LocalUsers/{identityId}");

            // note: we need to dispose the response even when we don't use it
            using (await ConnectionClient.Client.DeleteAsync(uri).ConfigureAwait(false));            
        }

        /// <summary>
        /// <see cref="INfieldLocalUserService.LogsAsync"/>
        /// </summary>
        public async Task<string> LogsAsync(LogQueryModel query)
        {
            Ensure.ArgumentNotNull(query, nameof(query));

            var uri = new Uri(ConnectionClient.NfieldServerUri, "LocalUsersLogs");

            return await ConnectionClient.Client.PostAsJsonAsync(uri, query)
                          .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                          .ContinueWith(task => JsonConvert.DeserializeObject<BackgroundActivityStatus>(task.Result))
                          .ContinueWith(task => ConnectionClient.GetActivityResultAsync<string>(task.Result.ActivityId, "DownloadDataUrl").Result)
                          .FlattenExceptions().ConfigureAwait(true);
        }

        /// <summary>
        /// <see cref="INfieldLocalUserService.DeserializeJsonAsync"/>
        /// </summary>
        private async Task<T> DeserializeJsonAsync<T>(HttpResponseMessage response)
        {
            using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var serializer = new JsonSerializer();
                    return serializer.Deserialize<T>(jsonReader);
                }
            }
        }
    }
}
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
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Utilities;

namespace Nfield.Services.Implementation
{
    internal class NfieldDomainSearchFieldsSettingService : INfieldDomainSearchFieldsSettingService, INfieldConnectionClientObject
    {
        public Task<DomainSearchFieldsSetting> GetAsync()
        {
            var uri = DomainSearchFieldsSettingUrl();

            return Client.GetAsync(uri)
                         .ContinueWith(task => JsonConvert.DeserializeObject<DomainSearchFieldsSetting>(
                            task.Result.Content.ReadAsStringAsync().Result))
                         .FlattenExceptions();
        }

        public Task<DomainSearchFieldsSetting> PutAsync(DomainSearchFieldsSetting settings)
        {
            Ensure.ArgumentNotNull(settings, nameof(settings));

            var uri = DomainSearchFieldsSettingUrl();

            return Client.PutAsJsonAsync(uri, settings)
                        .ContinueWith(task => JsonConvert.DeserializeObject<DomainSearchFieldsSetting>(
                            task.Result.Content.ReadAsStringAsync().Result))
                        .FlattenExceptions();
        }

        private INfieldHttpClient Client => ConnectionClient.Client;
        public INfieldConnectionClient ConnectionClient { get; internal set; }
        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        private Uri DomainSearchFieldsSettingUrl()
        {
            return new Uri(ConnectionClient.NfieldServerUri, "SearchFieldsSetting");
        }
    }
}

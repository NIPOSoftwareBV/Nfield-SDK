﻿//    This file is part of Nfield.SDK.
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
using Nfield.Utilities;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldBlacklistService"/>
    /// </summary>
    internal class NfieldBlacklistService : INfieldBlacklistService, INfieldConnectionClientObject
    {
        public async Task<string> GetAsync()
        {
            var uri = BlacklistUrl();

            using (var response = await Client.GetAsync(uri).ConfigureAwait(false))
            {
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }

        public async Task<BlacklistUploadStatus> PostAsync(string blacklist)
        {
            Ensure.ArgumentNotNullOrEmptyString(blacklist, nameof(blacklist));

            var uri = BlacklistUrl();
            var blacklistContent = new StringContent(blacklist);

            using (var response = await Client.PostAsync(uri, blacklistContent).ConfigureAwait(false))
            {
                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<BlacklistUploadStatus>(result);
            }
        }

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private INfieldHttpClient Client => ConnectionClient.Client;

        private Uri BlacklistUrl()
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Blacklist/");
        }
    }
}

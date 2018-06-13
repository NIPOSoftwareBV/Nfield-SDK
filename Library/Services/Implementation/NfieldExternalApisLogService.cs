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
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldExternalApisLogService"/>
    /// </summary>
    internal class NfieldExternalApisLogService : INfieldExternalApisLogService, INfieldConnectionClientObject
    {
        /// <summary>
        /// See <see cref="INfieldExternalApisLogService.PostAsync"/>
        /// </summary>
        public Task<string> PostAsync(ExternalApiLogDownload logDownloadRequest)
        {
            Ensure.ArgumentNotNull(logDownloadRequest, nameof(logDownloadRequest));

            return Client.PostAsJsonAsync(ExternalApiDownloadLogUri.AbsoluteUri, logDownloadRequest)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<BackgroundActivityStatus>(task.Result))
                         .ContinueWith(task => ConnectionClient.GetActivityResultAsync<string>(task.Result.ActivityId, "DownloadDataUrl").Result)
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

        private Uri ExternalApiDownloadLogUri
        {
            get { return new Uri(ConnectionClient.NfieldServerUri.AbsoluteUri + "externalapilogdownload/"); }
        }
    }
}

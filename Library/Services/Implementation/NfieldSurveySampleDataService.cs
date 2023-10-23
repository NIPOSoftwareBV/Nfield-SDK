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
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;
using Nfield.Utilities;
using System;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveySampleDataService"/>
    /// </summary>
    internal class NfieldSurveySampleDataService : INfieldSurveySampleDataService, INfieldConnectionClientObject
    {

        public async Task<string> PrepareDownloadSampleDataAsync(string surveyId, string fileName)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(fileName, nameof(fileName));

            var uri = SurveySampleDataDownloadUrl(surveyId, fileName);

            var response = await Client.PostAsync(uri, null).ConfigureAwait(false);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var backgroundActivityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(result);
            return await ConnectionClient.GetActivityResultAsync<string>(backgroundActivityStatus.ActivityId, "DownloadDataUrl").ConfigureAwait(false);
        }

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private INfieldHttpClient Client => ConnectionClient.Client;

        /// <summary>
        /// Used for new glu surveys data downloads
        /// </summary>
        private Uri SurveySampleDataDownloadUrl(string surveyId, string fileName)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/SampleDataDownload/{fileName}");
        }
    }
}

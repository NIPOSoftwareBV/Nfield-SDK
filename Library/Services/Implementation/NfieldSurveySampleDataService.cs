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
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;
using Nfield.Utilities;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveySampleDataService"/>
    /// </summary>
    internal class NfieldSurveySampleDataService : INfieldSurveySampleDataService, INfieldConnectionClientObject
    {
        /// <summary>
        /// See <see cref="INfieldSurveySampleDataService.GetAsync"/>
        /// </summary>
        public Task<BackgroundTask> GetAsync(string surveyId, string fileName)
        {
            CheckRequiredStringArgument(surveyId, nameof(surveyId));
            CheckRequiredStringArgument(fileName, nameof(fileName));

            var uri = SurveySampleDataUrl(surveyId, fileName);

            return Client.GetAsync(uri)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<BackgroundTask>(task.Result))
                         .FlattenExceptions();
        }

        public async Task<BackgroundActivityStatus> PrepareDownloadSampleDataAsync(string surveyId, string fileName)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(fileName, nameof(fileName));

            var uri = SurveySampleDataDownloadUrl(surveyId, fileName);

            using (var response = await Client.PostAsync(uri, null))
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BackgroundActivityStatus>(result);
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

        /// <summary>
        /// Used for deprecated classic surveys data downloads
        /// </summary>
        private Uri SurveySampleDataUrl(string surveyId, string fileName)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/SampleData/{fileName}");
        }

        /// <summary>
        /// Used for new glu surveys data downloads
        /// </summary>
        private Uri SurveySampleDataDownloadUrl(string surveyId, string fileName)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/SampleDataDownload/{fileName}");
        }

        private static void CheckRequiredStringArgument(string argument, string name)
        {
            if (argument == null)
                throw new ArgumentNullException(name);
            if (argument.Trim().Length == 0)
                throw new ArgumentException($"{name} cannot be empty");
        }

    }
}

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
using Nfield.Utilities;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveyLandingPageService"/>
    /// </summary>
    internal class NfieldSurveyLandingPageService : INfieldSurveyLandingPageService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldLandingPageService

        /// <summary>
        /// Uploads a landing page zip file for a specific survey from a file path.
        /// </summary>
        /// <param name="surveyId">The ID of the survey.</param>
        /// <param name="filePath">The path to the zip file.</param>
        /// <returns>The activity ID of the upload operation.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException">In case that the file does not exist</exception>
        /// <exception cref="T:System.AggregateException"></exception>
        public async Task<SurveyLandingPageUploadStatusResponseModel> UploadLandingPageAsync(string surveyId, string filePath)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(filePath, nameof(filePath));

            var fileName = Path.GetFileName(filePath);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file {filePath} does not exist.");
            }

            using (var fileStream = File.OpenRead(filePath))
            {
                return await DoUploadLandingPageAsync(surveyId, fileName, fileStream).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Uploads a landing page zip file for a specific survey.
        /// </summary>
        /// <param name="surveyId">The ID of the survey.</param>
        /// <param name="fileName">The name of the zip file.</param>
        /// <param name="content">The content of the zip file as a stream.</param>
        /// <returns>The activity ID of the upload operation.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="T:System.AggregateException"></exception>
        public async Task<SurveyLandingPageUploadStatusResponseModel> UploadLandingPageAsync(string surveyId, string fileName, Stream content)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(fileName, nameof(fileName));
            Ensure.ArgumentNotNull(content, nameof(content));

            return await DoUploadLandingPageAsync(surveyId, fileName, content).ConfigureAwait(false);
        }

        /// <summary>
        /// Exports the landing page for a specific survey.
        /// </summary>
        /// <param name="surveyId">The ID of the survey.</param>
        /// <returns>The export status and download URL.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<SurveyLandingPageExportStatusResponseModel> ExportLandingPageAsync(string surveyId)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));

            var response = await Client.GetAsync(LandingPageApi(surveyId)).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<SurveyLandingPageExportStatusResponseModel>(responseString);
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        #region Private Methods

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri LandingPageApi(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/landingPage/");
        }

        private async Task<SurveyLandingPageUploadStatusResponseModel> DoUploadLandingPageAsync(string surveyId, string fileName, Stream content)
        {
            using (var formData = new MultipartFormDataContent())
            {
                var fileContent = new StreamContent(content);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
                formData.Add(fileContent, "file", EnsureZipExtension(fileName));
                formData.Add(new StringContent(surveyId), "surveyId");

                var response = await Client.PostAsync(LandingPageApi(surveyId), formData).ConfigureAwait(false);
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<SurveyLandingPageUploadStatusResponseModel>(responseString);

                await ConnectionClient.GetActivityResultAsync<string>(result.ActivityId, "Status").ConfigureAwait(false);

                return result;
            }
        }

        private static string EnsureZipExtension(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return extension?.ToUpperInvariant() == ".ZIP" ? fileName : $"{fileName}.zip";
        }

        #endregion
    }
}
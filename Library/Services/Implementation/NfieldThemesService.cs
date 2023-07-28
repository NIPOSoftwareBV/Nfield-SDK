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
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldThemesService"/>
    /// </summary>
    internal class NfieldThemesService : INfieldThemesService, INfieldConnectionClientObject
    {

        #region Implementation of INfieldThemesService

        public async Task UploadThemeAsync(string templateId, string themeName, string filePath)
        {
            Ensure.ArgumentNotNullOrEmptyString(templateId, nameof(templateId));
            Ensure.ArgumentNotNullOrEmptyString(themeName, nameof(themeName));
            Ensure.ArgumentNotNullOrEmptyString(filePath, nameof(filePath));

            var fileName = Path.GetFileName(filePath);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(fileName);
            }

            using (var themeData = File.OpenRead(filePath))
            {
                await DoUploadThemeAsync(templateId, themeName, themeData).ConfigureAwait(false);
            }
        }

        public async Task UploadThemeAsync(string templateId, string themeName, Stream themeData)
        {
            Ensure.ArgumentNotNullOrEmptyString(templateId, nameof(templateId));
            Ensure.ArgumentNotNullOrEmptyString(themeName, nameof(themeName));
            Ensure.ArgumentNotNull(themeData, nameof(themeData));

            await DoUploadThemeAsync(templateId, themeName, themeData).ConfigureAwait(false);
        }

        public async Task RemoveAsync(string themeId)
        {
            Ensure.ArgumentNotNullOrEmptyString(themeId, nameof(themeId));

            var uri = GetThemesUri(themeId);
            await Client.DeleteAsync(uri).FlattenExceptions().ConfigureAwait(false);            
        }

        public async Task<string> DownloadThemeAsync(string themeId)
        {
            Ensure.ArgumentNotNullOrEmptyString(themeId, nameof(themeId));

            var uri = GetThemesUri(themeId);

            var response = await Client.GetAsync(uri).FlattenExceptions().ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        #region private methods
        /// <summary>
        /// Get theme uri based on the provided <paramref name="themeId"/>
        /// </summary>
        private Uri GetThemesUri(string themeId)
        {            
            return new Uri(ConnectionClient.NfieldServerUri, $"Themes/{themeId}");
        }

        /// <summary>
        /// Get Upload theme uri based on the provided <paramref name="themeId"/>, <paramref name="templateId"/> and <paramref name="themeName"/>
        /// </summary>
        private Uri GetUploadThemeUri(string templateId, string themeName)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Themes?templateId={templateId}&themeName={themeName}");
        }

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private async Task DoUploadThemeAsync(string templateId, string themeName, Stream themeData)
        {
            using (var streamContent = new StreamContent(themeData))
            {
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                var uri = GetUploadThemeUri(templateId, themeName);
                var response = await Client.PutAsync(uri, streamContent).ConfigureAwait(false);
                var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var activityId = JsonConvert.DeserializeObject<BackgroundActivityStatus>(responseString).ActivityId;
                _ = await ConnectionClient.GetActivityResultAsync<int>(activityId, "Status").ConfigureAwait(false);
            }
        }

        #endregion
    }
}

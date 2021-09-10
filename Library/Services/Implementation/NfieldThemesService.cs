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

using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
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
        #region Implementation of INfieldFieldworkOfficesService
        public async Task UploadThemeAsync(Theme theme, string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException(fileName);

            var uri = GetUploadThemeUri(theme.TemplateId, theme.Name);

            using (var byteArrayContent = new ByteArrayContent(File.ReadAllBytes(filePath)))
            {
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                await Client.PostAsync(uri, byteArrayContent).FlattenExceptions().ConfigureAwait(false);
            }
        }

        public async Task RemoveAsync(string themeId)
        {
            var uri = GetThemesUri(themeId);
            await Client.DeleteAsync(uri).FlattenExceptions().ConfigureAwait(false);            
        }

        public async Task DownloadThemeAsync(string themeId, string filePath, bool overwrite)
        {
            var uri = GetThemesUri(themeId);

            var response =  await Client.GetAsync(uri).FlattenExceptions().ConfigureAwait(false);
            using (var outputFileStream = new FileStream(filePath, overwrite ? FileMode.Create : FileMode.CreateNew))
            {
                await response.Content.CopyToAsync(outputFileStream).ConfigureAwait(false);
            }
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

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
            return new Uri(ConnectionClient.NfieldServerUri, $"Templates/{templateId}/Themes/{themeName}");
        }

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }
    }       
}

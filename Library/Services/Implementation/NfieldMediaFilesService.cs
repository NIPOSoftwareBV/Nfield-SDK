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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nfield.Services.Implementation
{
    internal class NfieldMediaFilesService : INfieldMediaFilesService, INfieldConnectionClientObject
    {

        #region INfieldMediaFilesService Members

        public Task<IQueryable<string>> QueryAsync(string surveyId)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }
            return Client.GetAsync(MediaFilesApi(surveyId, null))
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<string>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        public Task<int> GetCountAsync(string surveyId)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }
            var uri = new Uri(MediaFilesApi(surveyId, null), "Count");
            return Client.GetAsync(uri)
                .ContinueWith(rmt => int.Parse(rmt.Result.Content.ReadAsStringAsync().Result))
                .FlattenExceptions();
        }


        public Task<byte[]> GetAsync(string surveyId, string fileName)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }

            return Client.GetAsync(MediaFilesApi(surveyId, fileName))
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsByteArrayAsync())
                .ContinueWith(b => b.Result.Result)
                .FlattenExceptions();
        }

        public Task RemoveAsync(string surveyId, string fileName)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            return
                Client.DeleteAsync(MediaFilesApi(surveyId, fileName))
                      .FlattenExceptions();
        }

        public async Task<string> UploadAndSaveAsync(string surveyId, string fileName, byte[] content)
        {
            CheckRequiredArgumentsUpload(surveyId, fileName, content);

            var postContent = new ByteArrayContent(content);
            postContent.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            var response = await Client.PostAsync(MediaFilesApi(surveyId, fileName), postContent).ConfigureAwait(false);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var backgroundActivityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(result);
            await ConnectionClient.GetActivityResultAsync<string>(backgroundActivityStatus.ActivityId, "Status").ConfigureAwait(false);

            return backgroundActivityStatus.ActivityId;
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        #region Helpers
        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri MediaFilesApi(string surveyId, string fileName)
        {
            var path = new StringBuilder();
            path.AppendFormat("Surveys/{0}/MediaFiles/", surveyId);
            if (!string.IsNullOrEmpty(fileName))
            {
                path.AppendFormat("{0}", HttpUtility.UrlEncode(fileName));
            }
            return new Uri(ConnectionClient.NfieldServerUri, path.ToString());
        }


        private static void CheckRequiredArgumentsUpload(string surveyId, string fileName, byte[] content)
        {
            CheckRequiredStringArgument(surveyId, nameof(surveyId));

            CheckRequiredStringArgument(fileName, nameof(fileName));

            CheckRequiredByteArrayArgument(content, nameof(content));
        }

        private static void CheckRequiredStringArgument(string argument, string name)
        {
            if (argument == null)
                throw new ArgumentNullException(name);
            if (argument.Trim().Length == 0)
                throw new ArgumentException($"{name} cannot be empty");
        }

        private static void CheckRequiredByteArrayArgument(byte[] byteArray, string name)
        {
            if (byteArray == null)
                throw new ArgumentNullException(name);
        }

        #endregion

    }
}

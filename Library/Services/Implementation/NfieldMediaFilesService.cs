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
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;

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
            return Client.GetAsync(MediaFilesApi(surveyId, null).AbsoluteUri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<string>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        public Task<byte[]> GetAsync(string surveyId, string fileName)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }

            return Client.GetAsync(MediaFilesApi(surveyId, fileName).AbsoluteUri)
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
                Client.DeleteAsync(MediaFilesApi(surveyId, fileName).AbsoluteUri)
                      .FlattenExceptions();
        }

        public Task AddOrUpdateAsync(string surveyId, string fileName, byte[] content)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            var postContent = new ByteArrayContent(content);
            postContent.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            return
                Client.PutAsync(MediaFilesApi(surveyId, fileName).AbsoluteUri, postContent)
                      .FlattenExceptions();
        }

        #endregion

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

        private Uri MediaFilesApi(string surveyId, string fileName)
        {
            var uriText = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            uriText.AppendFormat("Surveys/{0}/MediaFiles/", surveyId);
            if (!string.IsNullOrEmpty(fileName))
            {
                uriText.AppendFormat("?fileName={0}", HttpUtility.UrlEncode(fileName));
            }
            return new Uri(uriText.ToString());
        }
    }
}

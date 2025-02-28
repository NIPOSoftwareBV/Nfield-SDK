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
    internal class NfieldSurveyInvitationImagesService : INfieldSurveyInvitationImagesService,
        INfieldConnectionClientObject
    {
        public Task<AddInvitationImageResult> AddImageAsync(string surveyId, string filename, Stream content)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(filename, nameof(filename));
            Ensure.ArgumentNotNull(content, nameof(content));

            var uri = SurveyInvitationImagesTemplatesUrl(surveyId, filename);
            var imageContent = new StreamContent(content);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            imageContent.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = filename
                };

            return Client.PostAsync(uri, imageContent)
                .ContinueWith(task => JsonConvert.DeserializeObject<AddInvitationImageResult>(
                    task.Result.Content.ReadAsStringAsync().Result))
                .FlattenExceptions();
        }

        private INfieldHttpClient Client => ConnectionClient.Client;
        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        private Uri SurveyInvitationImagesTemplatesUrl(string surveyId, string filename)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/InvitationImages/{filename}");
        }
    }
}

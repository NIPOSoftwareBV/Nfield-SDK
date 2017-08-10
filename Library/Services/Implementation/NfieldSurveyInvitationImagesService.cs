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

using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Utilities;

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

        private string SurveyInvitationImagesTemplatesUrl(string surveyId, string filename)
        {
            var result = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            result.AppendFormat(CultureInfo.InvariantCulture, @"Surveys/{0}/InvitationImages/{1}", surveyId, filename);
            return result.ToString();
        }
    }
}

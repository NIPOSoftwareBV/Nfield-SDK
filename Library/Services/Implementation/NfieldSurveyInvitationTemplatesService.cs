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
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Utilities;

namespace Nfield.Services.Implementation
{
    internal class NfieldSurveyInvitationTemplatesService : INfieldSurveyInvitationTemplatesService, INfieldConnectionClientObject
    {
        public Task<IEnumerable<InvitationTemplateModel>> GetAsync(string surveyId)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));

            var uri = SurveyInvitationTemplatesUrl(surveyId);

            return Client.GetAsync(uri)
                .ContinueWith(task => JsonConvert.DeserializeObject<IEnumerable<InvitationTemplateModel>>(
                    task.Result.Content.ReadAsStringAsync().Result))
                .FlattenExceptions();
        }

        public Task<InvitationTemplateModelValidated> AddAsync(string surveyId, InvitationTemplateModel invitationTemplate)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNull(invitationTemplate, nameof(invitationTemplate));

            var uri = SurveyInvitationTemplatesUrl(surveyId);
            return Client.PostAsJsonAsync(uri, invitationTemplate)
                .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(task => JsonConvert.DeserializeObject<InvitationTemplateModelValidated>(task.Result))
                .FlattenExceptions();
        }

        public Task<InvitationTemplateModelValidated> UpdateAsync(string surveyId, InvitationTemplateModel invitationTemplate)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNull(invitationTemplate, nameof(invitationTemplate));

            var uri = SurveyInvitationTemplatesUrl(surveyId);

            return Client.PutAsJsonAsync(new Uri(uri, invitationTemplate.Id.ToString()), invitationTemplate)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(stringTask => JsonConvert.DeserializeObject<InvitationTemplateModelValidated>(stringTask.Result))
                .FlattenExceptions();
        }

        public Task<bool> RemoveAsync(string surveyId, int templateId)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));

            var uri = SurveyInvitationTemplatesUrl(surveyId);

            return Client.DeleteAsync(new Uri(uri, templateId.ToString()))
                .ContinueWith(response => response.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(stringTask => JsonConvert.DeserializeObject<DeleteInvitationResponse>(stringTask.Result).IsSuccess)
                .FlattenExceptions();
        }

        private INfieldHttpClient Client => ConnectionClient.Client;
        public INfieldConnectionClient ConnectionClient { get; internal set; }
        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        private Uri SurveyInvitationTemplatesUrl(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri,
                string.Format(CultureInfo.InvariantCulture,
                    "Surveys/{0}/InvitationTemplates",
                    surveyId));
        }

        // ReSharper disable once ClassNeverInstantiated.Local, used for deserializing api response
        private class DeleteInvitationResponse
        {
            public bool IsSuccess { get; set; }
        }
    }
}

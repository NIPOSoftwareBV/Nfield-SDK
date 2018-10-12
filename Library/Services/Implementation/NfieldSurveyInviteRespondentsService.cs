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
    internal class NfieldSurveyInviteRespondentsService : INfieldSurveyInviteRespondentsService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyInviteRespondentsService

        public Task<InviteRespondentsStatus> SendInvitationsAsync(string surveyId, InvitationBatch batch)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNull(batch, nameof(batch));

            var uri = SurveyInviteRespondentsUrl(surveyId);
            var batchWithFilter = new InvitationBatchWithFilter()
            {
                EmailColumnName = batch.EmailColumnName,
                InvitationTemplateId = batch.InvitationTemplateId,
                Name = batch.Name,
                ScheduledFor = batch.ScheduledFor,
                Filters = new[]
                {
                    new SampleFilter {Name = "RespondentKey", Op = "in", Value = string.Join(",", batch.RespondentKeys)}
                }
            };

            return Client.PostAsJsonAsync(uri, batchWithFilter)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(stringResult => JsonConvert.DeserializeObject<InviteRespondentsStatus>(stringResult.Result))
                .FlattenExceptions();
        }

        public Task<IEnumerable<InvitationMonitorSurveyStatus>> GetSurveysInvitationStatusAsync()
        {
            var uri = new Uri(SurveyInviteRespondentsUrl(string.Empty), "SurveysInvitationStatus/");

            return Client.GetAsync(uri)
                         .ContinueWith(task => JsonConvert.DeserializeObject<IEnumerable<InvitationMonitorSurveyStatus>>(
                              task.Result.Content.ReadAsStringAsync().Result))
                         .FlattenExceptions();
        }

        public Task<IEnumerable<InvitationMonitorBatchStatus>> GetSurveyBatchesStatusAsync(string surveyId)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));

            var uri = new Uri(SurveyInviteRespondentsUrl(surveyId), "SurveyBatchesStatus/");

            return Client.GetAsync(uri)
                         .ContinueWith(task => JsonConvert.DeserializeObject<IEnumerable<InvitationMonitorBatchStatus>>(
                              task.Result.Content.ReadAsStringAsync().Result))
                         .FlattenExceptions();
        }

        public Task<IEnumerable<InvitationBatchStatus>> GetInvitationStatusAsync(string surveyId, string batchName)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(batchName, nameof(batchName));

            var uri = new Uri(SurveyInviteRespondentsUrl(surveyId), string.Format(CultureInfo.InvariantCulture, "InvitationStatus/{0}", batchName));

            return Client.GetAsync(uri)
                         .ContinueWith(task => JsonConvert.DeserializeObject<IEnumerable<InvitationBatchStatus>>(
                              task.Result.Content.ReadAsStringAsync().Result))
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

        private INfieldHttpClient Client => ConnectionClient.Client;

        private Uri SurveyInviteRespondentsUrl(string surveyId)
        {
            if (surveyId == string.Empty)
            {
                return new Uri(ConnectionClient.NfieldServerUri, string.Format(CultureInfo.InvariantCulture,
                    "Surveys/InviteRespondents/", surveyId));
            }
            return new Uri(ConnectionClient.NfieldServerUri, string.Format(CultureInfo.InvariantCulture,
                "Surveys/{0}/InviteRespondents/", surveyId));
        }
    }
}

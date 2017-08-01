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

namespace Nfield.Services.Implementation
{
    internal class NfieldSurveyInviteRespondentsService : INfieldSurveyInviteRespondentsService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyInviteRespondentsService

        public Task<InviteRespondentsStatus> SendInvitationsAsync(string surveyId, InvitationBatch batch)
        {
            CheckRequiredStringArgument(surveyId, nameof(surveyId));
            CheckRequiredArgument(batch, nameof(batch));

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
            // TODO not string.empty
            var uri = $"{SurveyInviteRespondentsUrl(string.Empty)}/SurveysInvitationStatus/";

            return Client.GetAsync(uri)
                         .ContinueWith(task => JsonConvert.DeserializeObject<IEnumerable<InvitationMonitorSurveyStatus>>(
                              task.Result.Content.ReadAsStringAsync().Result))
                         .FlattenExceptions();
        }

        public Task<IEnumerable<InvitationMonitorBatchStatus>> GetSurveyBatchesStatusAsync(string surveyId)
        {
            CheckRequiredStringArgument(surveyId, nameof(surveyId));

            var uri = $"{SurveyInviteRespondentsUrl(surveyId)}/SurveyBatchesStatus/";

            return Client.GetAsync(uri)
                         .ContinueWith(task => JsonConvert.DeserializeObject<IEnumerable<InvitationMonitorBatchStatus>>(
                              task.Result.Content.ReadAsStringAsync().Result))
                         .FlattenExceptions();
        }

        public Task<IEnumerable<InvitationBatchStatus>> GetInvitationStatusAsync(string surveyId, string batchName)
        {
            CheckRequiredStringArgument(surveyId, nameof(surveyId));
            CheckRequiredStringArgument(batchName, nameof(batchName));

            var uri = $"{SurveyInviteRespondentsUrl(surveyId)}/InvitationStatus/{batchName}";

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

        private static void CheckRequiredStringArgument(string argument, string name)
        {
            if (argument == null)
                throw new ArgumentNullException(name);
            if (argument.Trim().Length == 0)
                throw new ArgumentException($"{name} cannot be empty");
        }

        private static void CheckRequiredArgument(object argument, string name)
        {
            if (argument == null)
                throw new ArgumentNullException(name);
        }

        private string SurveyInviteRespondentsUrl(string surveyId)
        {
            var result = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            result.AppendFormat(CultureInfo.InvariantCulture, @"Surveys/{0}/InviteRespondents", surveyId);

            return result.ToString();
        }

    }
}

﻿//    This file is part of Nfield.SDK.
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
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Utilities;

namespace Nfield.Services.Implementation
{
    internal class NfieldSurveyInterviewInteractionsSettingsService : INfieldSurveyInterviewInteractionsSettingsService, INfieldConnectionClientObject
    {
        public Task<SurveyInterviewInteractionsSettings> GetAsync(string surveyId)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));

            var uri = SurveyOtherSettingsUrl(surveyId);

            return Client.GetAsync(uri)
                         .ContinueWith(task => JsonConvert.DeserializeObject<SurveyInterviewInteractionsSettings>(
                            task.Result.Content.ReadAsStringAsync().Result))
                         .FlattenExceptions();
        }

        public Task<SurveyInterviewInteractionsSettings> UpdateAsync(string surveyId, SurveyInterviewInteractionsSettings settings)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNull(settings, nameof(settings));

            var uri = SurveyOtherSettingsUrl(surveyId);

            return Client.PatchAsJsonAsync(uri, settings)
                        .ContinueWith(task => JsonConvert.DeserializeObject<SurveyInterviewInteractionsSettings>(
                            task.Result.Content.ReadAsStringAsync().Result))
                        .FlattenExceptions();
        }

        private INfieldHttpClient Client => ConnectionClient.Client;
        public INfieldConnectionClient ConnectionClient { get; internal set; }
        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        private Uri SurveyOtherSettingsUrl(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/InterviewInteractionsSettings");
        }
    }
}

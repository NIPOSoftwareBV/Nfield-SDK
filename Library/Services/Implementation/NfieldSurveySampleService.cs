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
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;
using Nfield.Utilities;

namespace Nfield.Services.Implementation
{
    internal class NfieldSurveySampleService : INfieldSurveySampleService, INfieldConnectionClientObject
    {
        private const string RespondentKey = "RespondentKey";
        private const string InterviewId = "InterviewId";


        public Task<string> GetAsync(string surveyId)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));

            var uri = SurveySampleUrl(surveyId);

            return Client.GetAsync(uri)
                .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                .FlattenExceptions();
        }

        public Task<SampleUploadStatus> PostAsync(string surveyId, string sample)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(sample, nameof(sample));

            var uri = SurveySampleUrl(surveyId);
            var sampleContent = new StringContent(sample);
            return Client.PostAsync(uri, sampleContent)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(stringResult => JsonConvert.DeserializeObject<SampleUploadStatus>(stringResult.Result))
                .FlattenExceptions();
        }

        public Task<int> DeleteAsync(string surveyId, string respondentKey)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(respondentKey, nameof(respondentKey));

            var uri = SurveySampleUrl(surveyId);
            var filters = new List<SampleFilter>
            {
                new SampleFilter{Name = RespondentKey, Op = "eq", Value = respondentKey}
            };

            return Client.DeleteAsJsonAsync<IEnumerable<SampleFilter>>(uri, filters)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(stringResult => JsonConvert.DeserializeObject<BackgroundActivityStatus>(stringResult.Result).ActivityId)
                .ContinueWith(activityResult => ConnectionClient.GetActivityResultAsync<int>(activityResult.Result, "DeletedTotal"))
                .Unwrap()
                .FlattenExceptions();
        }

        public Task<int> BlockAsync(string surveyId, string respondentKey)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(respondentKey, nameof(respondentKey));

            var uri = new Uri(SurveySampleUrl(surveyId), "Block");

            var filters = new List<SampleFilter>
            {
                new SampleFilter{Name = RespondentKey, Op = "eq", Value = respondentKey}
            };

            return Client.PutAsJsonAsync<IEnumerable<SampleFilter>>(uri, filters)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(stringResult => JsonConvert.DeserializeObject<BackgroundActivityStatus>(stringResult.Result).ActivityId)
                .ContinueWith(activityResult => ConnectionClient.GetActivityResultAsync<int>(activityResult.Result, "BlockedTotal"))
                .Unwrap()
                .FlattenExceptions();
        }

        public Task<bool> UpdateAsync(string surveyId, int sampleRecordId, IEnumerable<SampleColumnUpdate> columnsToUpdate)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentEnumerableNotNullOrEmpty(columnsToUpdate, nameof(columnsToUpdate));
            var m = new SurveyUpdateSampleRecordModel
            {
                SampleRecordId = sampleRecordId,
                ColumnUpdates = columnsToUpdate
            };

            var uri = new Uri(SurveySampleUrl(surveyId), "Update");

            return Client.PutAsJsonAsync(uri, m)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(stringResult => JsonConvert.DeserializeObject<SampleUpdateStatus>(stringResult.Result).ResultStatus)
                .FlattenExceptions();
        }

        public Task<int> ResetAsync(string surveyId, string respondentKey)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(respondentKey, nameof(respondentKey));

            var uri = new Uri(SurveySampleUrl(surveyId), "Reset");

            var filters = new List<SampleFilter>
            {
                new SampleFilter{Name = RespondentKey, Op = "eq", Value = respondentKey}
            };

            return Client.PutAsJsonAsync<IEnumerable<SampleFilter>>(uri, filters)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(stringResult => JsonConvert.DeserializeObject<BackgroundActivityStatus>(stringResult.Result).ActivityId)
                .ContinueWith(activityResult => ConnectionClient.GetActivityResultAsync<int>(activityResult.Result, "ResetTotal"))
                .Unwrap()
                .FlattenExceptions();
        }

        public Task<int> ClearByRespondentAsync(string surveyId, string respondentKey, IEnumerable<string> columnsToClear)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(respondentKey, nameof(respondentKey));
            Ensure.ArgumentEnumerableNotNullOrEmpty(columnsToClear, nameof(columnsToClear));

            var filters = new List<SampleFilter>
            {
                new SampleFilter{Name = RespondentKey, Op = "eq", Value = respondentKey}
            };

            return ClearAsync(surveyId, filters, columnsToClear);
        }

        public Task<int> ClearByInterviewAsync(string surveyId, int interviewId, IEnumerable<string> columnsToClear)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentEnumerableNotNullOrEmpty(columnsToClear, nameof(columnsToClear));

            var filters = new List<SampleFilter>
            {
                new SampleFilter{Name = InterviewId, Op = "eq", Value = interviewId.ToString()}
            };

            return ClearAsync(surveyId, filters, columnsToClear);
        }

        private Task<int> ClearAsync(string surveyId, List<SampleFilter> filters, IEnumerable<string> columnsToClear)
        {
            var uri = new Uri(SurveySampleUrl(surveyId) + "Clear");

            var request = new ClearSurveySampleModel
            {
                Filters = filters,
                Columns = columnsToClear
            };

            return Client.PutAsJsonAsync(uri, request)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(stringResult => JsonConvert.DeserializeObject<BackgroundActivityStatus>(stringResult.Result).ActivityId)
                .ContinueWith(activityResult => ConnectionClient.GetActivityResultAsync<int>(activityResult.Result, "ClearTotal"))
                .Unwrap()
                .FlattenExceptions();
        }

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private INfieldHttpClient Client => ConnectionClient.Client;

        private Uri SurveySampleUrl(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/Sample/");
        }
    }
}

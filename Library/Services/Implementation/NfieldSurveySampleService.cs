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
using System.Text;
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


        public async Task<string> GetAsync(string surveyId)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));

            var uri = SurveySampleUrl(surveyId);

            var response = await Client.GetAsync(uri);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<SampleUploadStatus> PostAsync(string surveyId, string sample)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(sample, nameof(sample));

            var uri = SurveySampleUrl(surveyId);
            var sampleContent = new StringContent(sample);

            var responseMessage = await Client.PostAsync(uri, sampleContent);
            var stringResult = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SampleUploadStatus>(stringResult);
        }
        
        public async Task<SampleUploadStatus> PostJsonAsync<TContent>(string surveyId, TContent sample)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNull(sample, nameof(sample));

            var uri = SurveySampleUrl(surveyId);
            var responseMessage = await Client.PostAsJsonAsync(uri, sample);
            var stringResult = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SampleUploadStatus>(stringResult);
        }

        public async Task<int> DeleteAsync(string surveyId, string respondentKey)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(respondentKey, nameof(respondentKey));

            var uri = SurveySampleUrl(surveyId);
            var filters = new List<SampleFilter>
            {
                new SampleFilter{Name = RespondentKey, Op = "eq", Value = respondentKey}
            };

            var responseMessage = await Client.DeleteAsJsonAsync<IEnumerable<SampleFilter>>(uri, filters);

            var stringResult = await responseMessage.Content.ReadAsStringAsync();
            var activityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(stringResult);

            return await ConnectionClient.GetActivityResultAsync<int>(activityStatus.ActivityId, "DeletedTotal");
        }

        public async Task<int> BlockAsync(string surveyId, string respondentKey)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(respondentKey, nameof(respondentKey));

            var uri = new Uri(SurveySampleUrl(surveyId), "Block");

            var filters = new List<SampleFilter>
            {
                new SampleFilter{Name = RespondentKey, Op = "eq", Value = respondentKey}
            };

            var responseMessage = await Client.PutAsJsonAsync<IEnumerable<SampleFilter>>(uri, filters);

            var stringResult = await responseMessage.Content.ReadAsStringAsync();
            var activityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(stringResult);

            return await ConnectionClient.GetActivityResultAsync<int>(activityStatus.ActivityId, "BlockedTotal");
        }

        public async Task<bool> UpdateAsync(string surveyId, int sampleRecordId, IEnumerable<SampleColumnUpdate> columnsToUpdate)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentEnumerableNotNullOrEmpty(columnsToUpdate, nameof(columnsToUpdate));
            var m = new SurveyUpdateSampleRecordModel
            {
                SampleRecordId = sampleRecordId,
                ColumnUpdates = columnsToUpdate                
            };

            var uri = new Uri(SurveySampleUrl(surveyId), "Update");

            var responseMessage = await Client.PutAsJsonAsync(uri, m);
            var stringResult = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SampleUpdateStatus>(stringResult).ResultStatus;
        }

        public async Task<int> ResetAsync(string surveyId, string respondentKey)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(respondentKey, nameof(respondentKey));

            var uri = new Uri(SurveySampleUrl(surveyId), "Reset");

            var filters = new List<SampleFilter>
            {
                new SampleFilter{Name = RespondentKey, Op = "eq", Value = respondentKey}
            };

            var responseMessage = await Client.PutAsJsonAsync<IEnumerable<SampleFilter>>(uri, filters);
            var stringResult = await responseMessage.Content.ReadAsStringAsync();
            var activityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(stringResult);

            return await ConnectionClient.GetActivityResultAsync<int>(activityStatus.ActivityId, "ResetTotal");
        }

        public async Task<int> ClearByRespondentAsync(string surveyId, string respondentKey, IEnumerable<string> columnsToClear)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(respondentKey, nameof(respondentKey));
            Ensure.ArgumentEnumerableNotNullOrEmpty(columnsToClear, nameof(columnsToClear));

            var filters = new List<SampleFilter>
            {
                new SampleFilter{Name = RespondentKey, Op = "eq", Value = respondentKey}
            };

            return await ClearAsync(surveyId, filters, columnsToClear);
        }

        public async Task<int> ClearByInterviewAsync(string surveyId, int interviewId, IEnumerable<string> columnsToClear)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentEnumerableNotNullOrEmpty(columnsToClear, nameof(columnsToClear));

            var filters = new List<SampleFilter>
            {
                new SampleFilter{Name = InterviewId, Op = "eq", Value = interviewId.ToString()}
            };

            return await ClearAsync(surveyId, filters, columnsToClear);
        }

        private async Task<int> ClearAsync(string surveyId, List<SampleFilter> filters, IEnumerable<string> columnsToClear)
        {
            var uri = new Uri(SurveySampleUrl(surveyId) + "Clear");

            var request = new ClearSurveySampleModel
            {
                Filters = filters,
                Columns = columnsToClear
            };

            var responseMessage = await Client.PutAsJsonAsync(uri, request);
            var stringResult = await responseMessage.Content.ReadAsStringAsync();
            var activityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(stringResult);

            return await ConnectionClient.GetActivityResultAsync<int>(activityStatus.ActivityId, "ClearTotal");
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

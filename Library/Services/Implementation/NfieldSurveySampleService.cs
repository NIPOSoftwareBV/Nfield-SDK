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
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;
using Nfield.SDK.Models;
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
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        public async Task<string> GetSingleSampleRecordAsync(string surveyId, int interviewId)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/Sample/{interviewId}");

            var response = await Client.GetAsync(uri);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<SampleUploadStatus> PostAsync(string surveyId, string sample)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(sample, nameof(sample));

            var uri = SurveySampleUrl(surveyId);
            var sampleContent = new StringContent(sample);

            var response = await Client.PostAsync(uri, sampleContent);
            var responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SampleUploadStatus>(responseAsString);
            return result;

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

            var response = await Client.DeleteAsJsonAsync<IEnumerable<SampleFilter>>(uri, filters);
            var responseAsString = await response.Content.ReadAsStringAsync();
            var activityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(responseAsString);
            return await ConnectionClient.GetActivityResultAsync<int>(activityStatus.ActivityId, "DeletedTotal");
        }

        public async Task<int> DeleteByFilterAsync(string surveyId, List<SampleFilter> filters)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNull(filters, nameof(filters));

            var uri = SurveySampleUrl(surveyId);

            var response = await Client.DeleteAsJsonAsync<IEnumerable<SampleFilter>>(uri, filters);
            var responseAsString = await response.Content.ReadAsStringAsync();
            var activityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(responseAsString);
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

            var response = await Client.PutAsJsonAsync<IEnumerable<SampleFilter>>(uri, filters);
            var responseAsString = await response.Content.ReadAsStringAsync();
            var activityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(responseAsString);
            return await ConnectionClient.GetActivityResultAsync<int>(activityStatus.ActivityId, "BlockedTotal");

        }

        public async Task<int> BlockByFilterAsync(string surveyId, List<SampleFilter> filters)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentEnumerableNotNullOrEmpty(filters, nameof(filters));

            var uri = new Uri(SurveySampleUrl(surveyId), "Block");

            var response = await Client.PutAsJsonAsync<IEnumerable<SampleFilter>>(uri, filters);
            var responseAsString = await response.Content.ReadAsStringAsync();
            var activityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(responseAsString);
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

            var response = await Client.PutAsJsonAsync(uri, m);
            var responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SampleUpdateStatus>(responseAsString);
            return result.ResultStatus;

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

            var response = await Client.PutAsJsonAsync<IEnumerable<SampleFilter>>(uri, filters);
            var responseAsString = await response.Content.ReadAsStringAsync();
            var activityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(responseAsString);
            return await ConnectionClient.GetActivityResultAsync<int>(activityStatus.ActivityId, "ResetTotal");
        }

        public async Task<int> ResetByFilterAsync(string surveyId, IEnumerable<SampleFilter> filters)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentEnumerableNotNullOrEmpty(filters, nameof(filters));

            var uri = new Uri(SurveySampleUrl(surveyId), "Reset");

            var response = await Client.PutAsJsonAsync<IEnumerable<SampleFilter>>(uri, filters);
            var responseAsString = await response.Content.ReadAsStringAsync();
            var activityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(responseAsString);
            return await ConnectionClient.GetActivityResultAsync<int>(activityStatus.ActivityId, "ResetTotal");

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

        public Task<int> ClearByFilterAsync(string surveyId, List<SampleFilter> filters, IEnumerable<string> columnsToClear)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentEnumerableNotNullOrEmpty(filters, nameof(filters));
            Ensure.ArgumentEnumerableNotNullOrEmpty(columnsToClear, nameof(columnsToClear));

            return ClearAsync(surveyId, filters, columnsToClear);
        }

        private async Task<int> ClearAsync(string surveyId, List<SampleFilter> filters, IEnumerable<string> columnsToClear)
        {
            var uri = new Uri(SurveySampleUrl(surveyId) + "Clear");

            var request = new ClearSurveySampleModel
            {
                Filters = filters,
                Columns = columnsToClear
            };

            var response = await Client.PutAsJsonAsync(uri, request);
            var responseAsString = await response.Content.ReadAsStringAsync();
            var activityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(responseAsString);
            return await ConnectionClient.GetActivityResultAsync<int>(activityStatus.ActivityId, "ClearTotal");
        }

        public async Task<IEnumerable<SampleColumnCreate>> CreateAsync(string surveyId, IEnumerable<SampleColumnCreate> sampleColumns)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNull(sampleColumns, nameof(sampleColumns));

            var uri = new Uri(SurveySampleUrl(surveyId), "Create");

            var response = await Client.PostAsJsonAsync(uri, sampleColumns);
            var responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<SampleColumnCreate>>(responseAsString);
            return result;

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

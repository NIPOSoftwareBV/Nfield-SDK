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

using System.Collections.Generic;
using System.Globalization;
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
    internal class NfieldSurveySampleService : INfieldSurveySampleService, INfieldConnectionClientObject
    {

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
                new SampleFilter{Name = "RespondentKey", Op = "eq", Value = respondentKey}
            };

            return Client.DeleteAsJsonAsync<IEnumerable<SampleFilter>>(uri, filters)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(stringResult => JsonConvert.DeserializeObject<SampleDeleteStatus>(stringResult.Result).DeletedCount)
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

        private string SurveySampleUrl(string surveyId)
        {
            var result = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            result.AppendFormat(CultureInfo.InvariantCulture, @"Surveys/{0}/Sample", surveyId);

            return result.ToString();
        }
    }
}

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
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveysService"/>
    /// </summary>
    internal class NfieldSurveyGroupService : INfieldSurveyGroupService, INfieldConnectionClientObject
    {
        public INfieldConnectionClient ConnectionClient { get; internal set; }
        public void InitializeNfieldConnection(INfieldConnectionClient connection) => ConnectionClient = connection;

        public async Task<IEnumerable<SurveyGroup>> GetAllAsync()
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, "SurveyGroups");

            using (var response = await ConnectionClient.Client.GetAsync(uri))
            {
                return await DeserializeJsonAsync<List<SurveyGroup>>(response);
            }
        }

        public async Task<SurveyGroup> GetAsync(int surveyGroupId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}");

            using (var response = await ConnectionClient.Client.GetAsync(uri))
            {
                return await DeserializeJsonAsync<SurveyGroup>(response);
            }
        }

        public async Task<IEnumerable<Survey>> GetSurveysAsync(int surveyGroupId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}/Surveys");

            using (var response = await ConnectionClient.Client.GetAsync(uri))
            {
                return await DeserializeJsonAsync<List<Survey>>(response);
            }
        }

        public async Task MoveSurveyAsync(string surveyId, int newSurveyGroupId)
        {
            if (surveyId == null)
                throw new ArgumentNullException(nameof(surveyId));

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/SurveyGroup");
            var content = new Dictionary<string, int>()
            {
                ["SurveyGroupId"] = newSurveyGroupId
            };

            // note: we need to dispose the response even when we don't use it
            using (var response = await ConnectionClient.Client.PutAsJsonAsync(uri, content))
            {
            }
        }

        public async Task<SurveyGroup> CreateAsync(SurveyGroupValues model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var uri = new Uri(ConnectionClient.NfieldServerUri, "SurveyGroups");

            using (var response = await ConnectionClient.Client.PostAsJsonAsync(uri, model))
            {
                var result = await DeserializeJsonAsync<SurveyGroup>(response);

                return result;
            }
        }

        public async Task<SurveyGroup> UpdateAsync(int surveyGroupId, SurveyGroupValues model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}");

            using (var response = await ConnectionClient.Client.PatchAsJsonAsync(uri, model))
            {
                var result = await DeserializeJsonAsync<SurveyGroup>(response);

                return result;
            }
        }

        public async Task DeleteAsync(int surveyGroupId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}");

            // note: we need to dispose the response even when we don't use it
            using (await ConnectionClient.Client.DeleteAsync(uri))
            {
            }
        }

        private async Task<T> DeserializeJsonAsync<T>(HttpResponseMessage response)
        {
            using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
            {
                using (var jsonReader = new JsonTextReader(reader))
                {
                    var serializer = new JsonSerializer();
                    return serializer.Deserialize<T>(jsonReader);
                }
            }
        }
    }
}
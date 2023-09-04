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
    internal class NfieldSurveyGroupAssignmentsService : INfieldSurveyGroupAssignmentsService, INfieldConnectionClientObject
    {
        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection) => ConnectionClient = connection;

        public async Task<IEnumerable<SurveyGroupDirectoryAssignment>> GetDirectoryAssignmentsAsync(int surveyGroupId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}/DirectoryAssignments");

            using (var response = await ConnectionClient.Client.GetAsync(uri).ConfigureAwait(false))
            {
                return await DeserializeJsonAsync<List<SurveyGroupDirectoryAssignment>>(response).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<SurveyGroupNativeAssignment>> GetLocalAssignmentsAsync(int surveyGroupId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}/LocalAssignments");

            using (var response = await ConnectionClient.Client.GetAsync(uri).ConfigureAwait(false))
            {
                return await DeserializeJsonAsync<List<SurveyGroupNativeAssignment>>(response).ConfigureAwait(false);
            }
        }

        public async Task<SurveyGroupDirectoryAssignment> AssignDirectoryAsync(int surveyGroupId, DirectoryIdentityModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}/AssignDirectory");

            using (var response = await ConnectionClient.Client.PutAsJsonAsync(uri, model).ConfigureAwait(false))
            {
                var result = await DeserializeJsonAsync<SurveyGroupDirectoryAssignment>(response).ConfigureAwait(false);

                return result;
            }
        }

        public async Task<SurveyGroupNativeAssignment> AssignLocalAsync(int surveyGroupId, string identityId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}/AssignLocal");

            var dictionary = new Dictionary<string, string>
            {
                { "NativeIdentityId", identityId }
            };

            using (var response = await ConnectionClient.Client.PutAsJsonAsync(uri, dictionary).ConfigureAwait(false))
            {
                var result = await DeserializeJsonAsync<SurveyGroupNativeAssignment>(response).ConfigureAwait(false);

                return result;
            }
        }

        public async Task<HttpResponseMessage> UnassignDirectoryAsync(int surveyGroupId, DirectoryIdentityModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}/UnassignDirectory");

            return await ConnectionClient.Client.PutAsJsonAsync(uri, model).ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> UnassignLocalAsync(int surveyGroupId, string identityId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}/UnassignLocal");

            var dictionary = new Dictionary<string, string>
            {
                { "NativeIdentityId", identityId }
            };

            return await ConnectionClient.Client.PutAsJsonAsync(uri, dictionary).ConfigureAwait(false);
        }

        private async Task<T> DeserializeJsonAsync<T>(HttpResponseMessage response)
        {
            using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync().ConfigureAwait(false)))
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

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
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}/Directory-Assignments");

            using (var response = await ConnectionClient.Client.GetAsync(uri))
            {
                return await DeserializeJsonAsync<List<SurveyGroupDirectoryAssignment>>(response);
            }
        }

        public async Task<IEnumerable<SurveyGroupNativeAssignment>> GetLocalAssignmentsAsync(int surveyGroupId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}/Local-Assignments");

            using (var response = await ConnectionClient.Client.GetAsync(uri))
            {
                return await DeserializeJsonAsync<List<SurveyGroupNativeAssignment>>(response);
            }
        }

        public async Task<SurveyGroupDirectoryAssignment> AssignDirectoryAsync(int surveyGroupId, DirectoryIdentityModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}/Assign-Directory");

            using (var response = await ConnectionClient.Client.PutAsJsonAsync(uri, model))
            {
                var result = await DeserializeJsonAsync<SurveyGroupDirectoryAssignment>(response);

                return result;
            }
        }

        public async Task<SurveyGroupNativeAssignment> AssignLocalAsync(int surveyGroupId, string identityId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}/Assign-Local");

            var dictionary = new Dictionary<string, string>
            {
                { "NativeIdentityId", identityId }
            };

            using (var response = await ConnectionClient.Client.PutAsJsonAsync(uri, dictionary))
            {
                var result = await DeserializeJsonAsync<SurveyGroupNativeAssignment>(response);

                return result;
            }
        }

        public async Task UnassignDirectoryAsync(int surveyGroupId, DirectoryIdentityModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}/Unassign-Directory");

            await ConnectionClient.Client.PutAsJsonAsync(uri, model);
        }

        public async Task UnassignLocalAsync(int surveyGroupId, string identityId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"SurveyGroups/{surveyGroupId}/Unassign-Local");

            await ConnectionClient.Client.PutAsJsonAsync(uri, identityId);
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

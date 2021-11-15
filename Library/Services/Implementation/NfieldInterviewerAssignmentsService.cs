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

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    internal class NfieldInterviewerAssignmentsService : INfieldInterviewerAssignmentsService, INfieldConnectionClientObject
    {

        #region Implementation of INfieldInterviewerAssignments

        /// <summary>
        /// Implements <see cref="INfieldInterviewerAssignmentsService.GetAsync(string)"/> 
        /// </summary>       
        public Task<IQueryable<InterviewerAssignmentModel>> QueryAsync(string interviewerId)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new InterviewerAssignmentContractResolver();

            return ConnectionClient.Client.GetAsync(GetInterviewerAssignmentsApiUrl(interviewerId))
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<InterviewerAssignmentModel>>(stringTask.Result, settings).AsQueryable())
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

        #region Private methods

        private Uri GetInterviewerAssignmentsApiUrl(string interviewerId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Interviewers/{interviewerId}/Assignments");
        }

        private class InterviewerAssignmentContractResolver : DefaultContractResolver
        {
            private Dictionary<string, string> PropertyMappings { get; set; }

            public InterviewerAssignmentContractResolver()
            {
                PropertyMappings = new Dictionary<string, string>
                {
                    {"IsAssigned", "Assigned"},
                    {"IsActive", "Active"},
                    {"SuccessfulCount", "Successful"},
                    {"ScreenedOutCount", "ScreenedOut"},
                    {"DroppedOutCount", "DroppedOut"},
                    {"RejectedCount", "Rejected"}
                };
            }

            protected override string ResolvePropertyName(string propertyName)
            {                
                return PropertyMappings.TryGetValue(propertyName, out var resolvedName) ? resolvedName : base.ResolvePropertyName(propertyName);
            }
        }

        #endregion
    }


}

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
        public Task<IQueryable<InterviewerAssignmentDataModel>> QueryAsync(string interviewerId)
        {
            return ConnectionClient.Client.GetAsync(GetInterviewerAssignmentsApiUrl(interviewerId))
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<InterviewerAssignmentDataModel>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }
        /// <summary>
        /// Implements <see cref="INfieldInterviewerAssignmentsService.UpdateAsync(string, InterviewerAssignmentModel)"/> 
        /// </summary>  
        public Task PutAsync(string interviewerId, InterviewerAssignmentModel model)
        {
            Ensure.ArgumentNotNull(model, nameof(model));

            return
                ConnectionClient.Client.PutAsJsonAsync(GetInterviewerAssignmentsApiUrl(interviewerId), model)
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
            return new Uri(ConnectionClient.NfieldServerUri, $"interviewers/{interviewerId}/assignments"); 
        }

        #endregion
    }
}

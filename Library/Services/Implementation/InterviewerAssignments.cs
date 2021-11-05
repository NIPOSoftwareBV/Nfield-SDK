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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.SDK.Services.Implementation
{
    internal class NfieldInterviewerAssignments : INfieldInterviewerAssignments, INfieldConnectionClientObject
    {

        #region Implementation of INfieldInterviewerAssignments

        /// <summary>
        /// Implements <see cref="INfieldInterviewerAssignments.GetAsync(string)"/> 
        /// </summary>       
        public Task<IQueryable<InterviewerAssignmentDataModel>> GetAsync(string interviewerId)
        {
            return ConnectionClient.Client.GetAsync(InterviewerAssignmentsApi(interviewerId))
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<InterviewerAssignmentDataModel>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }
        /// <summary>
        /// Implements <see cref="INfieldInterviewerAssignments.UpdateAsync(string, InterviewerAssignmentModel)"/> 
        /// </summary>  
        public Task UpdateAsync(string interviewerId, InterviewerAssignmentModel model)
        {
            throw new NotImplementedException();
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


        private Uri InterviewerAssignmentsApi(string interviewerId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"InterviewerAssignments/{interviewerId}"); 
        }

        #endregion
    }
}

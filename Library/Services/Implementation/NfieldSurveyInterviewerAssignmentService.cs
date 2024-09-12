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

using Newtonsoft.Json.Linq;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Utilities;
using System;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveyInterviewerAssignmentsService"/>
    /// </summary>
    internal class NfieldSurveyInterviewerAssignmentService : INfieldSurveyInterviewerAssignmentService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyInterviewerAssignmentService

        /// <summary>
        /// Implements <see cref="INfieldSurveyInterviewerAssignmentService.AssignAsync(string, string)"/> 
        /// </summary>     
        public Task AssignAsync(string surveyId, string interviewerId, SurveyInterviewerAssignmentModel surveyInterviewerAssignmentModel)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(interviewerId, nameof(interviewerId));
            Ensure.ArgumentNotNull(surveyInterviewerAssignmentModel, nameof(surveyInterviewerAssignmentModel));

            var uri = SurveyInterviewerAssignUrl(surveyId, interviewerId);
          
            return Client.PutAsJsonAsync(uri, surveyInterviewerAssignmentModel).FlattenExceptions();
        }

        /// <summary>
        /// Implements <see cref="INfieldSurveyInterviewerAssignmentService.UnassignAsync(string, string)"/> 
        /// </summary> 
        public Task UnassignAsync(string surveyId, string interviewerId)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(interviewerId, nameof(interviewerId));

            var uri = SurveyInterviewerUnassignUrl(surveyId, interviewerId);           

            return Client.PutAsJsonAsync(uri, new JObject()).FlattenExceptions();
        }

        #endregion        

        /// <summary>
         /// Constructs and returns the url for survey interviewer assignments
         /// based on supplied parameters
         /// </summary>
        private Uri SurveyInterviewerAssignUrl(string surveyId, string interviewerId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/Interviewers/{interviewerId}/Assign");
        }

        /// <summary>
        /// Constructs and returns the url for survey interviewer assignments
        /// based on supplied parameters
        /// </summary>
        private Uri SurveyInterviewerUnassignUrl(string surveyId, string interviewerId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/Interviewers/{interviewerId}/Unassign");
        }

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion
    }
}
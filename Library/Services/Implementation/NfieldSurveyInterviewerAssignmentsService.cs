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
using System.Threading.Tasks;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Utilities;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveyInterviewerAssignmentsService"/>
    /// </summary>
    internal class NfieldSurveyInterviewerAssignmentsService : INfieldSurveyInterviewerAssignmentsService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyInterviewerAssignmentsService

        /// <summary>
        /// Implements <see cref="INfieldSurveyInterviewerAssignmentsService.AssignAsync(string, string)"/> 
        /// </summary>     
        public Task AssignAsync(string surveyId, string interviewerId)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }
            if (string.IsNullOrEmpty(interviewerId))
            {
                throw new ArgumentNullException("interviewerId");
            }

            var uri = SurveyInterviewerAssignmentsUrl(surveyId);
            var model = new SurveyInterviewerAssignmentChangeModel { InterviewerId = interviewerId, Assign = true };

            return Client.PutAsJsonAsync(uri, model).FlattenExceptions();
        }

        /// <summary>
        /// Implements <see cref="INfieldSurveyInterviewerAssignmentsService.UnassignAsync(string, string)"/> 
        /// </summary> 
        public Task UnassignAsync(string surveyId, string interviewerId)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }
            if (string.IsNullOrEmpty(interviewerId))
            {
                throw new ArgumentNullException("interviewerId");
            }

            var uri = SurveyInterviewerAssignmentsUrl(surveyId);
            var model = new SurveyInterviewerAssignmentChangeModel { InterviewerId = interviewerId, Assign = false };

            return Client.PutAsJsonAsync(uri, model).FlattenExceptions();
        }

        /// <summary>
        /// Implements <see cref="INfieldSurveyInterviewerAssignmentsService.PutAsync(string, string, SurveyInterviewerAssignmentModel)"/> 
        /// </summary>  
        public Task PutAsync(string surveyId, string interviewerId, SurveyInterviewerAssignmentModel model)
        {
            Ensure.ArgumentNotNull(model, nameof(model));

            return
                ConnectionClient.Client.PutAsJsonAsync(SurveyInterviewerAssignmentsUrl(surveyId, interviewerId), model)
                .FlattenExceptions();
        }

        #endregion

        /// <summary>
        /// Constructs and returns the url for survey interviewer assignments
        /// based on supplied <paramref name="surveyId"/>
        /// </summary>
        private Uri SurveyInterviewerAssignmentsUrl(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/Assignment/");
        }

        /// <summary>
         /// Constructs and returns the url for survey interviewer assignments
         /// based on supplied parameters
         /// </summary>
        private Uri SurveyInterviewerAssignmentsUrl(string surveyId, string interviewerId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/interviewers/{interviewerId}/Assignment");
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
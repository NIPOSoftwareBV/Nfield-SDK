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
using System.Globalization;
using System.Threading.Tasks;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveyInterviewerAssignmentsService"/>
    /// </summary>
    internal class NfieldSurveyInterviewerAssignmentsService : INfieldSurveyInterviewerAssignmentsService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyInterviewerAssignmentsService

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
            var model = new SurveyInterviewerAssignmentChangeModel {InterviewerId = interviewerId, Assign = true};

            return Client.PutAsJsonAsync(uri, model).FlattenExceptions();
        }

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

        #endregion
        
        /// <summary>
        /// Constructs and returns the url for survey interviewer assignments
        /// based on supplied <paramref name="surveyId"/>
        /// </summary>
        private string SurveyInterviewerAssignmentsUrl(string surveyId)
        {
            return string.Format(CultureInfo.InvariantCulture,
                "{0}/Surveys/{1}/Assignment/",
                ConnectionClient.NfieldServerUri.AbsoluteUri, surveyId);
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
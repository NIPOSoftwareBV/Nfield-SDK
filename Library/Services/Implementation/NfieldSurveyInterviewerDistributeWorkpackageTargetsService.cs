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
using Nfield.SDK.Models;
using Nfield.Services;
using Nfield.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.SDK.Services.Implementation
{
    class NfieldSurveyInterviewerDistributeWorkpackageTargetsService : INfieldSurveyInterviewerDistributeWorkpackageTargetsService, INfieldConnectionClientObject
    {
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

        public Task PostAsync(string surveyId, string interviewerId, SurveyInterviewerDistributeModel surveyInterviewerDistributeModel)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(interviewerId, nameof(interviewerId));
            Ensure.ArgumentNotNull(surveyInterviewerDistributeModel, nameof(surveyInterviewerDistributeModel));

            return Client.PostAsJsonAsync(SurveyInterviewerAssignmentQuotaLevelTargetsUrl(surveyId, interviewerId), surveyInterviewerDistributeModel)
                .FlattenExceptions();
        }

        private Uri SurveyInterviewerAssignmentQuotaLevelTargetsUrl(string surveyId, string interviewerId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/Inyterviewers/{interviewerId}/DistributeWorkpackageTarget");
        }


        #endregion
    }
}

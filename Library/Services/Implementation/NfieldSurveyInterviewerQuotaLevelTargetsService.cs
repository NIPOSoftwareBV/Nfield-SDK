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
using System.Threading.Tasks;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.SDK.Models;
using Nfield.Utilities;
using Nfield.Services;

namespace Nfield.SDK.Services.Implementation
{
    class NfieldSurveyInterviewerQuotaLevelTargetsService : INfieldSurveyInterviewerQuotaLevelTargetsService, INfieldConnectionClientObject
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

        /// <summary>
        /// Implements <see cref="INfieldSurveyInterviewerQuotaLevelTargetsService.UpdateAsync(string , string, IEnumerable<WorkPackageTarget>)"/>
        /// </summary>
        public Task UpdateAsync(string surveyId, string interviewerId, IEnumerable<WorkPackageTarget> workPackageTargets)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(interviewerId, nameof(interviewerId));
            Ensure.ArgumentEnumerableNotNullOrEmpty(workPackageTargets, nameof(workPackageTargets));
         
            return Client.PutAsJsonAsync(SurveyInterviewerAssignmentQuotaLevelTargetsUrl(surveyId, interviewerId), workPackageTargets)
                .FlattenExceptions();
        }


        private Uri SurveyInterviewerAssignmentQuotaLevelTargetsUrl(string surveyId, string interviewerId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/Interviewers/{interviewerId}/QuotaLevelTargets/");
        }

        #endregion
    }
}

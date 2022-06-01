﻿//    This file is part of Nfield.SDK.
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

using Nfield.Infrastructure;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;
using Nfield.Utilities;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldInterviewQualityService"/>
    /// </summary>
    internal class NfieldInterviewsService : INfieldInterviewsService, INfieldConnectionClientObject
    {
        public Task<int> DeleteAsync(string surveyId, int interviewId)
        {
            CheckSurveyId(surveyId);
            return Client.DeleteAsync(InterviewsApiUri(surveyId, interviewId))
                .ContinueWith(
                    responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(stringResult =>
                     JsonConvert.DeserializeObject<BackgroundActivityStatus>(stringResult.Result).ActivityId)
                .ContinueWith(activityResult => ConnectionClient.GetActivityResultAsync<int>(activityResult.Result, "Status"))
                .Unwrap()
                .FlattenExceptions();
        }

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private static void CheckSurveyId(string surveyId)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            // the unnamed var _ fails to compile in appveyor
            Guid g;
            if (!Guid.TryParse(surveyId, out g))
                throw new ArgumentException($"SurveyId {surveyId} is not a valid identifier");
        }

        private INfieldHttpClient Client => ConnectionClient.Client;

        private Uri InterviewsApiUri(string surveyId, int interviewId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/Interviews/{interviewId}");
        }
    }
}

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

using Nfield.Infrastructure;
using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Models;
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;
using Nfield.Utilities;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldInterviewQualityService"/>
    /// </summary>
    internal class NfieldDeleteInterviewService : INfieldDeleteInterviewService, INfieldConnectionClientObject
    {
        public Task<int> DeleteAsync(string surveyId, string interviewId)
        {
            CheckSurveyId(surveyId);
            CheckInterviewId(interviewId);
            return Client.PutAsJsonAsync(DeleteIntervieApiUri(surveyId, interviewId),
                    new Interview() {SurveyId = surveyId, InterviewId = interviewId})
                .ContinueWith(
                    responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(stringResult => 
                     JsonConvert.DeserializeObject<BackgroundActivityStatus>(stringResult.Result).ActivityId)
                .ContinueWith(activityResult => ConnectionClient.GetActivityResultAsync(activityResult.Result, "DeletedTotal"))
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
            if (!Guid.TryParse(surveyId,out var _))
                throw new ArgumentException("surveyId is not a valid identifier");
        }

        private static void CheckInterviewId(string interviewId)
        {
            Ensure.ArgumentNotNullOrEmptyString(interviewId, nameof(interviewId));
            if (!Guid.TryParse(interviewId, out var _))
                throw new ArgumentException("interviewId is not a valid identifier");
        }

        private INfieldHttpClient Client => ConnectionClient.Client;

        private string DeleteIntervieApiUri(string surveyId, string interviewId)
        {
            var uriText = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            uriText.Append($"Surveys/{surveyId}/DeleteInterview/{interviewId}");
            return new Uri(uriText.ToString()).AbsoluteUri;
        }
    }
}

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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldInterviewQualityService"/>
    /// </summary>
    internal class NfieldInterviewQualityService : INfieldInterviewQualityService, INfieldConnectionClientObject
    {
        public Task<InterviewDetailsModel> QueryAsync(string surveyId, string interviewId)
        {
            CheckSurveyId(surveyId);
            CheckInterviewId(interviewId);

            return Client.GetAsync(InterviewQualityApi(surveyId, interviewId).AbsoluteUri)
                .ContinueWith(
                    responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(
                    stringTask =>
                        JsonConvert.DeserializeObject<InterviewDetailsModel>(stringTask.Result))
                .FlattenExceptions();
        }

        public Task<IQueryable<InterviewDetailsModel>> QueryAsync(string surveyId)
        {
            CheckSurveyId(surveyId);

            return Client.GetAsync(InterviewQualityApi(surveyId, null).AbsoluteUri)
                .ContinueWith(
                    responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(
                    stringTask =>
                        JsonConvert.DeserializeObject<List<InterviewDetailsModel>>(stringTask.Result).AsQueryable())
                .FlattenExceptions();
        }

        public Task<InterviewDetailsModel> PutAsync(string surveyId, InterviewDetailsModel interviewDetails)
        {
            return PutAsync(surveyId, interviewDetails.Id, interviewDetails.InterviewQuality);
        }

        public Task<InterviewDetailsModel> PutAsync(string surveyId, string interviewId, int newQualityState)
        {
            return PutAsync(surveyId, interviewId, (InterviewQuality)newQualityState);
        }

        private Task<InterviewDetailsModel> PutAsync(string surveyId, string interviewId, InterviewQuality newQualityState)
        {
            CheckSurveyId(surveyId);
            CheckInterviewId(interviewId);

            var model = new QualityNewStateChange {InterviewId = interviewId, NewState = newQualityState};

            return Client.PutAsJsonAsync(InterviewQualityApi(surveyId, null).AbsoluteUri, model).
                ContinueWith(
                    responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(
                    stringTask => JsonConvert.DeserializeObjectAsync<InterviewDetailsModel>(stringTask.Result).Result)
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
            if (surveyId == null)
                throw new ArgumentNullException("surveyId");
            if (surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be empty");
        }

        private static void CheckInterviewId(string interviewId)
        {
            if (interviewId == null)
                throw new ArgumentNullException("interviewId");
            if (interviewId.Trim().Length == 0)
                throw new ArgumentException("interviewId cannot be empty");
        }

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri InterviewQualityApi(string surveyId, string interviewId)
        {
            var uriText = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            uriText.AppendFormat("Surveys/{0}/InterviewQuality", surveyId);
            if (!string.IsNullOrEmpty(interviewId))
                uriText.AppendFormat("/{0}", interviewId);
            return new Uri(uriText.ToString());
        }
    }
}

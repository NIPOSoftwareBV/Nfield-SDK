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
using System.Net.Http;
using System.Threading.Tasks;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveySettingsService"/>
    /// </summary>
    internal class NfieldSurveyFieldworkService : INfieldSurveyFieldworkService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyFieldworkService

        /// <summary>
        /// See <see cref="INfieldSurveyFieldworkService.GetStatusAsync"/>
        /// </summary>
        public Task<SurveyStatus> GetStatusAsync(string surveyId)
        {
            CheckSurveyId(surveyId);

            var uri = string.Format(@"{0}{1}/{2}/{3}", SurveysApi.AbsoluteUri, surveyId, SurveyFieldworkControllerName, "Status");
            return Client.GetAsync(uri)
                .ContinueWith(
                    responseMessageTask => responseMessageTask.Result.Content.ReadAsAsync<int>().Result)
                .ContinueWith(stringTask => (SurveyStatus) stringTask.Result)
                .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveyFieldworkService.StartFieldworkAsync"/>
        /// </summary>
        public Task StartFieldworkAsync(string surveyId)
        {
            CheckSurveyId(surveyId);

            var uri = string.Format(@"{0}{1}/{2}/{3}", SurveysApi.AbsoluteUri, surveyId, SurveyFieldworkControllerName, "Start");

            return Client.PutAsync(uri, new StringContent(string.Empty)).FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveyFieldworkService.StopFieldworkAsync"/>
        /// </summary>
        public Task StopFieldworkAsync(string surveyId, StopFieldworkModel model)
        {
            CheckSurveyId(surveyId);

            var uri = string.Format(@"{0}{1}/{2}/{3}", SurveysApi.AbsoluteUri, surveyId, SurveyFieldworkControllerName, "Stop");

            return Client.PutAsJsonAsync(uri, model).FlattenExceptions();
        }

        #endregion

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

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri SurveysApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri.AbsoluteUri + "surveys/"); }
        }

        public string SurveyFieldworkControllerName { get { return "Fieldwork"; } }
    }
}

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
using Nfield.SDK.Services;
using Nfield.Utilities;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveyPerformanceService"/>
    /// </summary>
    internal class NfieldSurveyPerformanceService : INfieldSurveyPerformanceService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyPerformanceService

        /// <summary>
        /// See <see cref="INfieldSurveyPerformanceService.GetLiveMetricsAsync(string)"/>
        /// </summary>
        public Task<SurveyMetrics> GetLiveMetricsAsync(string surveyId)
        {
            CheckSurveyId(surveyId);

            var uri = new Uri(SurveysApi, $"{surveyId}/{SurveyPerformanceControllerName}/Metrics/Live");

            return Client.GetAsync(uri)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsAsync<SurveyMetrics>().Result)
                .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveyPerformanceService.GetTestMetricsAsync(string)"/>
        /// </summary>
        public Task<SurveyMetrics> GetTestMetricsAsync(string surveyId)
        {
            CheckSurveyId(surveyId);

            var uri = new Uri(SurveysApi, $"{surveyId}/{SurveyPerformanceControllerName}/Metrics/Test");

            return Client.GetAsync(uri)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsAsync<SurveyMetrics>().Result)
                .FlattenExceptions();
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        /// <summary>
        /// See <see cref="INfieldConnectionClientObject.ConnectionClient"/>
        /// </summary>
        public INfieldConnectionClient ConnectionClient { get; internal set; }

        /// <summary>
        /// See <see cref="INfieldConnectionClientObject.InitializeNfieldConnection(INfieldConnectionClient)"/>
        /// </summary>
        public void InitializeNfieldConnection(INfieldConnectionClient connection)
            => ConnectionClient = connection;

        #endregion

        private static void CheckSurveyId(string surveyId)
            => Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));

        private INfieldHttpClient Client => ConnectionClient.Client;

        private Uri SurveysApi => new Uri(ConnectionClient.NfieldServerUri, "Surveys/");

        public static string SurveyPerformanceControllerName => "Performance";
    }
}

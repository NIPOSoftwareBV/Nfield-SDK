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
using Nfield.SDK.Models;
using System;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveyQuotaFrameService"/>
    /// </summary>
    internal class NfieldSurveyQuotaFrameService : INfieldSurveyQuotaFrameService, INfieldConnectionClientObject
    {

        private const string QuotaControllerName = "SurveyQuotaFrame";

        /// <summary>
        /// See <see cref="INfieldSurveysQuotaFrameService.QuotaQueryAsync"/>
        /// </summary>
        /// <returns></returns>
        public Task<SurveyQuotaFrameModel> QuotaQueryAsync(string surveyId)
        {
            ValidateSurveyId(surveyId);

            var uri = new Uri(SurveysApi, $"{surveyId}/{QuotaControllerName}");

            return Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<SurveyQuotaFrameModel>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldSurveyQuotaFrameService.CreateOrUpdateQuotaAsync"/>
        /// </summary>
        public Task<SurveyQuotaFrameModel> CreateOrUpdateQuotaAsync(string surveyId, SurveyQuotaFrameModel quota)
        {
            ValidateSurveyId(surveyId);

            var uri = new Uri(SurveysApi, $"{surveyId}/{QuotaControllerName}");

            return Client.PutAsJsonAsync(uri, quota)
                         .ContinueWith(
                            responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                            stringTask => JsonConvert.DeserializeObject<SurveyQuotaFrameModel>(stringTask.Result))
                         .FlattenExceptions();
        }


        /// <summary>
        /// See <see cref="INfieldSurveyQuotaFrameService.UpdateQuotaTargetsAsync"/>
        /// </summary>
        public Task<SurveyQuotaFrameEtagModel> UpdateQuotaTargetsAsync(string surveyId, string eTag, SurveyQuotaFrameEtagModel targets)
        {
            ValidateSurveyId(surveyId);

            var uri = new Uri(SurveysApi, $"{surveyId}/{QuotaControllerName}/{eTag}");

            return Client.PutAsJsonAsync(uri, targets)
             .ContinueWith(
                responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                stringTask => JsonConvert.DeserializeObject<SurveyQuotaFrameEtagModel>(stringTask.Result))
             .FlattenExceptions();
        }


        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        internal static void ValidateSurveyId(string surveyId)
        {
            if (surveyId == null || surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be null or empty");
        }

        private Uri SurveysApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri, "Surveys/"); }
        }


    }
}
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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveysQuotaFrameService"/>
    /// </summary>
    internal class NfieldSurveysQuotaFrameService : INfieldSurveysQuotaFrameService, INfieldConnectionClientObject
    {

        private const string QuotaControllerName = "SurveyQuotaFrame";

        /// <summary>
        /// See <see cref="INfieldSurveysQuotaFrameService.QuotaQueryAsync"/>
        /// </summary>
        /// <returns></returns>
        public Task<SDK.Models.SurveyQuotaFrame> QuotaQueryAsync(string surveyId)
        {
            ValidateSurveyId(surveyId);

            var uri = new Uri(SurveysApi, $"{surveyId}/{QuotaControllerName}");

            return Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<SDK.Models.SurveyQuotaFrame>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldSurveysQuotaFrameService.CreateOrUpdateQuotaAsync"/>
        /// </summary>
        public Task<SDK.Models.SurveyQuotaFrame> CreateOrUpdateQuotaAsync(string surveyId, SDK.Models.SurveyQuotaFrame quota)
        {
            ValidateSurveyId(surveyId);

            var uri = new Uri(SurveysApi, $"{surveyId}/{QuotaControllerName}");

            return Client.PutAsJsonAsync(uri, quota)
                         .ContinueWith(
                            responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                            stringTask => JsonConvert.DeserializeObject<SDK.Models.SurveyQuotaFrame>(stringTask.Result))
                         .FlattenExceptions();
        }


        /// <summary>
        /// See <see cref="INfieldSurveysQuotaFrameService.UpdateQuotaTargetsAsync"/>
        /// </summary>
        public async Task<IEnumerable<QuotaFrameLevelTarget>> UpdateQuotaTargetsAsync(string surveyId, string eTag, IEnumerable<QuotaFrameLevelTarget> targets)
        {
            ValidateSurveyId(surveyId);

            var uri = new Uri(SurveysApi, $"Surveys/{surveyId}/{QuotaControllerName}/{eTag}");

            return (IEnumerable<QuotaFrameLevelTarget>)Client.PutAsJsonAsync(uri, targets)
             .ContinueWith(
                responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                stringTask => JsonConvert.DeserializeObject<IEnumerable<QuotaFrameLevelTarget>>(stringTask.Result))
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

        private static void ValidateSurveyId(string surveyId)
        {
            if (surveyId == null)
                throw new ArgumentNullException(nameof(surveyId));

            if (surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be empty");
        }

        private Uri SurveysApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri, "Surveys/"); }
        }


    }
}
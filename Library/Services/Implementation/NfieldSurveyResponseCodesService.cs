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
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveyResponseCodesService"/>
    /// </summary>
    internal class NfieldSurveyResponseCodesService : INfieldSurveyResponseCodesService, INfieldConnectionClientObject
    {

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri SurveyResponseCodesApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri.AbsoluteUri + "SurveyResponseCodes/"); }
        }

        /// <summary>
        /// <see cref="INfieldSurveyResponseCodesService.QueryAsync(string)"/>
        /// </summary>
        public Task<IQueryable<SurveyResponseCode>> QueryAsync(string surveyId)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }

            return Client.GetAsync(SurveyResponseCodesApi.AbsoluteUri + surveyId)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(
                    stringTask => JsonConvert.DeserializeObject<List<SurveyResponseCode>>(stringTask.Result).AsQueryable())
                .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldSurveyResponseCodesService.QueryAsync(string, int)"/>
        /// </summary>
        public Task<SurveyResponseCode> QueryAsync(string surveyId, int code)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }

            return
                Client.GetAsync(SurveyResponseCodeUrl(surveyId, code))
                    .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                    .ContinueWith(
                        stringTask => JsonConvert.DeserializeObject<SurveyResponseCode>(stringTask.Result))
                    .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldSurveyResponseCodesService.AddAsync"/>
        /// </summary>
        public Task<SurveyResponseCode> AddAsync(string surveyId, SurveyResponseCode responseCode)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }

            if (responseCode == null)
            {
                throw new ArgumentNullException("responseCode");
            }

            return Client.PostAsJsonAsync(SurveyResponseCodesApi.AbsoluteUri + surveyId, responseCode)
                .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(task => JsonConvert.DeserializeObjectAsync<SurveyResponseCode>(task.Result).Result)
                .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldSurveyResponseCodesService.UpdateAsync"/>
        /// </summary>
        public Task<SurveyResponseCode> UpdateAsync(string surveyId, SurveyResponseCode responseCode)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }

            if(responseCode == null)
            {
                throw new ArgumentNullException("responseCode");
            }

            var updatedresponseCode = new UpdateSurveyResponseCode
            {
                ResponseCodeDescription = responseCode.Description,
                IsDefinite = responseCode.IsDefinite,
                IsSelectable = responseCode.IsSelectable,
                AllowAppointment = responseCode.AllowAppointment
            };
            
            return
                Client.PatchAsJsonAsync(SurveyResponseCodeUrl(surveyId, responseCode.ResponseCode), updatedresponseCode)
                    .ContinueWith(
                        responseMessageTask => 
                            responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                    .ContinueWith(
                        stringTask => JsonConvert.DeserializeObjectAsync<SurveyResponseCode>(stringTask.Result).Result)
                    .FlattenExceptions();
        }


        /// <summary>
        /// <see cref="INfieldSurveyResponseCodesService.RemoveAsync"/>
        /// </summary>
        public Task RemoveAsync(string surveyId, int code)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }

            return
                Client.DeleteAsync(SurveyResponseCodeUrl(surveyId, code))
                      .FlattenExceptions();
        }
        
        #region Implementation of INfieldConnectionClientObject

        /// <summary>
        /// <see cref="INfieldConnectionClientObject.ConnectionClient"/>
        /// </summary>
        public INfieldConnectionClient ConnectionClient { get; internal set; }

        /// <summary>
        /// <see cref="INfieldConnectionClientObject.InitializeNfieldConnection"/>
        /// </summary>
        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        /// <summary>
        /// Constructs and returns the url for survey response code 
        /// based on supplied <paramref name="surveyId"/>  and <paramref name="code"/>
        /// </summary>
        private string SurveyResponseCodeUrl(string surveyId, int code)
        {
            return SurveyResponseCodesApi.AbsoluteUri + surveyId + string.Format("?responseCode={0}", code);
        }
    }

    /// <summary>
    /// Update model for a survey response code
    /// </summary>
    internal class UpdateSurveyResponseCode
    {
        /// <summary>
        /// User defined description of the response code given
        /// </summary>
        public string ResponseCodeDescription { get; set; }

        /// <summary>
        /// Determines if the Response code is a definitive or not (IsFinal - true or false)
        /// </summary>
        public bool? IsDefinite { get; set; }

        /// <summary>
        /// Determines if response code is selectable by the interviewer
        /// </summary>
        public bool? IsSelectable { get; set; }

        /// <summary>
        /// Determines if the Response code is meant for an appointment or not (IsIntermediate - true or false)
        /// </summary>
        public bool? AllowAppointment { get; set; }
    }
}
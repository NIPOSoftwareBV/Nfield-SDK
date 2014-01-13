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
            get { return new Uri(ConnectionClient.NfieldServerUri.AbsoluteUri + "SurveyResponseCode/"); }
        }

        /// <summary>
        /// <see cref="INfieldSurveyResponseCodesService.QueryAsync(string)"/>
        /// </summary>
        public Task<IQueryable<SurveyResponseCode>> QueryAsync(string surveyId)
        {
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
            return
                Client.GetAsync(SurveyResponseCodesApi.AbsoluteUri + surveyId + string.Format("?responseCode={0}", code))
                    .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                    .ContinueWith(
                        stringTask => JsonConvert.DeserializeObject<SurveyResponseCode>(stringTask.Result))
                    .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldSurveyResponseCodesService.AddAsync"/>
        /// </summary>
        public Task<SurveyResponseCode> AddAsync(string surveyId, SurveyResponseCode resposneCode)
        {
            if (resposneCode == null)
            {
                throw new ArgumentNullException("resposneCode");
            }

            return Client.PostAsJsonAsync(SurveyResponseCodesApi + surveyId, resposneCode)
                .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(task => JsonConvert.DeserializeObjectAsync<SurveyResponseCode>(task.Result).Result)
                .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldSurveyResponseCodesService.UpdateAsync"/>
        /// </summary>
        public Task<SurveyResponseCode> UpdateAsync(string surveyId, SurveyResponseCode resposneCode)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// <see cref="INfieldSurveyResponseCodesService.RemoveAsync"/>
        /// </summary>
        public Task RemoveAsync(string surveyId, int code)
        {
            return
                Client.DeleteAsync(SurveyResponseCodesApi + surveyId + string.Format("?responseCode={0}", code))
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

    }
}
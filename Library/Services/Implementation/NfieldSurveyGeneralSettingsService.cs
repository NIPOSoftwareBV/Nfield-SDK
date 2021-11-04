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
using System.Threading.Tasks;

namespace Nfield.Services.Implementation

{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveyGeneralSettingsService"/>
    /// </summary>
    internal class NfieldSurveyGeneralSettingsService : INfieldSurveyGeneralSettingsService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyGeneralSettingsService

        /// <summary>
        /// See <see cref="INfieldSurveyGeneralSettingsService.QueryAsync"/>
        /// </summary>
        public Task<SurveyGeneralSettings> QueryAsync(string surveyId)
        {
            CheckSurveyId(surveyId);

            return Client.GetAsync(SurveyGeneralSettingsApi(surveyId))
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<SurveyGeneralSettings>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveyGeneralSettingsService.UpdateAsync"/>
        /// </summary>
        public Task UpdateAsync(string surveyId, SurveyGeneralSettings generalSettings)
        {
            CheckSurveyId(surveyId);

            return Client.PatchAsJsonAsync(SurveyGeneralSettingsApi(surveyId), generalSettings)
                .FlattenExceptions();
        }

        #endregion

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

        private Uri SurveyGeneralSettingsApi(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/GeneralSettings");
        }

        private INfieldHttpClient Client => ConnectionClient.Client;

        private static void CheckSurveyId(string surveyId)
        {
            if (string.IsNullOrEmpty(surveyId))
                throw new ArgumentNullException(nameof(surveyId));
            if (surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be empty");
        }
    }
}

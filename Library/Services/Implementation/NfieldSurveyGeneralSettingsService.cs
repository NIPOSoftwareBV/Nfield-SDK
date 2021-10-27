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
    /// Implementation of <see cref="INfieldSurveyGeneralSettingsService"/>
    /// </summary>
    class NfieldSurveyGeneralSettingsService : INfieldSurveyGeneralSettingsService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyGeneralSettingsService

        /// <summary>
        /// See <see cref="INfieldSurveyGeneralSettingsService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<SurveyGeneralSetting>> QueryAsync(string surveyId)
        {
            CheckSurveyId(surveyId);

            return Client.GetAsync(SurveyGeneralSettingsApi(surveyId))
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<SurveyGeneralSetting>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveyGeneralSettingsService.PatchAsync"/>
        /// </summary>
        public Task UpdateAsync(string surveyId, IEnumerable<SurveyGeneralSetting> models)
        {
            CheckSurveyId(surveyId);

            return Client.PatchAsJsonAsync(SurveyGeneralSettingsApi(surveyId), models)
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
            return new Uri(ConnectionClient.NfieldServerUri, $"SurveyGeneralSettings/{surveyId}");
        }

        private INfieldHttpClient Client => ConnectionClient.Client;

        private static void CheckSurveyId(string surveyId)
        {
            if (surveyId == null)
                throw new ArgumentNullException(nameof(surveyId));
            if (surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be empty");
        }
    }
}

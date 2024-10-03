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
    /// Implementation of <see cref="INfieldSurveySettingsService"/>
    /// </summary>
    internal class NfieldSurveySettingsService : INfieldSurveySettingsService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldLanguagesService

        /// <summary>
        /// See <see cref="INfieldSurveySettingsService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<SurveySetting>> QueryAsync(string surveyId)
        {
            CheckSurveyId(surveyId);

            return Client.GetAsync(SettingsApi(surveyId, null))
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<SurveySetting>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveySettingsService.AddOrUpdateAsync"/>
        /// </summary>
        public Task<SurveySetting> AddOrUpdateAsync(string surveyId, SurveySetting setting)
        {
            CheckSurveyId(surveyId);

            if (setting == null)
            {
                throw new ArgumentNullException("setting");
            }

            return Client.PostAsJsonAsync(SettingsApi(surveyId, null), setting)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<SurveySetting>(task.Result))
                         .FlattenExceptions();
        }

        public async Task<SurveySetting> AddOrUpdateAsync(string surveyId, Models.Enum.SurveySetting setting)
        {
            return await AddOrUpdateAsync(surveyId, new SurveySetting { Name = setting.Name.ToString(), Value = setting.Value });
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

        private Uri SettingsApi(string surveyId, string id)
        {
            var path = new StringBuilder();
            path.AppendFormat("Surveys/{0}/Settings", surveyId);
            if (!string.IsNullOrEmpty(id))
                path.AppendFormat("/{0}", id);
            return new Uri(ConnectionClient.NfieldServerUri, path.ToString());
        }
    }
}

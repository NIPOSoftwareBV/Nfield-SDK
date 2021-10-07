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
using Nfield.SDK.Models;

namespace Nfield.Services.Implementation

{
    class NfieldSurveyLanguageTranslationsService : INfieldSurveyLanguageTranslationsService, INfieldConnectionClientObject
    {
        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        /// <summary>
        /// Implementation of <see cref="INfieldSurveyLanguageTranslationsService"/>
        /// </summary>

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        #region Implementation of INfieldLanguageTranslationsService

        public Task<IQueryable<Language>> QueryAsync(string surveyId)
        {
            CheckSurveyId(surveyId);

            return Client.GetAsync(LanguageTranslationsApi(surveyId))
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask =>
                 JsonConvert.DeserializeObject<List<Language>>(stringTask.Result).AsQueryable())
             .FlattenExceptions();
        }

        public Task<SurveyLanguageTranslations> AddAsync(string surveyId, SurveyLanguageTranslations translations)
        {
            CheckSurveyId(surveyId);

            return Client.PostAsJsonAsync(LanguageTranslationsApi(surveyId), translations)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<SurveyLanguageTranslations>(task.Result))
                         .FlattenExceptions();
        }

        public Task<SurveyLanguageTranslations> UpdateAsync(string surveyId, int languageId, SurveyLanguageTranslations translations)
        {
            CheckSurveyId(surveyId);

            return Client.PatchAsJsonAsync(LanguageTranslationsApi(surveyId, languageId), translations)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<SurveyLanguageTranslations>(task.Result))
                         .FlattenExceptions();
        }

        public Task RemoveAsync(string surveyId, int languageId)
        {
            CheckSurveyId(surveyId);

            return
                Client.DeleteAsync(LanguageTranslationsApi(surveyId, languageId))
                    .FlattenExceptions();
        }

        #endregion

        private static void CheckSurveyId(string surveyId)
        {
            if (surveyId == null)
                throw new ArgumentNullException("surveyId");
            if (surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be empty");
        }

        private Uri LanguageTranslationsApi(string surveyId, int? languageId = null)
        {
            var path = new StringBuilder();
            path.AppendFormat("Surveys/{0}/LanguageTranslations", surveyId);
            if (languageId != null)
            {
                path.AppendFormat("/{0}", languageId);
            }

            return new Uri(ConnectionClient.NfieldServerUri, path.ToString());
        }
    }
}

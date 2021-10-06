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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.SDK.Models;

namespace Nfield.Services.Implementation

{
    class NfieldLanguageTranslationsService : INfieldLanguageTranslationsService, INfieldConnectionClientObject
    {

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

        public Task<IQueryable<SurveyLanguageTranslations>> QueryAsync(string surveyId)
        {
            CheckSurveyId(surveyId);

            return Client.GetAsync(LanguageTranslationsApi(surveyId))
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask =>
                 JsonConvert.DeserializeObject<IQueryable<SurveyLanguageTranslations>>(stringTask.Result).AsQueryable())
             .FlattenExceptions();
        }

        public Task<SurveyLanguageTranslations> AddAsync(string surveyId, string languageId, SurveyLanguageTranslations translations)
        {
            CheckSurveyId(surveyId);
            CheckLanguageId(languageId);

            return Client.PostAsJsonAsync(LanguageTranslationsApi(surveyId, languageId), translations)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<SurveyLanguageTranslations>(task.Result))
                         .FlattenExceptions();
        }

        public Task<SurveyLanguageTranslations> UpdateAsync(string surveyId, string languageId, SurveyLanguageTranslations translations)
        {
            CheckSurveyId(surveyId);
            CheckLanguageId(languageId);

            return Client.PatchAsJsonAsync(LanguageTranslationsApi(surveyId, languageId), translations)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<SurveyLanguageTranslations>(task.Result))
                         .FlattenExceptions();
        }

        public Task RemoveAsync(string surveyId, string languageId)
        {
            CheckSurveyId(surveyId);
            CheckLanguageId(languageId);

            return
                Client.DeleteAsync(LanguageTranslationsApi(surveyId, languageId))
                    .FlattenExceptions();
        }

        private static void CheckSurveyId(string surveyId)
        {
            if (surveyId == null)
                throw new ArgumentNullException("surveyId");
            if (surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be empty");
        }

        private static void CheckLanguageId(string languageId)
        {
            if (languageId == null)
                throw new ArgumentNullException("languageId");
            if (languageId.Trim().Length == 0)
                throw new ArgumentException("LanguageId cannot be empty");
        }

        private Uri LanguageTranslationsApi(string surveyId, string languageId = null)
        {
            var path = new StringBuilder();
            path.AppendFormat("Surveys/{0}/Languages/Translations", surveyId);
            if (!string.IsNullOrEmpty(languageId))
            {
                path.AppendFormat("/{0}", languageId);
            }

            return new Uri(ConnectionClient.NfieldServerUri, path.ToString());
        }
    }
}

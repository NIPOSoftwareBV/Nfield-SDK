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
    internal class NfieldSamplingPointsService : INfieldSamplingPointsService, INfieldConnectionClientObject
    {
        #region INfieldTranslationsService Members

        /// <summary>
        /// See <see cref="INfieldTranslationsService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<SamplingPoint>> QueryAsync(string surveyId)
        {
            CheckSurveyId(surveyId);

            return Client.GetAsync(SamplingPointsApi(surveyId))
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<SamplingPoint>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldTranslationsService.AddAsync"/>
        /// </summary>
        public Task<Translation> AddAsync(string surveyId, int languageId, Translation translation)
        {
            CheckSurveyId(surveyId);

            if (translation == null)
            {
                throw new ArgumentNullException("translation");
            }

            return Client.PostAsJsonAsync(TranslationsApi(surveyId, languageId, null), translation)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<Translation>(task.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldTranslationsService.RemoveAsync"/>
        /// </summary>
        public Task RemoveAsync(string surveyId, int languageId, Translation translation)
        {
            CheckSurveyId(surveyId);

            if (translation == null)
            {
                throw new ArgumentNullException("translation");
            }

            return
                Client.DeleteAsync(TranslationsApi(surveyId, languageId, translation.Name))
                      .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldTranslationsService.UpdateAsync"/>
        /// </summary>
        public Task UpdateAsync(string surveyId, int languageId, Translation translation)
        {
            CheckSurveyId(surveyId);

            if (translation == null)
            {
                throw new ArgumentNullException("translation");
            }

            return Client.PutAsJsonAsync(TranslationsApi(surveyId, languageId, null),
                translation).FlattenExceptions();
        }


        /// <summary>
        /// See <see cref="INfieldTranslationsService.DefaultTextsAsync"/>
        /// </summary>
        public Task<IQueryable<Translation>> DefaultTextsAsync
        {
            get
            {
                return Client.GetAsync(new Uri(ConnectionClient.NfieldServerUri, "DefaultTexts"))
                             .ContinueWith(
                                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                             .ContinueWith(
                                 stringTask =>
                                 JsonConvert.DeserializeObject<List<Translation>>(stringTask.Result).AsQueryable())
                             .FlattenExceptions();
            }
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

        private Uri SamplingPointsApi(string surveyId, string samplingPointId = null)
        {
            var path = new StringBuilder();
            path.AppendFormat("Surveys/{0}/SamplingPoints",
                    surveyId, samplingPointId);
            if (!string.IsNullOrEmpty(samplingPointId))
                path.AppendFormat("/{0}", samplingPointId);

            return new Uri(ConnectionClient.NfieldServerUri, path.ToString());
        }

    }
}

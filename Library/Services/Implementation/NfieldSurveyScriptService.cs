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
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveyScriptService"/>
    /// </summary>
    internal class NfieldSurveyScriptService : INfieldSurveyScriptService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyScriptService

        /// <summary>
        /// See <see cref="INfieldSurveyScriptService.GetAsync"/>
        /// </summary>
        public Task<SurveyScript> GetAsync(string surveyId)
        {
            return Client.GetAsync(SurveyScriptApi.AbsoluteUri + surveyId)
                .ContinueWith(
                    responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(
                    stringTask =>
                        JsonConvert.DeserializeObject<SurveyScript>(stringTask.Result))
                .FlattenExceptions();
        }

        #endregion

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

        private Uri SurveyScriptApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri.AbsoluteUri + "surveyscript/"); }
        }

        /// <summary>
        /// See <see cref="INfieldSurveyScriptService.PostAsync(string,Nfield.Models.SurveyScript)"/>
        /// </summary>
        public Task<SurveyScript> PostAsync(string surveyId, SurveyScript surveyScript)
        {
            if (surveyScript == null)
            {
                throw new ArgumentNullException("surveyScript");
            }
            return Client.PostAsJsonAsync(SurveyScriptApi.AbsoluteUri + surveyId, surveyScript)
                .ContinueWith(
                    responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(
                    stringTask =>
                        JsonConvert.DeserializeObject<SurveyScript>(stringTask.Result))
                .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveyScriptService.PostAsync(string,string)"/>
        /// </summary>
        public Task<SurveyScript> PostAsync(string surveyId, string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException(fileName);
            
            var surveyScript = new SurveyScript
            {
                FileName = fileName,
                Script = File.ReadAllText(filePath)
            };

            return PostAsync(surveyId, surveyScript);
        }
    }
}

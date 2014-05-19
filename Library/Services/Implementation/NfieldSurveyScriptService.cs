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
using System.Globalization;
using System.IO;
using System.Text;
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
        readonly IFileSystem _fileSystem;

        public NfieldSurveyScriptService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        #region Implementation of INfieldSurveyScriptService

        /// <summary>
        /// See <see cref="INfieldSurveyScriptService.GetAsync"/>
        /// </summary>
        public Task<SurveyScript> GetAsync(string surveyId)
        {
            var uri = SurveyScriptUrl(surveyId);
            return Client.GetAsync(uri)
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


        /// <summary>
        /// See <see cref="INfieldSurveyScriptService.PostAsync(string,Nfield.Models.SurveyScript)"/>
        /// </summary>
        public Task<SurveyScript> PostAsync(string surveyId, SurveyScript surveyScript)
        {
            if (surveyScript == null)
            {
                throw new ArgumentNullException("surveyScript");
            }
            var uri = SurveyScriptUrl(surveyId);
            return Client.PostAsJsonAsync(uri, surveyScript)
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

            var fileName = _fileSystem.Path.GetFileName(filePath);

            if (!_fileSystem.File.Exists(filePath))
                throw new FileNotFoundException(fileName);
            
            var surveyScript = new SurveyScript
            {
                FileName = fileName,
                Script = _fileSystem.File.ReadAllText(filePath)
            };

            return PostAsync(surveyId, surveyScript);
        }

        private string SurveyScriptUrl(string surveyId)
        {
            var result = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            result.AppendFormat(CultureInfo.InvariantCulture, @"Surveys/{0}/Script/", surveyId);

            return result.ToString();
        }
    }
}

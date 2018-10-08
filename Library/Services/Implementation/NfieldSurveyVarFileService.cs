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
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveyVarFileService"/>
    /// </summary>
    internal class NfieldSurveyVarFileService : INfieldSurveyVarFileService, INfieldConnectionClientObject
    {

        #region Implementation of INfieldSurveyVarFileService

        /// <summary>
        /// See <see cref="INfieldSurveyVarFileService.GetAsync(string)"/>
        /// </summary>
        public Task<SurveyVarFile> GetAsync(string surveyId)
        {
            var uri = SurveyVarFileUrl(surveyId);
            return Client.GetAsync(uri)
                .ContinueWith(
                    responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(
                    stringTask =>
                        JsonConvert.DeserializeObject<SurveyVarFile>(stringTask.Result))
                .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveyScriptService.GetAsync(string, string)"/> 
        /// </summary>
        public Task<SurveyVarFile> GetAsync(string surveyId, string eTag)
        {
            var uri = SurveyVarFileUrl(surveyId, eTag);
            return Client.GetAsync(uri)
                .ContinueWith(
                    responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(
                    stringTask =>
                        JsonConvert.DeserializeObject<SurveyVarFile>(stringTask.Result))
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

        private Uri SurveyVarFileUrl(string surveyId, string eTag = null)
        {
            var path = new StringBuilder();
            path.AppendFormat(CultureInfo.InvariantCulture, @"Surveys/{0}/VarFile/", surveyId);

            if (!string.IsNullOrEmpty(eTag))
                path.AppendFormat("{0}", eTag);

            return new Uri(ConnectionClient.NfieldServerUri, path.ToString());
        }
    }
}

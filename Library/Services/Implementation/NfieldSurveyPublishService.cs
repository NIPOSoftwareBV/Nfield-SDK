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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveyPublishService"/>
    /// </summary>
    internal class NfieldSurveyPublishService : INfieldSurveyPublishService, INfieldConnectionClientObject
    {
        public Task<SurveyPackageStateModel> GetAsync(string surveyId)
        {
            CheckSurveyId(surveyId);


            return Client.GetAsync(PublishSurveyApi(surveyId).AbsoluteUri)
                .ContinueWith(
                    responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(
                    stringTask =>
                        JsonConvert.DeserializeObject<SurveyPackageStateModel>(stringTask.Result))
                .FlattenExceptions();
        }

        public Task PutAsync(string surveyId, SurveyPublishTypeUpgradeModel surveyPublishTypeUpgrade)
        {
            CheckSurveyId(surveyId);
            return Client.PutAsJsonAsync(PublishSurveyApi(surveyId).AbsoluteUri, surveyPublishTypeUpgrade).
                ContinueWith(
                    responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(
                    stringTask => JsonConvert.DeserializeObjectAsync<SurveyPublishTypeUpgradeModel>(stringTask.Result).Result)
                .FlattenExceptions();
            
        }


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

        private Uri PublishSurveyApi(string surveyId)
        {
            var uriText = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            uriText.AppendFormat("Surveys/{0}/Publish", surveyId);
            return new Uri(uriText.ToString());
        }
    }
}

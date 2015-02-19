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
using System.Text;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    internal class NfieldSurveyPackageService : INfieldSurveyPackageService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyPackageService

        public System.Threading.Tasks.Task<SurveyPackage> GetSurveyPackageAsync(string surveyId, InterviewPackageType interviewPackageType)
        {
            ValidateParams(surveyId, interviewPackageType);

            return Client.GetAsync(SurveyPackageApi(surveyId, interviewPackageType).AbsoluteUri)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObjectAsync<SurveyPackage>(task.Result).Result)
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

        private Uri SurveyPackageApi(string surveyId, InterviewPackageType interviewPackageType)
        {
            var uriText = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);

            uriText.AppendFormat("Surveys/{0}/Package/?type={1}", surveyId, interviewPackageType);

            return new Uri(uriText.ToString());
        }

        private static void ValidateParams(string surveyId, InterviewPackageType interviewPackageType)
        {
            if (surveyId == null)
                throw new ArgumentNullException("surveyId");

            if (surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be empty");

            if (interviewPackageType == InterviewPackageType.Unknown)
                throw new ArgumentException("package type cannot be Unknown");
        }
    }
}

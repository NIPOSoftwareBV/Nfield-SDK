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
using Nfield.Infrastructure;
using Nfield.SDK.Models;
using Nfield.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.SDK.Services.Implementation
{
    internal class NfieldSurveysManualTestService : INfieldSurveysManualTestService, INfieldConnectionClientObject
    {
        readonly IFileSystem _fileSystem;

        public NfieldSurveysManualTestService()
        {
            _fileSystem = new FileSystem();
        }

        public NfieldSurveysManualTestService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public async Task<IQueryable<SurveyManualTest>> GetSurveyManualTestsAsync(string surveyId)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/ManualTests");

            var response = await Client.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<SurveyManualTest>>(result).AsQueryable();
        }

        public async Task<IQueryable<SurveyManualTest>> GetSurveyManualTestAsync(string surveyId, string manualTestId)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNullOrEmptyString(manualTestId, nameof(manualTestId));

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/ManualTests/{manualTestId}");

            var response = await Client.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<SurveyManualTest>>(result).AsQueryable();
        }

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
    }
}

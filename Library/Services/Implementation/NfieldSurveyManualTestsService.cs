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
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;
using Nfield.Models;
using Nfield.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace Nfield.Services.Implementation
{
    internal class NfieldSurveyManualTestsService : INfieldSurveyManualTestsService, INfieldConnectionClientObject
    {
        readonly IFileSystem _fileSystem;

        public NfieldSurveyManualTestsService()
        {
            _fileSystem = new FileSystem();
        }

        public NfieldSurveyManualTestsService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public async Task<IQueryable<SurveyManualTest>> GetManualTestsAsync()
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"ManualTests");

            var response = await Client.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<SurveyManualTest>>(result).AsQueryable();
        }

        public async Task<IQueryable<SurveyManualTest>> GetSurveyManualTestsAsync(string surveyId)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/ManualTests");

            var response = await Client.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<SurveyManualTest>>(result).AsQueryable();
        }

        public async Task<ManualTestSurveyResult> StartCreateManualTestSurveyAsync(string surveyId, StartCreateManualTestSurvey request)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNull(request, nameof(request));

            using (var content = MultipartDataContent(request))
            {
                var uri = new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/ManualTests");

                using (var response = await Client.PostAsync(uri, content).ConfigureAwait(false))
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var backgroundActivityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(responseContent);
                    return await ConnectionClient.GetActivityResultAsync<ManualTestSurveyResult>(backgroundActivityStatus.ActivityId).ConfigureAwait(false);
                }
            }
        }

        public Task<ManualTestSurveyResult> StartCreateManualTestSurveyAsync(string surveyId, StartCreateManualTestSurveyFile request)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNull(request, nameof(request));

            var mappedRequest = new StartCreateManualTestSurvey
            {
                EnableReporting = request.EnableReporting,
                UseOriginalSample = request.UseOriginalSample
            };

            if (!string.IsNullOrEmpty(request.SampleDataFilePath))
            {
                var fileName = _fileSystem.Path.GetFileName(request.SampleDataFilePath);

                if (!_fileSystem.File.Exists(request.SampleDataFilePath))
                {
                    throw new FileNotFoundException(fileName);
                }

                mappedRequest.SampleDataFileName = fileName;
                mappedRequest.SampleDataFile = _fileSystem.File.ReadAllBytes(request.SampleDataFilePath);
            }

            return StartCreateManualTestSurveyAsync(surveyId, mappedRequest);
        }

        private static MultipartFormDataContent MultipartDataContent(StartCreateManualTestSurvey request)
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent($"{request.EnableReporting}"), nameof(request.EnableReporting) },
                { new StringContent($"{request.UseOriginalSample}"), nameof(request.UseOriginalSample) }
            };

            if (!string.IsNullOrEmpty(request.SampleDataFileName) && request.SampleDataFile?.Length > 0)
            {
                var bytesContent = new ByteArrayContent(request.SampleDataFile);
                bytesContent.Headers.Add("Content-Type", "application/octet-stream");
                bytesContent.Headers.Add("Content-Disposition", $"form-data; name={nameof(request.SampleDataFile)}; filename={request.SampleDataFileName}");
                content.Add(bytesContent, nameof(request.SampleDataFile), request.SampleDataFileName);
            }
            return content;
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

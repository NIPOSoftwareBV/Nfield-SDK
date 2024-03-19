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
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;
using Nfield.Utilities;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveyInterviewSimulationService"/>
    /// </summary>
    internal class NfieldSurveyInterviewSimulationService : INfieldSurveyInterviewSimulationService, INfieldConnectionClientObject
    {
        readonly IFileSystem _fileSystem;

        public NfieldSurveyInterviewSimulationService()
        {
            _fileSystem = new FileSystem();
        }

        public NfieldSurveyInterviewSimulationService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// <see cref="INfieldSurveyInterviewSimulationService.GetHintsAsync(string)"/>
        /// </summary>
        public async Task<Uri> GetHintsAsync(string surveyId)
        {
            using (var response = await Client.GetAsync(SurveySimulationHintsEndPoint(surveyId)).ConfigureAwait(false))
            {
                var uri = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return new Uri(uri);
            }
        }

        /// <summary>
        /// <see cref="INfieldSurveyInterviewSimulationService.StartSimulationAsync(string, InterviewSimulation)"/>
        /// </summary>
        public async Task StartSimulationAsync(string surveyId, InterviewSimulation simulationRequest)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNull(simulationRequest, nameof(simulationRequest));

            var content = MultipartDataContent(simulationRequest);

            var response = await Client.PostAsync(StartInterviewSimulationsEndPoint(surveyId), content).ConfigureAwait(false);
            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var backgroundActivityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(result);
            _ = await ConnectionClient.GetActivityResultAsync<string>(backgroundActivityStatus.ActivityId, "Status").ConfigureAwait(false);
        }

        /// <summary>
        /// <see cref="INfieldSurveyInterviewSimulationService.StartSimulationAsync(string, InterviewSimulationFiles)"/>
        /// </summary>
        public async Task StartSimulationAsync(string surveyId, InterviewSimulationFiles simulationRequest)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNull(simulationRequest, nameof(simulationRequest));

            var interviewSimulation = new InterviewSimulation
            {
                InterviewsCount = simulationRequest.InterviewsCount,
                EnableReporting = simulationRequest.EnableReporting,
                UseOriginalSample = simulationRequest.UseOriginalSample
            };

            if (!string.IsNullOrEmpty(simulationRequest.HintsFilePath))
            {
                var fileName = _fileSystem.Path.GetFileName(simulationRequest.HintsFilePath);

                if (!_fileSystem.File.Exists(simulationRequest.HintsFilePath))
                    throw new FileNotFoundException(fileName);

                interviewSimulation.HintsFileName = fileName;
                interviewSimulation.Hints = _fileSystem.File.ReadAllText(simulationRequest.HintsFilePath);
            }

            if (!string.IsNullOrEmpty(simulationRequest.SampleDataFilePath))
            {
                var fileName = _fileSystem.Path.GetFileName(simulationRequest.SampleDataFilePath);

                if (!_fileSystem.File.Exists(simulationRequest.SampleDataFilePath))
                    throw new FileNotFoundException(fileName);

                interviewSimulation.SampleDataFileName = fileName;
                interviewSimulation.SampleData = _fileSystem.File.ReadAllText(simulationRequest.HintsFilePath);
            }

            await StartSimulationAsync(surveyId, interviewSimulation);
        }

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; private set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        #endregion

        #region Helpers

        private Uri SurveySimulationHintsEndPoint(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"surveys/{surveyId}/InterviewSimulations/DownloadHints");
        }

        private Uri StartInterviewSimulationsEndPoint(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"surveys/{surveyId}/InterviewSimulations/StartInterviewSimulations");
        }

        private static MultipartFormDataContent MultipartDataContent(InterviewSimulation simulationRequest)
        {
            var content = new MultipartFormDataContent
            {
                { new StringContent($"{simulationRequest.InterviewsCount}"), nameof(simulationRequest.InterviewsCount) },
                { new StringContent($"{simulationRequest.EnableReporting}"), nameof(simulationRequest.EnableReporting) },
                { new StringContent($"{simulationRequest.UseOriginalSample}"), nameof(simulationRequest.UseOriginalSample) }
            };

            if (!string.IsNullOrEmpty(simulationRequest.HintsFileName) && !string.IsNullOrEmpty(simulationRequest.Hints))
            {
                content.Add(new StringContent(simulationRequest.Hints), nameof(simulationRequest.Hints), simulationRequest.HintsFileName);
            }

            if (!string.IsNullOrEmpty(simulationRequest.SampleDataFileName) && !string.IsNullOrEmpty(simulationRequest.SampleData))
            {
                content.Add(new StringContent(simulationRequest.SampleData), nameof(simulationRequest.SampleData), simulationRequest.SampleDataFileName);
            }

            content.Headers.ContentType.MediaType = "multipart/form-data";

            return content;
        }

        #endregion
    }
}

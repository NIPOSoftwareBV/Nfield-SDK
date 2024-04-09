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
        public async Task<string> GetHintsAsync(string surveyId)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));

            using (var response = await Client.GetAsync(SurveySimulationHintsEndPoint(surveyId)).ConfigureAwait(false))
            {
                var hints = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return hints;
            }
        }

        /// <summary>
        /// <see cref="INfieldSurveyInterviewSimulationService.StartSimulationAsync(string, InterviewSimulation)"/>
        /// </summary>
        public async Task<InterviewSimulationResult> StartSimulationAsync(string surveyId, InterviewSimulation simulationRequest)
        {
            Ensure.ArgumentNotNullOrEmptyString(surveyId, nameof(surveyId));
            Ensure.ArgumentNotNull(simulationRequest, nameof(simulationRequest));

            using (var content = MultipartDataContent(simulationRequest))
            {
                using (var response = await Client.PostAsync(StartInterviewSimulationsEndPoint(surveyId), content).ConfigureAwait(false))
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var backgroundActivityStatus = JsonConvert.DeserializeObject<BackgroundActivityStatus>(responseContent);
                    return await ConnectionClient.GetActivityResultAsync<InterviewSimulationResult>(backgroundActivityStatus.ActivityId).ConfigureAwait(false);
                }
            }
        }
        
        /// <summary>
        /// <see cref="INfieldSurveyInterviewSimulationService.StartSimulationAsync(string, InterviewSimulationFiles)"/>
        /// </summary>
        public Task<InterviewSimulationResult> StartSimulationAsync(string surveyId, InterviewSimulationFiles simulationRequest)
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
                interviewSimulation.HintsFile = _fileSystem.File.ReadAllText(simulationRequest.HintsFilePath);
            }

            if (!string.IsNullOrEmpty(simulationRequest.SampleDataFilePath))
            {
                var fileName = _fileSystem.Path.GetFileName(simulationRequest.SampleDataFilePath);

                if (!_fileSystem.File.Exists(simulationRequest.SampleDataFilePath))
                    throw new FileNotFoundException(fileName);

                interviewSimulation.SampleDataFileName = fileName;
                interviewSimulation.SampleDataFile = _fileSystem.File.ReadAllBytes(simulationRequest.SampleDataFilePath);
            }

            return StartSimulationAsync(surveyId, interviewSimulation);
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

            if (!string.IsNullOrEmpty(simulationRequest.HintsFileName) && !string.IsNullOrEmpty(simulationRequest.HintsFile))
            {
                content.Add(new StringContent(simulationRequest.HintsFile), nameof(simulationRequest.HintsFile), simulationRequest.HintsFileName);
            }

            if (!string.IsNullOrEmpty(simulationRequest.SampleDataFileName) && simulationRequest.SampleDataFile?.Length > 0)
            {
                var bytesContent = new ByteArrayContent(simulationRequest.SampleDataFile);
                bytesContent.Headers.Add("Content-Type", "application/octet-stream");
                bytesContent.Headers.Add("Content-Disposition", $"form-data; name={nameof(simulationRequest.SampleDataFile)}; filename={simulationRequest.SampleDataFileName}");
                content.Add(bytesContent, nameof(simulationRequest.SampleDataFile), simulationRequest.SampleDataFileName);
            }
            return content;
        }

        #endregion
    }
}

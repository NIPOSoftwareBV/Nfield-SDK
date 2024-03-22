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

using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;
using Nfield.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyInterviewSimulationService"/>
    /// </summary>
    public class NfieldSurveyInterviewSimulationTests : NfieldServiceTestsBase
    {
        private readonly NfieldSurveyInterviewSimulationService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;
        private readonly string _surveyId;
        private readonly Uri _endpoint;

        public NfieldSurveyInterviewSimulationTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldSurveyInterviewSimulationService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            _surveyId = Guid.NewGuid().ToString();
            _endpoint = new Uri(ServiceAddress, $"surveys/{_surveyId}/InterviewSimulations/DownloadHints");
        }

        [Fact]
        public async Task TestGetHintsAsync()
        {
            const string uri = "https://nfieldpurple.blob.core.windows.net/survey-simulation-hints/2f34c076-da0a-4b75-98a9-306088025668";
            var content = new StringContent(uri);

            _mockedHttpClient
                .Setup(client => client.GetAsync(_endpoint))
                .Returns(CreateTask(HttpStatusCode.OK, content))
                .Verifiable();

            var actual = await _target.GetHintsAsync(_surveyId);
            Assert.Equal(actual, new Uri(uri));
        }

        #region StartSimulation

        [Fact]
        public void TestStartSimulation_SurveyIdIsNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                () => _target.StartSimulationAsync(null, new InterviewSimulation()));
        }

        [Fact]
        public void TestStartSimulation_SurveyIdIsEmptyString_Throws()
        {
            Assert.ThrowsAsync<ArgumentException>(
                () => _target.StartSimulationAsync(string.Empty, new InterviewSimulation()));
        }

        [Fact]
        public void TestStartSimulation_InterviewSimulationIsNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                () => _target.StartSimulationAsync("surveyId", (InterviewSimulation)null));
        }

        [Fact]
        public async Task TestStartSimulation_PostsExpectedContent()
        {
            var activityStatus = new BackgroundActivityStatus { ActivityId = "activityId" };
            var content = new StringContent(JsonConvert.SerializeObject(activityStatus));

            _mockedHttpClient.Setup(client =>
                client.PostAsync(new Uri(ServiceAddress, $"surveys/{_surveyId}/InterviewSimulations/StartInterviewSimulations"), It.IsAny<MultipartFormDataContent>()))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            _mockedHttpClient
                .Setup(client => client.GetAsync(It.IsAny<Uri>()))
                .Returns(Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new
                        {
                            ActivityId = "activityId",
                            Status = 2
                        }))
                    })).Verifiable();

            var result = await _target.StartSimulationAsync(_surveyId, new InterviewSimulation { InterviewsCount = 5, EnableReporting = true, UseOriginalSample = false });

            _mockedHttpClient.Verify(client => client.PostAsync(It.IsAny<Uri>(), It.Is<MultipartFormDataContent>(c =>
                c.IsMimeMultipartContent() &&
                c.Count() == 3 &&
                IsPropertyAvailable(c, new ContentProperty { Name = "InterviewsCount", NameValue = "5" }) &&
                IsPropertyAvailable(c, new ContentProperty { Name = "EnableReporting", NameValue = "true" }) &&
                IsPropertyAvailable(c, new ContentProperty { Name = "UseOriginalSample", NameValue = "false" })
                )));
        }

        [Fact]
        public async Task TestStartSimulation_PostsExpectedContentWithHintsAndSampleData()
        {
            var activityStatus = new BackgroundActivityStatus { ActivityId = "activityId" };
            var content = new StringContent(JsonConvert.SerializeObject(activityStatus));

            _mockedHttpClient.Setup(client =>
                client.PostAsync(new Uri(ServiceAddress, $"surveys/{_surveyId}/InterviewSimulations/StartInterviewSimulations"), It.IsAny<MultipartFormDataContent>()))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            _mockedHttpClient
                .Setup(client => client.GetAsync(It.IsAny<Uri>()))
                .Returns(Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new
                        {
                            ActivityId = "activityId",
                            Status = 2
                        }))
                    })).Verifiable();

            var result = await _target.StartSimulationAsync(_surveyId, new InterviewSimulation {
                InterviewsCount = 1,
                UseOriginalSample = true,
                HintsFileName = "hintsFileName", HintsFile = "hintsFileContent",
                SampleDataFileName = "sampleDataFileName", SampleDataFile = "sampleDataFileContent"
            });

            _mockedHttpClient.Verify(client => client.PostAsync(It.IsAny<Uri>(), It.Is<MultipartFormDataContent>(c =>
                c.IsMimeMultipartContent() &&
                c.Count() == 5 &&
                IsPropertyAvailable(c, new ContentProperty { Name = "InterviewsCount", NameValue = "1" }) &&
                IsPropertyAvailable(c, new ContentProperty { Name = "UseOriginalSample", NameValue = "true" }) &&
                IsFilePropertyAvailable(c, new ContentProperty { Name = "HintsFile", NameValue = "hintsFile", FileName = "hintsFileName", FileContent = "hintsFileContent" }) &&
                IsFilePropertyAvailable(c, new ContentProperty { Name = "SampleDataFile", NameValue = "sampleDataFile", FileName = "sampleDataFileName", FileContent = "sampleDataFileContent" })
                )));
        }

        [Fact]
        public async Task TestStartSimulation_SimulationSucceeded_ReturnsSimulationResult()
        {
            const int RequestedInterviewsCount = 5;
            const string OriginalSurveyId = nameof(OriginalSurveyId);
            const string SimulationSurveyId = nameof(SimulationSurveyId);
            var ErrorMessages = new List<string>() { "error message 1", "error message 2" };

            var activityStatus = new BackgroundActivityStatus { ActivityId = "activityId" };
            var content = new StringContent(JsonConvert.SerializeObject(activityStatus));

            _mockedHttpClient.Setup(client =>
                client.PostAsync(new Uri(ServiceAddress, $"surveys/{_surveyId}/InterviewSimulations/StartInterviewSimulations"), It.IsAny<MultipartFormDataContent>()))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            _mockedHttpClient
                .Setup(client => client.GetAsync(It.IsAny<Uri>()))
                .Returns(Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new
                        {
                            ActivityId = "activityId", Status = 2, RequestedInterviewsCount, OriginalSurveyId, SimulationSurveyId, ErrorMessages
                        }))
                    })).Verifiable();

            var result = await _target.StartSimulationAsync(_surveyId, new InterviewSimulation());

            Assert.Equal(RequestedInterviewsCount, result.RequestedInterviewsCount);
            Assert.Equal(OriginalSurveyId, result.OriginalSurveyId);
            Assert.Equal(SimulationSurveyId, result.SimulationSurveyId);
            Assert.Equal(2, result.ErrorMessages.Count());
        }

        #endregion

        private bool IsPropertyAvailable(MultipartFormDataContent content, ContentProperty contentProperty)
        {
            foreach (var contentItem in content)
            {
                var contentHeader = contentItem.Headers.ContentDisposition.Parameters;
                if (contentHeader.FirstOrDefault(c => c.Name == "name" && c.Value == contentProperty.Name) != null)
                {
                    var contentString = contentItem.ReadAsStringAsync().Result;
                    if (string.Compare(contentString, contentProperty.NameValue, StringComparison.OrdinalIgnoreCase) != 0)
                        return false;

                    return true;
                }
            }

            return false;
        }

        private bool IsFilePropertyAvailable(MultipartFormDataContent content, ContentProperty contentProperty)
        {
            foreach (var contentItem in content)
            {
                var contentHeader = contentItem.Headers.ContentDisposition.Parameters;
                if (contentHeader.FirstOrDefault(c => c.Name == "name" && c.Value == contentProperty.Name) != null &&
                    contentHeader.FirstOrDefault(c => c.Name == "filename" && c.Value == contentProperty.FileName) != null)
                {
                    var contentString = contentItem.ReadAsStringAsync().Result;
                    if (string.Compare(contentString, contentProperty.FileContent, StringComparison.OrdinalIgnoreCase) != 0)
                        return false;

                    return true;
                }
            }

            return false;
        }

        class ContentProperty
        {
            public string Name { get; set; }
            public string NameValue { get; set; }
            public string FileName {  get; set; }
            public string FileContent { get; set; }
        }
    }
}

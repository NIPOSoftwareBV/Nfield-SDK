﻿//    This file is part of Nfield.SDK.
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
using Nfield.SDK.Models;
using Nfield.Services.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
        private readonly Uri _getHintsEndpoint;
        private readonly Uri _startSimulationEndpoint;
        private readonly Mock<IFileSystem> _mockedFileSystem;

        public NfieldSurveyInterviewSimulationTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _mockedFileSystem = new Mock<IFileSystem>();

            _target = new NfieldSurveyInterviewSimulationService(_mockedFileSystem.Object);
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            _surveyId = Guid.NewGuid().ToString();
            _getHintsEndpoint = new Uri(ServiceAddress, $"Surveys/{_surveyId}/InterviewSimulations/DownloadHints");
            _startSimulationEndpoint = new Uri(ServiceAddress, $"Surveys/{_surveyId}/InterviewSimulations/StartInterviewSimulations");
        }

        [Fact]
        public async Task TestGetHintsAsync()
        {
            const string Hints = "hints data";
            var content = new StringContent(Hints);

            _mockedHttpClient
                .Setup(client => client.GetAsync(_getHintsEndpoint))
                .Returns(CreateTask(HttpStatusCode.OK, content))
                .Verifiable();

            var actual = await _target.GetHintsAsync(_surveyId);
            Assert.Equal(Hints, actual);
        }

        [Fact]
        public void TestGetInterviewSimulationsAsync_ServerReturnsQuery_ReturnsListWithSimulations()
        {
            var interviewSimulationsEndPoint = new Uri(ServiceAddress, "Surveys/InterviewSimulations");

            var expectedSimulationSurveys = new[]
            {
                new SurveyInterviewSimulation(SurveyType.Basic) { SurveyId = Guid.NewGuid().ToString() },
                new SurveyInterviewSimulation(SurveyType.Advanced) { SurveyId = Guid.NewGuid().ToString() }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(interviewSimulationsEndPoint))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSimulationSurveys))));

            var target = new NfieldSurveyInterviewSimulationService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSimulationSurveys = target.GetInterviewSimulationsAsync().Result;

            Assert.Equal(expectedSimulationSurveys[0].SurveyId, actualSimulationSurveys.ToArray()[0].SurveyId);
            Assert.Equal(expectedSimulationSurveys[1].SurveyId, actualSimulationSurveys.ToArray()[1].SurveyId);
            Assert.Equal(2, actualSimulationSurveys.Count());
        }

        [Fact]
        public void TestGetSurveyInterviewSimulationAsync_ServerReturnsQuery_ReturnsListWithSimulations()
        {
            var surveyId = Guid.NewGuid().ToString();
            var expectedSimulationSurvey = new SurveyInterviewSimulation(SurveyType.Basic) { SurveyId = surveyId };

            var surveyInterviewSimulationEndPoint = new Uri(ServiceAddress, $"Surveys/{surveyId}/InterviewSimulation");

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(surveyInterviewSimulationEndPoint))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSimulationSurvey))));

            var target = new NfieldSurveyInterviewSimulationService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSimulationSurvey = target.GetSurveyInterviewSimulationAsync(surveyId).Result;

            Assert.Equal(expectedSimulationSurvey.SurveyId, actualSimulationSurvey.SurveyId);
        }

        [Fact]
        public void TestGetSurveyInterviewSimulationAsync_SurveyIdIsNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.GetSurveyInterviewSimulationAsync(null));
        }

        [Fact]
        public void TestGetSurveyInterviewSimulationAsync_SurveyIdIsEmptyString_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.GetSurveyInterviewSimulationAsync(string.Empty));
        }

        #region StartSimulation

        [Fact]
        public void TestStartSimulation_SurveyIdIsNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.StartSimulationAsync(null, new InterviewSimulation()));
        }

        [Fact]
        public void TestStartSimulation_SurveyIdIsEmptyString_Throws()
        {
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _target.StartSimulationAsync(string.Empty, new InterviewSimulation()));
        }

        [Fact]
        public void TestStartSimulation_InterviewSimulationIsNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.StartSimulationAsync("surveyId", (InterviewSimulation)null));
        }

        [Fact]
        public async Task TestStartSimulation_PostsExpectedContent()
        {
            var activityStatus = new BackgroundActivityStatus { ActivityId = "activityId" };
            var content = new StringContent(JsonConvert.SerializeObject(activityStatus));

            _mockedHttpClient.Setup(client =>
                client.PostAsync(_startSimulationEndpoint, It.IsAny<MultipartFormDataContent>()))
                .Callback((Uri uri, HttpContent httpContent) =>
                {
                    var multipart = ((MultipartFormDataContent)httpContent);
                    Assert.Equal(3, multipart.Count());
                    Assert.True(IsPropertyAvailable(multipart, new ContentProperty { Name = "InterviewsCount", NameValue = "5" }));
                    Assert.True(IsPropertyAvailable(multipart, new ContentProperty { Name = "EnableReporting", NameValue = "true" }));
                    Assert.True(IsPropertyAvailable(multipart, new ContentProperty { Name = "UseOriginalSample", NameValue = "false" }));
                })
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

            _mockedHttpClient.Verify(client => client.PostAsync(It.IsAny<Uri>(), It.IsAny<MultipartFormDataContent>()), Times.Once()); 
        }

        [Fact]
        public async Task TestStartSimulation_PostsExpectedContentWithHintsAndSampleData()
        {
            var activityStatus = new BackgroundActivityStatus { ActivityId = "activityId" };
            var content = new StringContent(JsonConvert.SerializeObject(activityStatus));

            _mockedHttpClient.Setup(client =>
                client.PostAsync(_startSimulationEndpoint, It.IsAny<MultipartFormDataContent>()))
                .Callback((Uri uri, HttpContent httpContent) =>
                {
                    var multipart = ((MultipartFormDataContent)httpContent);
                    Assert.Equal(5, multipart.Count());
                    Assert.True(IsPropertyAvailable(multipart, new ContentProperty { Name = "InterviewsCount", NameValue = "1" }));
                    Assert.True(IsPropertyAvailable(multipart, new ContentProperty { Name = "UseOriginalSample", NameValue = "true" }));
                    Assert.True(IsPropertyAvailable(multipart, new ContentProperty { Name = "EnableReporting", NameValue = "false" }));
                    Assert.True(IsFilePropertyAvailable(multipart, new ContentProperty { Name = "HintsFile", NameValue = "hintsFile", FileName = "hintsFileName", FileContent = "hintsFileContent" }));
                    Assert.True(IsFilePropertyAvailable(multipart, new ContentProperty { Name = "SampleDataFile", NameValue = "sampleDataFile", FileName = "sampleDataFileName", FileContent = "sampleDataFileContent" }));
                })
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
                SampleDataFileName = "sampleDataFileName", SampleDataFile = Encoding.Default.GetBytes("sampleDataFileContent")
            });

            _mockedHttpClient.Verify(client => client.PostAsync(It.IsAny<Uri>(), It.IsAny<MultipartFormDataContent>()), Times.Once());
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
                client.PostAsync(_startSimulationEndpoint, It.IsAny<MultipartFormDataContent>()))
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
            Assert.Contains(ErrorMessages[0], result.ErrorMessages);
            Assert.Contains(ErrorMessages[1], result.ErrorMessages);
        }

        #endregion

        #region StartSimulation - get hints and sample data from file

        [Fact]
        public void TestStartSimulation_File_SurveyIdIsNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.StartSimulationAsync(null, new InterviewSimulationFiles()));
        }

        [Fact]
        public void TestStartSimulation_File_SurveyIdIsEmptyString_Throws()
        {
            Assert.ThrowsAsync<ArgumentException>(
                async () => await _target.StartSimulationAsync(string.Empty, new InterviewSimulationFiles()));
        }

        [Fact]
        public void TestStartSimulation_File_InterviewSimulationIsNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.StartSimulationAsync("surveyId", (InterviewSimulationFiles)null));
        }

        [Fact]
        public async Task TestStartSimulation_File_HintsFileDoesNotExist_Throws()
        {
            const string HintsFileName = nameof(HintsFileName);
            _mockedFileSystem.Setup(fs => fs.Path.GetFileName(It.IsAny<string>())).Returns(HintsFileName);
            _mockedFileSystem.Setup(fs => fs.File.Exists(It.IsAny<string>())).Returns(false);

            var exception = await Assert.ThrowsAsync<FileNotFoundException>(
                async () => await _target.StartSimulationAsync(_surveyId, new InterviewSimulationFiles { HintsFilePath = "hintsPath", SampleDataFilePath = "samplePath"}));

            Assert.Equal(HintsFileName, exception.Message);
        }

        [Fact]
        public async Task TestStartSimulation_File_SampleDataFileDoesNotExist_Throws()
        {
            const string HintsFilePath = nameof(HintsFilePath);
            const string SampleDataFileName = nameof(SampleDataFileName);
            const string SampleDataFilePath = nameof(SampleDataFilePath);

            _mockedFileSystem.Setup(fs => fs.Path.GetFileName(HintsFilePath)).Returns(string.Empty);
            _mockedFileSystem.Setup(fs => fs.File.Exists(HintsFilePath)).Returns(true);
            _mockedFileSystem.Setup(fs => fs.File.ReadAllText(It.IsAny<string>())).Returns(string.Empty);

            _mockedFileSystem.Setup(fs => fs.Path.GetFileName(SampleDataFilePath)).Returns(SampleDataFileName);
            _mockedFileSystem.Setup(fs => fs.File.Exists(SampleDataFilePath)).Returns(false);

            var exception = await Assert.ThrowsAsync<FileNotFoundException>(
                async () => await _target.StartSimulationAsync(_surveyId, new InterviewSimulationFiles { HintsFilePath = HintsFilePath, SampleDataFilePath = SampleDataFilePath }));

            Assert.Equal(SampleDataFileName, exception.Message);
        }

        [Fact]
        public async Task TestStartSimulation_File_HintsAndSampleDataFromFile_PostsExpectedData()
        {
            const string HintsFilePath = nameof(HintsFilePath);
            const string HintsFileName = nameof(HintsFileName);
            const string HintsData = "hints data";
            const string SampleDataFileName = nameof(SampleDataFileName);
            const string SampleDataFilePath = nameof(SampleDataFilePath);
            const string SampleData = "sample data";

            _mockedFileSystem.Setup(fs => fs.Path.GetFileName(HintsFilePath)).Returns(HintsFileName);
            _mockedFileSystem.Setup(fs => fs.File.Exists(HintsFilePath)).Returns(true);
            _mockedFileSystem.Setup(fs => fs.File.ReadAllText(HintsFilePath)).Returns(HintsData);

            _mockedFileSystem.Setup(fs => fs.Path.GetFileName(SampleDataFilePath)).Returns(SampleDataFileName);
            _mockedFileSystem.Setup(fs => fs.File.Exists(SampleDataFilePath)).Returns(true);
            _mockedFileSystem.Setup(fs => fs.File.ReadAllBytes(SampleDataFilePath)).Returns(Encoding.Default.GetBytes(SampleData));

            var activityStatus = new BackgroundActivityStatus { ActivityId = "activityId" };
            var content = new StringContent(JsonConvert.SerializeObject(activityStatus));

            _mockedHttpClient.Setup(client =>
                client.PostAsync(_startSimulationEndpoint, It.IsAny<MultipartFormDataContent>()))
                .Callback((Uri uri, HttpContent httpContent) =>
                {
                    var multipart = ((MultipartFormDataContent)httpContent);
                    Assert.Equal(5, multipart.Count());
                    Assert.True(IsPropertyAvailable(multipart, new ContentProperty { Name = "InterviewsCount", NameValue = "1" }));
                    Assert.True(IsPropertyAvailable(multipart, new ContentProperty { Name = "UseOriginalSample", NameValue = "true" }));
                    Assert.True(IsPropertyAvailable(multipart, new ContentProperty { Name = "EnableReporting", NameValue = "true" }));
                    Assert.True(IsFilePropertyAvailable(multipart, new ContentProperty { Name = "HintsFile", NameValue = "hintsFile", FileName = HintsFileName, FileContent = HintsData }));
                    Assert.True(IsFilePropertyAvailable(multipart, new ContentProperty { Name = "SampleDataFile", NameValue = "sampleDataFile", FileName = SampleDataFileName, FileContent = SampleData }));
                })
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

            var result = await _target.StartSimulationAsync(_surveyId, new InterviewSimulationFiles
            {
                InterviewsCount = 1,
                UseOriginalSample = true,
                EnableReporting = true,
                HintsFilePath = HintsFilePath,
                SampleDataFilePath = SampleDataFilePath
            });

            _mockedHttpClient.Verify(client => client.PostAsync(It.IsAny<Uri>(), It.IsAny<MultipartFormDataContent>()), Times.Once());
        }

        #endregion

        private static bool IsPropertyAvailable(MultipartFormDataContent content, ContentProperty contentProperty)
        {
            foreach (var contentItem in content)
            {
                var contentHeader = contentItem.Headers.ContentDisposition.Parameters;
                if (contentHeader.FirstOrDefault(c => c.Name == "name" && c.Value == contentProperty.Name) != null)
                {
                    var contentString = contentItem.ReadAsStringAsync().Result;
                    return string.Compare(contentString, contentProperty.NameValue, StringComparison.OrdinalIgnoreCase) == 0;
                }
            }

            return false;
        }

        private static bool IsFilePropertyAvailable(MultipartFormDataContent content, ContentProperty contentProperty)
        {
            foreach (var contentItem in content)
            {
                var contentHeader = contentItem.Headers.ContentDisposition.Parameters;
                if (contentHeader.FirstOrDefault(c => c.Name == "name" && c.Value == contentProperty.Name) != null &&
                    contentHeader.FirstOrDefault(c => c.Name == "filename" && c.Value == contentProperty.FileName) != null)
                {
                    if (contentProperty.Name == "HintsFile" && contentItem.Headers.ContentType.MediaType != "text/plain")
                    {
                        return false;
                    }

                    if (contentProperty.Name == "SampleDataFile" && contentItem.Headers.ContentType.MediaType != "application/octet-stream")
                    {
                        return false;
                    }

                    var contentString = contentItem.ReadAsStringAsync().Result;
                    return string.Compare(contentString, contentProperty.FileContent, StringComparison.OrdinalIgnoreCase) == 0;
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

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
using Nfield.Services.Implementation;
using System;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;

namespace Nfield.Services
{
    public class NfieldSurveyManualTestsServiceTests : NfieldServiceTestsBase
    {
        private readonly NfieldSurveyManualTestsService _target;
        private readonly Mock<INfieldHttpClient> _mockedHttpClient;
        private readonly string _surveyId;
        private readonly Mock<IFileSystem> _mockedFileSystem;

        public NfieldSurveyManualTestsServiceTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _mockedFileSystem = new Mock<IFileSystem>();

            _target = new NfieldSurveyManualTestsService(_mockedFileSystem.Object);
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            _surveyId = Guid.NewGuid().ToString();
        }

        [Fact]
        public void TestGetSurveyManualTestsAsync_SurveyIdIsNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.GetSurveyManualTestsAsync(null));
        }

        [Fact]
        public void TestGetSurveyManualTestAsync_SurveyIdIsNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.GetSurveyManualTestAsync(null, "manualTestSurveyId"));
        }

        [Fact]
        public void TestGetSurveyManualTestAsync_ManualTestSurveyIdIsNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.GetSurveyManualTestAsync("surveyId", null));
        }

        [Fact]
        public void TestGetSurveyManualTestAsync_ServerReturnsQuery_ReturnsTestSurvey()
        {
            var manualTestSurveyId = Guid.NewGuid().ToString();
            var endPoint = new Uri(ServiceAddress, $"Surveys/{_surveyId}/ManualTests/{manualTestSurveyId}");
            var expectedTestSurvey = new SurveyManualTest(SurveyType.Basic) { SurveyId = manualTestSurveyId };

            _mockedHttpClient
                .Setup(client => client.GetAsync(endPoint))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedTestSurvey))));

            var actualTestSurvey = _target.GetSurveyManualTestAsync(_surveyId, manualTestSurveyId).Result;

            Assert.Equal(expectedTestSurvey.SurveyId, actualTestSurvey.SurveyId);
        }

        [Fact]
        public void TestStartCreateManualTestSurveyAsync_SurveyIdIsNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.StartCreateManualTestSurveyAsync(null, new StartCreateManualTestSurvey()));
        }

        [Fact]
        public void TestStartCreateManualTestSurveyAsync_RequestIsNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.StartCreateManualTestSurveyAsync("surveyId", (StartCreateManualTestSurvey)null));
        }

        [Fact]
        public async Task TestStartCreateManualTestSurveyAsync_PostsExpectedContent()
        {
            var activityStatus = new BackgroundActivityStatus { ActivityId = "activityId" };
            var content = new StringContent(JsonConvert.SerializeObject(activityStatus));
            var endPoint = new Uri(ServiceAddress, $"Surveys/{_surveyId}/ManualTests");

            _mockedHttpClient.Setup(client =>
                client.PostAsync(endPoint, It.IsAny<MultipartFormDataContent>()))
                .Callback((Uri uri, HttpContent httpContent) =>
                {
                    var multipart = ((MultipartFormDataContent)httpContent);
                    Assert.Equal(2, multipart.Count());
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

            var result = await _target.StartCreateManualTestSurveyAsync(_surveyId, new StartCreateManualTestSurvey { EnableReporting = true, UseOriginalSample = false });

            _mockedHttpClient.Verify(client => client.PostAsync(It.IsAny<Uri>(), It.IsAny<MultipartFormDataContent>()), Times.Once());
        }

        [Fact]
        public void TestStartCreateManualTestSurveyAsync_File_SurveyIdIsNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.StartCreateManualTestSurveyAsync(null, new StartCreateManualTestSurveyFile()));
        }

        [Fact]
        public void TestStartCreateManualTestSurveyAsync_File_RequestIsNull_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _target.StartCreateManualTestSurveyAsync("surveyId", (StartCreateManualTestSurveyFile)null));
        }

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
            public string FileName { get; set; }
            public string FileContent { get; set; }
        }
    }
}

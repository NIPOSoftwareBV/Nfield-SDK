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
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    public class NfieldSurveyLandingPageServiceTests : NfieldServiceTestsBase
    {
        #region UploadLandingPageAsync from file path

        [Fact]
        public void UploadLandingPageAsync_FileDoesNotExist_ThrowsFileNotFoundException()
        {
            var target = new NfieldSurveyLandingPageService();
            target.InitializeNfieldConnection(new Mock<INfieldConnectionClient>().Object);

            var ex = Assert.ThrowsAsync<FileNotFoundException>(() =>
                target.UploadLandingPageAsync("surveyId", "nonexistent.zip"));

            Assert.Contains("does not exist", ex.Result.Message);
        }

        [Fact]
        public void UploadLandingPageAsync_FilePathIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveyLandingPageService();
            target.InitializeNfieldConnection(new Mock<INfieldConnectionClient>().Object);

            Assert.ThrowsAsync<ArgumentNullException>(() =>
                target.UploadLandingPageAsync("surveyId", null));
        }

        #endregion

        #region UploadLandingPageAsync with stream

        [Fact]
        public void UploadLandingPageAsync_StreamIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveyLandingPageService();
            target.InitializeNfieldConnection(new Mock<INfieldConnectionClient>().Object);

            Assert.ThrowsAsync<ArgumentNullException>(() =>
                target.UploadLandingPageAsync("surveyId", "landingPage.zip", null));
        }

        [Fact]
        public async Task UploadLandingPageAsync_DoesNotThrow()
        {
            const string surveyId = "SurveyId";
            const string fileName = "MyLandingPage.zip";
            byte[] content = new byte[] { 1, 2, 3, 4, 5 };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var response = new StringContent(JsonConvert.SerializeObject(new { ActivityId = "activityId" }));

            mockedHttpClient
                .Setup(client => client.PostAsync(new Uri(ServiceAddress, $"Surveys/{surveyId}/landingPage/"),
                        It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.OK, response));

            mockedHttpClient
                .Setup(client => client.GetAsync(It.IsAny<Uri>()))
                .Returns(Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new
                        {
                            ActivityId = "activityId",
                            Status = 2 // Succeeded
                        }))
                    }))
                .Verifiable();

            var target = new NfieldSurveyLandingPageService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var stream = new MemoryStream(content);
            await target.UploadLandingPageAsync(surveyId, fileName, stream);
        }

        #endregion
    }
}
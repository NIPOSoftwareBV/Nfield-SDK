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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyScriptService"/>
    /// </summary>
    public class NfieldSurveyDataServiceTests : NfieldServiceTestsBase
    {
        private readonly NfieldSurveyDataService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;
        readonly Mock<INfieldConnectionClient> _mockedNfieldConnection;

        public NfieldSurveyDataServiceTests()
        {
            _mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(_mockedNfieldConnection);

            _target = new NfieldSurveyDataService();
            _target.InitializeNfieldConnection(_mockedNfieldConnection.Object);

        }

        #region PrepareDownload

        [Fact]
        public void TestPrepareDownload_WhenSurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.PrepareDownload(null, It.IsAny<SurveyDataRequest>())));
        }

        [Fact]
        public void TestPrepareDownload_WhenSurveyIdIsEmptyString_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.PrepareDownload(string.Empty, It.IsAny<SurveyDataRequest>())));
        }

        [Fact]
        public void TestPrepareDownload_WhenSurveyIdIsWhiteSpace_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.PrepareDownload(" ", It.IsAny<SurveyDataRequest>())));
        }

        [Fact]
        public void TestPrepareDownload_CallsCorrectURI()
        {
            const string surveyId = "SurveyId";

            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(It.IsAny<Uri>(), It.IsAny<SurveyDataRequest>()))
                .Returns(Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { ActivityId = "activityId" }))
                    })).Verifiable();

            _mockedHttpClient
                .Setup(client => client.GetAsync(It.IsAny<Uri>()))
                .Returns(Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { DownloadDataUrl = "link", ActivityId = "activityId", Status = 2 /* Succeeded */ }))
                    })).Verifiable();

            _target.PrepareDownload(surveyId, new SurveyDataRequest()).Wait();

            _mockedHttpClient
                .Verify(
                    client =>
                        client.PostAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{surveyId}/DataDownload"), It.IsAny<SurveyDataRequest>()),
                    Times.Once());
        }

        #endregion

        #region PrepareInterviewDownload

        [Fact]
        public void TestPrepareInterviewDownload_WhenSurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.PrepareInterviewDownload(null, 1, It.IsAny<SurveyDataInterviewRequest>())));
        }

        [Fact]
        public void TestPrepareInterviewDownload_WhenSurveyIdIsEmptyString_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.PrepareInterviewDownload(string.Empty, 1, It.IsAny<SurveyDataInterviewRequest>())));
        }

        [Fact]
        public void TestPrepareInterviewDownload_WhenSurveyIdIsWhiteSpace_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.PrepareInterviewDownload(" ", 1, It.IsAny<SurveyDataInterviewRequest>())));
        }

        [Fact]
        public void TestPrepareInterviewDownload_CallsCorrectURI()
        {
            const string surveyId = "SurveyId";
            const int interviewId = 1;

            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(It.IsAny<Uri>(), It.IsAny<SurveyDataInterviewRequest>()))
                .Returns(Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { ActivityId = "activityId" }))
                    })).Verifiable();

            _mockedHttpClient
                .Setup(client => client.GetAsync(It.IsAny<Uri>()))
                .Returns(Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { DownloadDataUrl = "link", ActivityId = "activityId", Status = 2 /* Succeeded */ }))
                    })).Verifiable();

            _target.PrepareInterviewDownload(surveyId, interviewId, new SurveyDataInterviewRequest()).Wait();

            _mockedHttpClient
                .Verify(
                    client =>
                        client.PostAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{surveyId}/DataDownload/{interviewId}"), It.IsAny<SurveyDataInterviewRequest>()),
                    Times.Once());
        }

        #endregion
    }
}

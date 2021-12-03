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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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
    public class NfieldSurveyFieldworkServiceTests : NfieldServiceTestsBase
    {
        private readonly NfieldSurveyFieldworkService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;
        readonly Mock<INfieldConnectionClient> _mockedNfieldConnection;

        public NfieldSurveyFieldworkServiceTests()
        {
            _mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(_mockedNfieldConnection);

            _target = new NfieldSurveyFieldworkService();
            _target.InitializeNfieldConnection(_mockedNfieldConnection.Object);

        }

        #region GetStatusAsync

        [Fact]
        public void TestGetStatusAsync_WhenSurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.GetStatusAsync(null)));
        }

        [Fact]
        public void TestGetStatusAsync_WhenSurveyIdIsEmptyString_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.GetStatusAsync(string.Empty)));
        }

        [Fact]
        public void TestGetStatusAsync_WhenSurveyIdIsWhiteSpace_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.GetStatusAsync(" ")));
        }

        [Fact]
        public void TestGetStatusAsync_Always_CallsCorrectURI()
        {
            const string surveyId = "SurveyId";

            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/" + surveyId + "/Fieldwork/Status")))
                .Returns(CreateTask(HttpStatusCode.OK, new ObjectContent<int>(0, new JsonMediaTypeFormatter())));

            var actual = _target.GetStatusAsync(surveyId).Result;

            Assert.Equal(0, (int)actual);
        }

        #endregion

        #region StartFieldworkAsync

        [Fact]
        public void TestStartFieldworkAsync_WhenSurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.StartFieldworkAsync(null)));
        }

        [Fact]
        public void TestStartFieldworkAsync_WhenSurveyIdIsEmptyString_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.StartFieldworkAsync(string.Empty)));
        }

        [Fact]
        public void TestStartFieldworkAsync_WhenSurveyIdIsWhiteSpace_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.StartFieldworkAsync(" ")));
        }

        [Fact]
        public void TestStartFieldworkAsync_Always_CallsCorrectURI()
        {
            const string surveyId = "SurveyId";
            _mockedHttpClient.Setup(c => c.PutAsync(It.IsAny<Uri>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(string.Empty)));

            _target.StartFieldworkAsync(surveyId);

            _mockedHttpClient
                .Verify(client => client.PutAsync(new Uri(ServiceAddress, "Surveys/" + surveyId + "/Fieldwork/Start"), It.IsAny<HttpContent>()), Times.Once());
        }

        #endregion

        #region StopFieldworkAsync

        [Fact]
        public void TestStopFieldworkAsync_WhenSurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.StopFieldworkAsync(null, new StopFieldworkModel())));
        }

        [Fact]
        public void TestStopFieldworkAsync_WhenSurveyIdIsEmptyString_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.StopFieldworkAsync(string.Empty, new StopFieldworkModel())));
        }

        [Fact]
        public void TestStopFieldworkAsync_WhenSurveyIdIsWhiteSpace_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.StopFieldworkAsync(" ", new StopFieldworkModel())));
        }

        [Fact]
        public void TestStopFieldworkAsync_Always_CallsCorrectURI()
        {
            const string surveyId = "SurveyId";
            var model = new StopFieldworkModel();

            _mockedHttpClient.Setup(c => c.PutAsJsonAsync(It.IsAny<Uri>(), model))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(string.Empty)));

            _target.StopFieldworkAsync(surveyId, model);

            _mockedHttpClient
                .Verify(client => client.PutAsJsonAsync(new Uri(ServiceAddress, "Surveys/" + surveyId + "/Fieldwork/Stop"), model), Times.Once());
        }

        #endregion

        #region FinishFieldworkAsync

        [Fact]
        public void TestFinishFieldworkAsync_WhenSurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.FinishFieldworkAsync(null)));
        }

        [Fact]
        public void TestFinishFieldworkAsync_WhenSurveyIdIsEmptyString_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.FinishFieldworkAsync(string.Empty)));
        }

        [Fact]
        public void TestFinishFieldworkAsync_WhenSurveyIdIsWhiteSpace_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.FinishFieldworkAsync(" ")));
        }

        [Fact]
        public void TestFinishFieldworkAsync_Always_CallsCorrectURI()
        {
            const string surveyId = "SurveyId";
            _mockedHttpClient.Setup(c => c.PutAsync(It.IsAny<Uri>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(string.Empty)));

            _target.FinishFieldworkAsync(surveyId);

            _mockedHttpClient
                .Verify(client => client.PutAsync(new Uri(ServiceAddress, "Surveys/" + surveyId + "/Fieldwork/Finish"), It.IsAny<HttpContent>()), Times.Once());
        }
        #endregion

        #region GetCountsAsync

        [Fact]
        public async void TestGetCountsAsync_ReturnRightValues()
        {
            // Setup
            string surveyId = Guid.NewGuid().ToString();
            var random = new Random();
            var expectedResult = new SurveyFieldworkCounts
            {
                ActiveInterviews = random.Next(20),
                DroppedOut = random.Next(20),
                DroppedOutDeleted = random.Next(20),
                SurveyId = surveyId,
                Rejected = random.Next(20),
                RejectedDeleted = random.Next(20),
                ScreenedOut = random.Next(20),
                ScreenedOutDeleted = random.Next(20),
                ScreenedOutOverview = new[] { new SurveyFieldworkCounts.ResponseCodeCount { ResponseCode = 201 + random.Next(20), Count = random.Next(20) } },
                Successful = random.Next(20),
                SuccessfulDeleted = random.Next(20),
                SuccessfulLast24Hours = random.Next(20)
            };
            _mockedHttpClient.Setup(c => c.GetAsync(It.Is<Uri>(u=>u.ToString().EndsWith("Surveys/" + surveyId + "/Fieldwork/Counts"))))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedResult)))).Verifiable();

            // Act
            var result = await  _target.GetCountsAsync(surveyId);

            // Asserts
            _mockedHttpClient.Verify();
            Assert.Equal(expectedResult.ActiveInterviews, result.ActiveInterviews);
            Assert.Equal(expectedResult.DroppedOut, result.DroppedOut);
            Assert.Equal(expectedResult.DroppedOutDeleted, result.DroppedOutDeleted);
            Assert.Equal(expectedResult.SurveyId, result.SurveyId);
            Assert.Equal(expectedResult.Rejected, result.Rejected);
            Assert.Equal(expectedResult.RejectedDeleted, result.RejectedDeleted);
            Assert.Equal(expectedResult.ScreenedOut, result.ScreenedOut);
            Assert.Equal(expectedResult.ScreenedOutDeleted, result.ScreenedOutDeleted);
            Assert.Equal(expectedResult.ScreenedOutOverview.Count(), result.ScreenedOutOverview.Count());
            Assert.Equal(expectedResult.ScreenedOutOverview.First().Count, result.ScreenedOutOverview.First().Count);
            Assert.Equal(expectedResult.ScreenedOutOverview.First().ResponseCode, result.ScreenedOutOverview.First().ResponseCode);
            Assert.Equal(expectedResult.Successful, result.Successful);
            Assert.Equal(expectedResult.SuccessfulDeleted, result.SuccessfulDeleted);
            Assert.Equal(expectedResult.SuccessfulLast24Hours, result.SuccessfulLast24Hours);
        }

        [Fact]
        public void TestGetCountsAsync_WhenSurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.GetCountsAsync(null)));
        }

        [Fact]
        public void TestGetCountsAsync_WhenSurveyIdIsEmptyString_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.FinishFieldworkAsync(string.Empty)));
        }

        #endregion
    }
}

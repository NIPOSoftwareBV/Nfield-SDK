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
using System.Net.Http.Formatting;
using Moq;
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
                .Setup(client => client.GetAsync(ServiceAddress + "surveys/" + surveyId + "/Fieldwork/Status"))
                .Returns(CreateTask(HttpStatusCode.OK, new ObjectContent<int>(0, new JsonMediaTypeFormatter())));

            var actual = _target.GetStatusAsync(surveyId).Result;

            Assert.Equal(0, (int) actual);
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
            _mockedHttpClient.Setup(c => c.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(string.Empty)));

            _target.StartFieldworkAsync(surveyId);

            _mockedHttpClient
                .Verify(client => client.PutAsync(ServiceAddress + "surveys/" + surveyId + "/Fieldwork/Start", It.IsAny<HttpContent>()), Times.Once());
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

            _mockedHttpClient.Setup(c => c.PutAsJsonAsync(It.IsAny<string>(), model))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(string.Empty)));

            _target.StopFieldworkAsync(surveyId, model);

            _mockedHttpClient
                .Verify(client => client.PutAsJsonAsync(ServiceAddress + "surveys/" + surveyId + "/Fieldwork/Stop", model), Times.Once());
        }

        #endregion

    }
}

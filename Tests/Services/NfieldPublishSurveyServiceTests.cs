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
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyScriptService"/>
    /// </summary>
    public class NfieldPublishSurveyServiceTests : NfieldServiceTestsBase
    {
        private readonly NfieldPublishSurveyService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;
        readonly Mock<INfieldConnectionClient> _mockedNfieldConnection;

        public NfieldPublishSurveyServiceTests()
        {
            _mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(_mockedNfieldConnection);

            _target = new NfieldPublishSurveyService();
            _target.InitializeNfieldConnection(_mockedNfieldConnection.Object);

        }

        #region GetStatusAsync

        [Fact]
        public void TestGetAsync_WhenSurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.GetAsync(null)));
        }

        [Fact]
        public void TestGetAsync_WhenSurveyIdIsEmptyString_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.GetAsync(string.Empty)));
        }

        [Fact]
        public void TestGetAsync_WhenSurveyIdIsWhiteSpace_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.GetAsync(" ")));
        }

        [Fact]
        public void TestGetAsync_Always_CallsCorrectURI()
        {
            const string surveyId = "SurveyId";
            
            _mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "Surveys/" + surveyId + "/Publish"))
                .Returns(CreateTask(HttpStatusCode.OK, new ObjectContent<int>(0, new JsonMediaTypeFormatter())));

            var actual = _target.GetAsync(surveyId).Result;

            Assert.Equal(0, (int) actual);
        }

        #endregion

        #region StartFieldworkAsync

        [Fact]
        public void TestPutAsync_WhenSurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.PutAsync(null)));
        }

        [Fact]
        public void TestPutAsync_WhenSurveyIdIsEmptyString_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.PutAsync(string.Empty)));
        }

        [Fact]
        public void TestPutAsync_WhenSurveyIdIsWhiteSpace_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.PutAsync(" ")));
        }

        [Fact]
        public void TestPutAsync_Always_CallsCorrectURI()
        {
            const string surveyId = "SurveyId";
            _mockedHttpClient.Setup(c => c.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(string.Empty)));

            _target.PutAsync(surveyId);

            _mockedHttpClient
                .Verify(client => client.PutAsync(ServiceAddress + "Surveys/" + surveyId + "/Publish", It.IsAny<HttpContent>()), Times.Once());
        }

        #endregion

       
    }
}

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
    public class NfieldSurveyPublishServiceTests : NfieldServiceTestsBase
    {
        private readonly NfieldSurveyPublishService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;
        readonly Mock<INfieldConnectionClient> _mockedNfieldConnection;

        public NfieldSurveyPublishServiceTests()
        {
            _mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(_mockedNfieldConnection);

            _target = new NfieldSurveyPublishService();
            _target.InitializeNfieldConnection(_mockedNfieldConnection.Object);

        }

        #region GetAsync

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
            var testModel = new SurveyPackageStateModel() { Live = (PackagePublishState)1, Test = (PackagePublishState)2 };
            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/" + surveyId + "/Publish")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(testModel))));

            var actual = _target.GetAsync(surveyId).Result;

            Assert.Equal((PackagePublishState)1, actual.Live);
            Assert.Equal((PackagePublishState)2, actual.Test);
        }

        #endregion

        #region PutAsync

        [Fact]
        public void TestPutAsync_WhenSurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.PutAsync(null, It.IsAny<SurveyPublishTypeUpgradeModel>())));
        }

        [Fact]
        public void TestPutAsync_WhenSurveyIdIsEmptyString_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.PutAsync(string.Empty, It.IsAny<SurveyPublishTypeUpgradeModel>())));
        }

        [Fact]
        public void TestPutAsync_WhenSurveyIdIsWhiteSpace_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.PutAsync(" ", It.IsAny<SurveyPublishTypeUpgradeModel>())));
        }

        [Fact]
        public void TestPutAsync_Always_CallsCorrectURI()
        {
            const string surveyId = "SurveyId";
            _mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(It.IsAny<Uri>(), It.IsAny<SurveyPublishTypeUpgradeModel>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(It.IsAny<SurveyPublishTypeUpgradeModel>()))));

            _target.PutAsync(surveyId, new SurveyPublishTypeUpgradeModel()).Wait();

            _mockedHttpClient
                .Verify(
                    client =>
                        client.PutAsJsonAsync(new Uri(ServiceAddress, "Surveys/" + surveyId + "/Publish"), It.IsAny<SurveyPublishTypeUpgradeModel>()),
                    Times.Once());
        }

        #endregion


    }
}

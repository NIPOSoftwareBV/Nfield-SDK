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
using Nfield.Infrastructure;
using Nfield.Services.Implementation;
using Xunit;
using Nfield.Models;
using Moq;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyPackageService"/>
    /// </summary>
    public class NfieldSurveyPackageServiceTests : NfieldServiceTestsBase
    {
        private const string SurveyId = "SurveyId";

        #region GetSurveyPackageAsync

        [Fact]
        public void TestGetSurveyPackageAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyPackageService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.GetSurveyPackageAsync(null, InterviewPackageType.Test)));
        }

        [Fact]
        public void TestGetSurveyPackageAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveyPackageService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.GetSurveyPackageAsync("", InterviewPackageType.Test)));
        }

        [Fact]
        public void TestGetSurveyPackageAsync_PackageTypeIsUnknown_Throws()
        {
            var target = new NfieldSurveyPackageService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.GetSurveyPackageAsync("SurveyId", InterviewPackageType.Unknown)));
        }

        [Fact]
        public void TestGetSurveyPackageAsync_ReturnsSurveyPackage()
        {
            var expectedPackage = new SurveyPackage()
            {
                SurveyName = "SurveyName"
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "Surveys/" + SurveyId + "/Package/?type=" + InterviewPackageType.Live))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedPackage))));

            var target = new NfieldSurveyPackageService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.GetSurveyPackageAsync(SurveyId, InterviewPackageType.Live).Result;

            Assert.Equal(expectedPackage.SurveyName, actual.SurveyName);
        }

        #endregion

    }
}

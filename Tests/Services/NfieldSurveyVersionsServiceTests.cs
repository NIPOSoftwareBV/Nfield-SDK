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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyVersionsService"/>
    /// </summary>
    public class NfieldSurveyVersionsServiceTests : NfieldServiceTestsBase
    {
        private const string SurveyId = "SurveyId";

        #region GetSurveyVersionsAsync

        [Fact]
        public void TestGetSurveyVersionsAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyVersionsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.GetSurveyVersionsAsync(null)));
        }

        [Fact]
        public void TestGetSurveyVersionsAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveyVersionsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.GetSurveyVersionsAsync(string.Empty)));
        }

        [Fact]
        public void TestGetSurveyVersionsAsync_ReturnsSurveyVersions()
        {
            var expectedVersions = new List<SurveyVersion>
            {
                new SurveyVersion()
                {
                    Etag = "637235755520645294"
                },
                 new SurveyVersion()
                {
                    Etag = "637244319282906584"
                }
            }.ToArray();

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/" + SurveyId + "/Versions")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedVersions))));

            var target = new NfieldSurveyVersionsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.GetSurveyVersionsAsync(SurveyId).Result.ToArray();

            Assert.Equal(expectedVersions[0].Etag, actual[0].Etag);
            Assert.Equal(expectedVersions[1].Etag, actual[1].Etag);
        }

        #endregion
    }
}

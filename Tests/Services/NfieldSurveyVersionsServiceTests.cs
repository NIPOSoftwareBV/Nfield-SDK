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
            var date0 = DateTime.UtcNow;
            var date1 = date0.AddDays(1); ;
            var expectedVersions = new List<SurveyVersion>
            {
                new SurveyVersion
                {
                    Etag = date0.Ticks.ToString(),
                    PublishDateUtc = date0,
                    NrOfSuccessfuls = 1,
                    NrOfDroppedOuts = 2,
                    NrOfScreenedOuts = 3
                },
                new SurveyVersion
                {
                    Etag = date1.Ticks.ToString(),
                    PublishDateUtc = date1,
                    NrOfSuccessfuls = 4,
                    NrOfDroppedOuts = 5,
                    NrOfScreenedOuts = 6
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
            Assert.Equal(expectedVersions[0].PublishDateUtc, actual[0].PublishDateUtc);
            Assert.Equal(expectedVersions[0].NrOfSuccessfuls, actual[0].NrOfSuccessfuls);
            Assert.Equal(expectedVersions[0].NrOfDroppedOuts, actual[0].NrOfDroppedOuts);
            Assert.Equal(expectedVersions[0].NrOfScreenedOuts, actual[0].NrOfScreenedOuts);
            Assert.Equal(expectedVersions[1].Etag, actual[1].Etag);
            Assert.Equal(expectedVersions[1].PublishDateUtc, actual[1].PublishDateUtc);
            Assert.Equal(expectedVersions[1].NrOfSuccessfuls, actual[1].NrOfSuccessfuls);
            Assert.Equal(expectedVersions[1].NrOfDroppedOuts, actual[1].NrOfDroppedOuts);
            Assert.Equal(expectedVersions[1].NrOfScreenedOuts, actual[1].NrOfScreenedOuts);
        }

        #endregion
    }
}

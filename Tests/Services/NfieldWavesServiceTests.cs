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
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Nfield.SDK.Models;
using System.Linq;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldWavesService"/>
    /// </summary>
    public class NfieldWavesServiceTests : NfieldServiceTestsBase
    {
        [Fact]
        public void TestWavesAsync_ServerReturnsQuery_ReturnsListWithWaves()
        {
            const string parentSurveyId = "parentSurveyId";
            var expectedWavesSurveys = new[]
            {
                new Survey(SurveyType.OnlineBasic) { SurveyId = Guid.NewGuid().ToString() },
                new Survey(SurveyType.OnlineBasic) { SurveyId = Guid.NewGuid().ToString(), },
            };

            var getWavesEndPoint = new Uri(ServiceAddress, $"Surveys/{parentSurveyId}/Waves/");

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(getWavesEndPoint))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedWavesSurveys))));

            var target = new NfieldWavesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualWaves = target.GetParentSurveyWavesAsync(parentSurveyId).Result;

            Assert.Equal(expectedWavesSurveys[0].SurveyId, actualWaves.ToArray()[0].SurveyId);
            Assert.Equal(expectedWavesSurveys[1].SurveyId, actualWaves.ToArray()[1].SurveyId);
            Assert.Equal(2, actualWaves.Count());
        }

        [Fact]
        public void TestWaveAsync_ReturnsSurvey()
        {
            const string parentSurveyId = "parentSurveyId";
            var survey = new Survey(SurveyType.OnlineBasic) { SurveyId = Guid.NewGuid().ToString() };

            var getWavesEndPoint = new Uri(ServiceAddress, $"Surveys/{parentSurveyId}/Waves/");

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(getWavesEndPoint, survey))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(survey))));

            var target = new NfieldWavesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSurvey = target.AddWaveAsync(parentSurveyId, survey).Result;

            Assert.Equal(survey.SurveyId, actualSurvey.SurveyId);
        }
    }
}

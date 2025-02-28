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
    /// Tests for <see cref="NfieldParentSurveyService"/>
    /// </summary>
    public class NfieldParentSurveyServiceTests : NfieldServiceTestsBase
    {
        [Fact]
        public void TestGetParentSurveysAsync_ServerReturnsQuery_ReturnsListWithParentSurveys()
        {

            var expectedParentSurveys = new[]
            {
                new Survey(SurveyType.OnlineBasic) { SurveyId = Guid.NewGuid().ToString() },
                new Survey(SurveyType.OnlineBasic) { SurveyId = Guid.NewGuid().ToString() },
            };

            var getParentSurveysEndPoint = new Uri(ServiceAddress, $"ParentSurveys");

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(getParentSurveysEndPoint))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedParentSurveys))));

            var target = new NfieldParentSurveyService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualParentSurveys = target.GetParentSurveysAsync().Result;

            Assert.Equal(expectedParentSurveys[0].SurveyId, actualParentSurveys.ToArray()[0].SurveyId);
            Assert.Equal(expectedParentSurveys[1].SurveyId, actualParentSurveys.ToArray()[1].SurveyId);
            Assert.Equal(2, actualParentSurveys.Count());
        }

        [Fact]
        public void TestCreateParentSurveyAsync_ReturnsSurvey()
        {
            var createSurvey = new ParentSurvey();
            var expectedSurvey = new Survey(SurveyType.OnlineBasic) { SurveyId = Guid.NewGuid().ToString() };

            var createParentSurveyEndPoint = new Uri(ServiceAddress, $"ParentSurveys");

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(createParentSurveyEndPoint, createSurvey))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSurvey))));

            var target = new NfieldParentSurveyService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSurvey = target.AddParentSurveyAsync(createSurvey).Result;

            Assert.Equal(expectedSurvey.SurveyId, actualSurvey.SurveyId);
        }


    }
}

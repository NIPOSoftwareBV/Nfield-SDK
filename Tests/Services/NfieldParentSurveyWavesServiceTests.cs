﻿//    This file is part of Nfield.SDK.
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
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldParentSurveyWavesService"/>
    /// </summary>
    public class NfieldParentSurveyWavesServiceTests : NfieldServiceTestsBase
    {
        [Fact]
        public void TestWavesAsync_ServerReturnsQuery_ReturnsListWithWaves()
        {
            string parentSurveyId = Guid.NewGuid().ToString();
            var expectedWavesSurveys = new[]
            {
                new Survey(SurveyType.OnlineBasic) { SurveyId = Guid.NewGuid().ToString() },
                new Survey(SurveyType.OnlineBasic) { SurveyId = Guid.NewGuid().ToString(), },
            };

            var getWavesEndPoint = new Uri(ServiceAddress, $"ParentSurveys/{parentSurveyId}/Waves/");

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(getWavesEndPoint))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedWavesSurveys))));

            var target = new NfieldParentSurveyWavesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualWaves = target.GetParentSurveyWavesAsync(parentSurveyId).Result;

            Assert.Equal(expectedWavesSurveys[0].SurveyId, actualWaves.ToArray()[0].SurveyId);
            Assert.Equal(expectedWavesSurveys[1].SurveyId, actualWaves.ToArray()[1].SurveyId);
            Assert.Equal(2, actualWaves.Count());
        }

        [Fact]
        public void TestAddWaveAsync_NewWave_ReturnsWaveSurvey()
        {
            var parentSurveyId = Guid.NewGuid().ToString();
            var createSurvey = new ParentSurveyWave();
            var expectedSurvey = new Survey(SurveyType.OnlineBasic) { SurveyId = Guid.NewGuid().ToString() };

            var getWavesEndPoint = new Uri(ServiceAddress, $"ParentSurveys/{parentSurveyId}/Waves/");

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(getWavesEndPoint, createSurvey))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSurvey))));

            var target = new NfieldParentSurveyWavesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSurvey = target.AddWaveAsync(parentSurveyId, createSurvey).Result;

            Assert.Equal(expectedSurvey.SurveyId, actualSurvey.SurveyId);
        }

        [Fact]
        public void TestAddWaveAsync_NewWaveFromOther_ReturnsWaveSurvey()
        {
            var parentSurveyId = Guid.NewGuid().ToString();
            var waveId = Guid.NewGuid().ToString();
            var createSurveyCopy = new ParentSurveyWaveCopy();
            var expectedSurvey = new Survey(SurveyType.OnlineBasic) { SurveyId = Guid.NewGuid().ToString() };

            var getWavesEndPoint = new Uri(ServiceAddress, $"ParentSurveys/{parentSurveyId}/Waves/{waveId}");

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(getWavesEndPoint, createSurveyCopy))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSurvey))));

            var target = new NfieldParentSurveyWavesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSurvey = target.CopyWaveAsync(parentSurveyId, waveId, createSurveyCopy).Result;

            Assert.Equal(expectedSurvey.SurveyId, actualSurvey.SurveyId);
        }
    }
}

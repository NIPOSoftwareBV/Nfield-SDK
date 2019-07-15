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
using System.Threading.Tasks;
using Xunit;

namespace Nfield.Services
{

    public class NfieldSurveyGroupServiceTests : NfieldServiceTestsBase
    {
        [Fact]
        public async Task CanGetAllSurveyGroups()
        {
            var expectedSurveyGroups = new[]
            {
                new SurveyGroup
                {
                    SurveyGroupId = 2,
                    Name = "War & Peace",
                    Description = "Napoleon satisfaction surveys",
                    CreationDate = new DateTime(1812, 06, 26)
                },
                new SurveyGroup
                {
                    SurveyGroupId = 1,
                    Name = "Default",
                    CreationDate = new DateTime(1799, 11, 10)
                }
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "SurveyGroups")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSurveyGroups))));

            var target = new NfieldSurveyGroupService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSurveyGroups = await target.GetAllAsync();

            Assert.Equal(actualSurveyGroups.Count(), 2);

            var defaultGroup = actualSurveyGroups.Single(sg => sg.SurveyGroupId == 1);
            Assert.Equal(defaultGroup.Name, "Default");
            Assert.Equal(defaultGroup.CreationDate, new DateTime(1799, 11, 10));

            var secondGroup = actualSurveyGroups.Single(sg => sg.SurveyGroupId == 2);
            Assert.Equal(secondGroup.Name, "War & Peace");
            Assert.Equal(secondGroup.Description, "Napoleon satisfaction surveys");
            Assert.Equal(secondGroup.CreationDate, new DateTime(1812, 06, 26));
        }

        [Fact]
        public async Task CanGetSurveysInGroupAsync()
        {
            var expectedSurveys = new[]
            { new Survey(SurveyType.Basic) { SurveyId = "TestSurvey" },
              new Survey(SurveyType.Advanced) { SurveyId = "AnotherTestSurvey" }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "SurveyGroups/1/Surveys")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSurveys))));

            var target = new NfieldSurveyGroupService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSurveys = await target.GetSurveysAsync(1);

            Assert.Equal(expectedSurveys[0].SurveyId, actualSurveys.ToArray()[0].SurveyId);
            Assert.Equal(expectedSurveys[1].SurveyId, actualSurveys.ToArray()[1].SurveyId);
            Assert.Equal(2, actualSurveys.Count());
        }
    }
}

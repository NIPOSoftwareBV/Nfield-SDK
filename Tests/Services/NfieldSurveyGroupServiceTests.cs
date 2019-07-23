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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Nfield.Services
{

    public class NfieldSurveyGroupServiceTests : NfieldServiceTestsBase
    {
        private readonly NfieldSurveyGroupService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;

        public NfieldSurveyGroupServiceTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldSurveyGroupService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);
        }

        [Fact]
        public async Task CanCreateNewGroup()
        {
            var expectedGroup = new SurveyGroup
            {
                SurveyGroupId = 1,
                Name = "Default",
                Description = null,
                CreationDate = new DateTime(1799, 11, 10)
            };

            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, "SurveyGroups"), It.IsAny<SurveyGroupValues>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedGroup))));

            var result = await _target.CreateAsync(new SurveyGroupValues
            {
                Name = "Default",
                Description = null
            });

            Assert.NotNull(result);
            Assert.Equal("Default", result.Name);
            Assert.Equal(null, result.Description);
            Assert.Equal(new DateTime(1799, 11, 10), result.CreationDate);
            Assert.Equal(1, result.SurveyGroupId);
        }

        [Fact]
        public async Task CanCreateModifyAndDeleteGroup()
        {
            var createdGroup = new SurveyGroup
            {
                SurveyGroupId = 2,
                Name = "Default",
                Description = null,
                CreationDate = new DateTime(1799, 11, 10)
            };

            var updatedGroup = new SurveyGroup
            {
                SurveyGroupId = 2,
                Name = "Default with a twist",
                Description = "Modified description",
                CreationDate = new DateTime(1799, 11, 10)
            };

            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, "SurveyGroups"), It.IsAny<SurveyGroupValues>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(createdGroup))));
            _mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(new Uri(ServiceAddress, "SurveyGroups/2"), It.IsAny<SurveyGroupValues>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(updatedGroup))));
            _mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, "SurveyGroups/2")))
                .Returns(CreateTask(HttpStatusCode.OK));

            var result = await _target.CreateAsync(new SurveyGroupValues
            {
                Name = "Default",
                Description = null
            });

            Assert.NotNull(result);
            Assert.Equal("Default", result.Name);
            Assert.Equal(null, result.Description);
            Assert.Equal(new DateTime(1799, 11, 10), result.CreationDate);
            Assert.Equal(2, result.SurveyGroupId);

            // modify survey group object we got back, then post it
            result.Name = "Default with a twist";
            result.Description = "Modified description";

            result = await _target.UpdateAsync(result.SurveyGroupId, result);

            Assert.NotNull(result);
            Assert.Equal(updatedGroup.Name, result.Name);
            Assert.Equal(updatedGroup.Description, result.Description);
            Assert.Equal(updatedGroup.CreationDate, result.CreationDate);
            Assert.Equal(2, result.SurveyGroupId);

            await _target.DeleteAsync(result.SurveyGroupId);
        }

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

            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "SurveyGroups")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSurveyGroups))));

            var actualSurveyGroups = await _target.GetAllAsync();

            Assert.Equal(2, actualSurveyGroups.Count());

            var defaultGroup = actualSurveyGroups.Single(sg => sg.SurveyGroupId == 1);
            var expectedGroup = expectedSurveyGroups.Where(g => g.SurveyGroupId == 1).FirstOrDefault();

            Assert.Equal(expectedGroup.Name, defaultGroup.Name);
            Assert.Equal(expectedGroup.CreationDate, defaultGroup.CreationDate);

            var secondGroup = actualSurveyGroups.Single(sg => sg.SurveyGroupId == 2);
            expectedGroup = expectedSurveyGroups.Where(g => g.SurveyGroupId == 2).FirstOrDefault();

            Assert.Equal(expectedGroup.Name, secondGroup.Name);
            Assert.Equal(expectedGroup.Description, secondGroup.Description);
            Assert.Equal(expectedGroup.CreationDate, secondGroup.CreationDate);
        }

        [Fact]
        public async Task CanGetSpecificSurveyGroup()
        {
            var defaultSurveyGroup = new SurveyGroup
            {
                SurveyGroupId = 1,
                Name = "Default",
                Description = "Default Survey Group",
                CreationDate = DateTime.UtcNow
            };

            var expectedSurveyGroup = new SurveyGroup
            {
                SurveyGroupId = 2,
                Name = "SG",
                Description = "Some description for a survey group",
                CreationDate = DateTime.UtcNow

            };

            var surveyGroups = new[] { defaultSurveyGroup, expectedSurveyGroup };

            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"SurveyGroups/{expectedSurveyGroup.SurveyGroupId}")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSurveyGroup))));

            var actualSurveyGroup = await _target.GetAsync(expectedSurveyGroup.SurveyGroupId);

            Assert.Equal(expectedSurveyGroup.SurveyGroupId, actualSurveyGroup.SurveyGroupId);
            Assert.Equal(expectedSurveyGroup.Name, actualSurveyGroup.Name);
            Assert.Equal(expectedSurveyGroup.Description, actualSurveyGroup.Description);
            Assert.Equal(expectedSurveyGroup.CreationDate, actualSurveyGroup.CreationDate);
        }

        [Fact]
        public async Task CanGetSurveysInGroupAsync()
        {
            var expectedSurveys = new[]
            { new Survey(SurveyType.Basic) { SurveyId = "TestSurvey" },
              new Survey(SurveyType.Advanced) { SurveyId = "AnotherTestSurvey" }
            };

            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "SurveyGroups/1/Surveys")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSurveys))));

            var actualSurveys = await _target.GetSurveysAsync(1);

            Assert.Equal(expectedSurveys[0].SurveyId, actualSurveys.ToArray()[0].SurveyId);
            Assert.Equal(expectedSurveys[1].SurveyId, actualSurveys.ToArray()[1].SurveyId);
            Assert.Equal(2, actualSurveys.Count());
        }
    }
}

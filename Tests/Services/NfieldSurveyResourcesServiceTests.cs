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
using System.Threading.Tasks;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyResourcesService"/>
    /// </summary>
    public class NfieldSurveyResourcesServiceTests : NfieldServiceTestsBase
    {
        #region QueryAsync

        [Fact]
        public async Task TestQueryAsync_ServerReturnsQuery_ReturnsListWithSurveyResources()
        {
            var expectedSurveyResources = new List<SurveyResource>();

            for (var i = 0; i < 5; i++)
            {
                var surveyResource = new SurveyResource
                {
                    SurveyId = Guid.NewGuid().ToString(),
                    SurveyName = $"Survey{i}",
                    Owner = $"Owner{i}",
                    ClientName = $"Client{i}",
                    Size = i,
                    CreationDate = new DateTime(2020, 1, 1),
                    LastDataDownloadDate = new DateTime(2020, 1, 2),
                    LastDataCollectionDate = new DateTime(2020, 1, 3),
                    WillBeStoppedOn = new DateTime(2022, 10, 2),
                    WillBeDeletedOn = new DateTime(2022, 10, 3),
                    State = SurveyStatus.UnderConstruction,
                    Channel = SurveyChannel.Online,
                    IsExcludedFromAutomaticCleanup = i % 2 == 0
                };

                expectedSurveyResources.Add(surveyResource);
            }
            
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "SurveyResources/")))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(expectedSurveyResources))));

            var target = new NfieldSurveyResourcesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSurveyResources = await target.QueryAsync();

            Assert.Equal(5, actualSurveyResources.Count());

            for (var i = 0; i < 5; i++)
            {
                var actualSurveyResource = actualSurveyResources.ElementAt(i);
                var expectedSurveyResource = expectedSurveyResources[i];

                Assert.Equal(expectedSurveyResource.SurveyId, actualSurveyResource.SurveyId);
                Assert.Equal(expectedSurveyResource.SurveyName, actualSurveyResource.SurveyName);
                Assert.Equal(expectedSurveyResource.Owner, actualSurveyResource.Owner);
                Assert.Equal(expectedSurveyResource.Size, actualSurveyResource.Size);
                Assert.Equal(expectedSurveyResource.CreationDate, actualSurveyResource.CreationDate);
                Assert.Equal(expectedSurveyResource.LastDataDownloadDate, actualSurveyResource.LastDataDownloadDate);
                Assert.Equal(expectedSurveyResource.LastDataCollectionDate, actualSurveyResource.LastDataCollectionDate);
                Assert.Equal(expectedSurveyResource.WillBeStoppedOn, actualSurveyResource.WillBeStoppedOn);
                Assert.Equal(expectedSurveyResource.WillBeDeletedOn, actualSurveyResource.WillBeDeletedOn);
                Assert.Equal(expectedSurveyResource.State, actualSurveyResource.State);
                Assert.Equal(expectedSurveyResource.Channel, actualSurveyResource.Channel);
                Assert.Equal(expectedSurveyResource.IsExcludedFromAutomaticCleanup, actualSurveyResource.IsExcludedFromAutomaticCleanup);
            }
        }

        #endregion
    }
}
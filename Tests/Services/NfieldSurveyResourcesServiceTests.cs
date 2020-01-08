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
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithSurveyResources()
        {
            var expectedSurveyResources = new List<SurveyResources>();

            for (var i = 0; i < 5; i++)
            {
                var surveyResource = new SurveyResources
                {
                    SurveyId = Guid.NewGuid().ToString(),
                    SurveyName = $"Survey{i}",
                    Owner = $"Owner{i}",
                    ClientName = $"Client{i}",
                    Size = i,
                    CreationDate = new DateTime(2020, 1, 1),
                    LastDataDownloadDate = new DateTime(2020,1,2),
                    LastDataCollectionDate = new DateTime(2020, 1, 3),
                    State = SurveyStatus.UnderConstruction,
                    Channel = SurveyChannel.Online
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

            var actualSurveyResources = target.QueryAsync().Result;

            Assert.Equal(5, actualSurveyResources.Count());

            for (var i = 0; i < 5; i++)
            {
                Assert.Equal(expectedSurveyResources[i].SurveyId, actualSurveyResources.ToArray()[i].SurveyId);
                Assert.Equal(expectedSurveyResources[i].SurveyName, actualSurveyResources.ToArray()[i].SurveyName);
                Assert.Equal(expectedSurveyResources[i].Owner, actualSurveyResources.ToArray()[i].Owner);
                Assert.Equal(expectedSurveyResources[i].Size, actualSurveyResources.ToArray()[i].Size);
                Assert.Equal(expectedSurveyResources[i].CreationDate, actualSurveyResources.ToArray()[i].CreationDate);
                Assert.Equal(expectedSurveyResources[i].LastDataDownloadDate, actualSurveyResources.ToArray()[i].LastDataDownloadDate);
                Assert.Equal(expectedSurveyResources[i].LastDataCollectionDate, actualSurveyResources.ToArray()[i].LastDataCollectionDate);
                Assert.Equal(expectedSurveyResources[i].State, actualSurveyResources.ToArray()[i].State);
                Assert.Equal(expectedSurveyResources[i].Channel, actualSurveyResources.ToArray()[i].Channel);
            }
        }

        #endregion
    }
}
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
    /// <summary>
    /// Tests for <see cref="NfieldSurveyResourcesService"/>
    /// </summary>
    public class NfieldSurveyResourcesServiceTests : NfieldServiceTestsBase
    {
        #region QueryAsync

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithSurveyResources()
        {
            var expectedSurveyResources = new[]
            { new SurveyResources { Owner = "Owner1" },
              new SurveyResources { Owner = "Owner2" },
              new SurveyResources { Owner = "Owner3" }
            };
            
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "SurveyResources/")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSurveyResources))));

            var target = new NfieldSurveyResourcesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSurveyResources = target.QueryAsync().Result;

            Assert.Equal(expectedSurveyResources[0].Owner, actualSurveyResources.ToArray()[0].Owner);
            Assert.Equal(expectedSurveyResources[1].Owner, actualSurveyResources.ToArray()[1].Owner);
            Assert.Equal(expectedSurveyResources[2].Owner, actualSurveyResources.ToArray()[2].Owner);
            Assert.Equal(3, actualSurveyResources.Count());
        }

        #endregion
    }
}

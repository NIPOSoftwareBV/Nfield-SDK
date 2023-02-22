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
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;
using Nfield.Services.Implementation;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldDomainInterviewersWorklogService"/>
    /// </summary>
    public class NfieldDomainInterviewersWorklogServiceTests : NfieldServiceTestsBase
    {
        private const string InterviewersWorklogEndpoint = "InterviewersWorklog";

        private readonly NfieldDomainInterviewersWorklogService _target;
        private readonly Mock<INfieldHttpClient> _mockedHttpClient;
        private readonly Mock<INfieldConnectionClient> _mockedNfieldConnection;

        public NfieldDomainInterviewersWorklogServiceTests()
        {
            _mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(_mockedNfieldConnection);

            _target = new NfieldDomainInterviewersWorklogService();
            _target.InitializeNfieldConnection(_mockedNfieldConnection.Object);
        }


        #region QueryAsync

        [Fact]
        public async Task TestQueryAsync_ServerReturnsUrl_ReturnsUrlAsync()
        {
            // Arrange
            var logQuery = new LogQueryModel
            {
                From = DateTime.Now.AddDays(-3).ToUniversalTime(),
                To = DateTime.Now.ToUniversalTime()
            };
            var expectedURl = "http://expected.url";
            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, InterviewersWorklogEndpoint), logQuery))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));
            _mockedHttpClient.Setup(client => client.GetAsync(new Uri(ServiceAddress, $"BackgroundActivities/activity1")))                
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(new { Status = 2, DownloadDataUrl = expectedURl })))).Verifiable();

           

            // Act
            var result = await _target.QueryAsync(logQuery);

            // Assert
            Assert.Equal(expectedURl, result);
            _mockedHttpClient.Verify();
        }

        #endregion
    }
}

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

using System;
using Nfield.Infrastructure;
using Nfield.Services.Implementation;
using Xunit;
using Nfield.Models;
using Moq;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldInterviewersService"/>
    /// </summary>
    public class NfieldInterviewersServiceTests : NfieldServiceTestsBase
    {

        #region GetInterviewersWorklogDownloadLinkAsync

        [Fact]
        public async Task DownloadLogs()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            const string activityId = "activity-id";
            const string logsLink1 = "logs-link-1";
            const string logsLink2 = "logs-link-2";
            var query = new LogQueryModel
            {
                From = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                To = DateTime.UtcNow
            };

            mockedHttpClient
               .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, $"InterviewersWorklog"), It.Is<LogQueryModel>(
                   q => q.From == query.From && q.To == query.To)))
               .Returns(
                Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { ActivityId = activityId }))
                    })).Verifiable();

            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"BackgroundActivities/{activityId}")))
                .Returns(Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { DownloadDataUrl = logsLink1, ActivityId = activityId, Status = 2 /* Succeeded */ }))
                    })).Verifiable();

            // Test it using the model
            var result = await target.QueryLogsAsync(query);
            mockedHttpClient.Verify();
            Assert.Equal(logsLink1, result);

        }

        #endregion

    }
}

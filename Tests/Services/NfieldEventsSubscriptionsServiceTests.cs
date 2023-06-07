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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.SDK.Models.Events;
using Nfield.SDK.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldEventsSubscriptionsServiceTests"/>
    /// </summary>
    public class NfieldEventsSubscriptionsServiceTests : NfieldServiceTestsBase
    {
        [Fact]
        public async Task Test_QueryAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var eventSubscriptions= new List<EventsSubscriptionModel> {
                new EventsSubscriptionModel {
                    ResourceId = Guid.NewGuid().ToString(),
                    Topic = Guid.NewGuid().ToString(),
                    Name = "WebHookSubscription",
                    WebHookUri = "https://www.avalidurl.com",
                    EventTypes = new List<string>
                    {
                        "TargetReached"
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(eventSubscriptions));

            var endpointUri = new Uri(ServiceAddress, $"Events/Subscriptions");

            mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldEventsSubscriptionsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = await target.QueryAsync();
            mockedHttpClient.Verify();
            Assert.Equal(eventSubscriptions.First().ResourceId, actual.First().ResourceId);
            Assert.Equal(eventSubscriptions.First().Topic, actual.First().Topic);
            Assert.Equal(eventSubscriptions.First().WebHookUri, actual.First().WebHookUri);
            Assert.Equal(eventSubscriptions.First().Name, actual.First().Name);
            Assert.Equal(eventSubscriptions.First().EventTypes.Count(), actual.First().EventTypes.Count());
            Assert.Equal(eventSubscriptions.First().EventTypes.First(), actual.First().EventTypes.First());

        }
    }
}

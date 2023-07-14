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
    /// Tests for <see cref="NfieldEventSubscriptionsServiceTests"/>
    /// </summary>
    public class NfieldEventSubscriptionsServiceTests : NfieldServiceTestsBase
    {
        private const string EventSubscriptionName = "MyWebHookSubscription";

        private readonly NfieldEventSubscriptionsService _target;
        private readonly Mock<INfieldHttpClient> _mockedHttpClient;
        private readonly Mock<INfieldConnectionClient> _mockedNfieldConnection;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <remarks>
        /// Initialize mock objects to reuse them inside the tests.
        /// </remarks>
        public NfieldEventSubscriptionsServiceTests()
        {
            _mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(_mockedNfieldConnection);

            _target = new NfieldEventSubscriptionsService();
            _target.InitializeNfieldConnection(_mockedNfieldConnection.Object);
        }

        [Fact]
        public async Task Test_QueryAsync()
        {
            // Arrange
            var eventSubscriptions = new List<EventSubscriptionModel> {
                new EventSubscriptionModel {
                    DomainId = Guid.NewGuid().ToString(),
                    Name = "WebHookSubscription",
                    WebHookUri = "https://www.avalidurl.com",
                    EventTypes = new List<string>
                    {
                        "TargetReached"
                    }
                }
            };

            var content = new StringContent(JsonConvert.SerializeObject(eventSubscriptions));

            var endpointUri = new Uri(ServiceAddress, "Events/Subscriptions");

            _mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            // Act
            var actual = await _target.QueryAsync();

            // Assert
            _mockedHttpClient.Verify();
            Assert.Equal(eventSubscriptions.First().DomainId, actual.First().DomainId);
            Assert.Equal(eventSubscriptions.First().WebHookUri, actual.First().WebHookUri);
            Assert.Equal(eventSubscriptions.First().Name, actual.First().Name);
            Assert.Equal(eventSubscriptions.First().EventTypes.Count(), actual.First().EventTypes.Count());
            Assert.Equal(eventSubscriptions.First().EventTypes.First(), actual.First().EventTypes.First());
        }

        [Fact]
        public async Task Test_GetAsync()
        {
            // Arrange
            var eventSubscription = new EventSubscriptionModel
            {
                DomainId = Guid.NewGuid().ToString(),
                Name = EventSubscriptionName,
                WebHookUri = "https://www.avalidurl.com",
                EventTypes = new List<string>
                    {
                        "TargetReached"
                    }
            };

            var content = new StringContent(JsonConvert.SerializeObject(eventSubscription));

            var endpointUri = new Uri(ServiceAddress, $"Events/Subscriptions/{EventSubscriptionName}");

            _mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            // Act
            var actual = await _target.GetAsync(EventSubscriptionName);

            // Assert
            _mockedHttpClient.Verify();
            Assert.Equal(eventSubscription.DomainId, actual.DomainId);
            Assert.Equal(eventSubscription.WebHookUri, actual.WebHookUri);
            Assert.Equal(eventSubscription.Name, actual.Name);
            Assert.Equal(eventSubscription.EventTypes.Count(), actual.EventTypes.Count());
            Assert.Equal(eventSubscription.EventTypes.First(), actual.EventTypes.First());
        }

        [Fact]
        public async Task Test_CreateAsync()
        {
            // Arrange
            var createSubscriptionModel = new CreateEventSubscriptionModel
            {
                EventSubscriptionName = EventSubscriptionName,
                Endpoint = new Uri("https://www.validurl.com"),
                EventTypes = new List<string> { "TargetReached" }
            };

            var content = new StringContent(JsonConvert.SerializeObject(new EventSubscriptionModel { Name = EventSubscriptionName }));

            var endpointUri = new Uri(ServiceAddress, "Events/Subscriptions");

            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(endpointUri, createSubscriptionModel))
                .Returns(CreateTask(HttpStatusCode.OK, content))
                .Verifiable();

            // Act
            var createdEventSubscription = await _target.CreateAsync(createSubscriptionModel);

            // Assert
            _mockedHttpClient.Verify();
            Assert.Equal(EventSubscriptionName, createdEventSubscription.SubscriptionName);
        }

        [Fact]
        public async Task Test_UpdateAsync()
        {
            // Arrange
            var endpointUri = new Uri(ServiceAddress, $"Events/Subscriptions/{EventSubscriptionName}");

            var model = new UpdateEventSubscriptionModel
            {
                Endpoint = new Uri("https://www.validurl.com"),
                EventTypes = new List<string> { "TargetReached" }
            };

            _mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(endpointUri, model))
                .Returns(CreateTask(HttpStatusCode.OK))
                .Verifiable();

            // Act
            await _target.UpdateAsync(EventSubscriptionName, model);

            // Assert
            _mockedHttpClient.Verify();
        }

        [Fact]
        public async Task Test_DeleteAsync()
        {
            // Arrange
            var endpointUri = new Uri(ServiceAddress, $"Events/Subscriptions/{EventSubscriptionName}");

            _mockedHttpClient
                .Setup(client => client.DeleteAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            // Act
            await _target.DeleteAsync(EventSubscriptionName);

            // Assert
            _mockedHttpClient.Verify();
        }
    }
}

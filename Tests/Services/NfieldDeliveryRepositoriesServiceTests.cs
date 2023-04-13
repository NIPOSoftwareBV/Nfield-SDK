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
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.SDK.Models.Delivery;
using Nfield.SDK.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldDeliveryRepositoriesService"/>
    /// </summary>
    public class NfieldDeliveryRepositoriesServiceTests : NfieldServiceTestsBase
    {
        [Fact]
        public async Task Test_QueryAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var repositoryModels = new List<RepositoryModel> { new RepositoryModel { Id = 2, Name = "repositoryName" } };
            var content = new StringContent(JsonConvert.SerializeObject(repositoryModels));

            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories");

            mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldDeliveryRepositoriesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = await target.QueryAsync();
            mockedHttpClient.Verify();
            Assert.Equal(repositoryModels.First().Name, actual.First().Name);
        }

        [Fact]
        public async Task Test_GetAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var repositoryModel = new RepositoryModel { Id = 2, Name = "repositoryName" };
            var content = new StringContent(JsonConvert.SerializeObject(repositoryModel));

            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryModel.Id}");

            mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldDeliveryRepositoriesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = await target.GetAsync(repositoryModel.Id);
            mockedHttpClient.Verify();
            Assert.Equal(repositoryModel.Name, actual.Name);
        }

        [Fact]
        public async Task Test_GetCredentialsAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var repositoryConnectionInfo = new RepositoryConnectionInfo { UserId = "userId" };
            var content = new StringContent(JsonConvert.SerializeObject(repositoryConnectionInfo));

            var repositoryId = 1;
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}/Credentials");

            mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldDeliveryRepositoriesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = await target.GetCredentialsAsync(repositoryId);
            mockedHttpClient.Verify();
            Assert.Equal(repositoryConnectionInfo.UserId, repositoryConnectionInfo.UserId);
        }

        [Fact]
        public async Task Test_GetMetricsAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var repositoryMetricsModel = new RepositoryMetricsModel { DTU = new ResourceMetricModel { Value = 3 } };
            var content = new StringContent(JsonConvert.SerializeObject(repositoryMetricsModel));

            var repositoryId = 1;
            var interval = 2;
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}/Metrics/{interval}");

            mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldDeliveryRepositoriesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = await target.GetMetricsAsync(repositoryId, interval);
            mockedHttpClient.Verify();
            Assert.Equal(repositoryMetricsModel.DTU.Value, actual.DTU.Value);
        }

        [Fact]
        public async Task Test_QuerySubscriptionsLogsAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var repositorySubscriptionLogModels = new List<RepositorySubscriptionLogModel> { new RepositorySubscriptionLogModel { PlanName = "planName" } };
            var content = new StringContent(JsonConvert.SerializeObject(repositorySubscriptionLogModels));

            var repositoryId = 1;
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}/Logs/Subscriptions");

            mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldDeliveryRepositoriesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = await target.QuerySubscriptionsLogsAsync(repositoryId);
            mockedHttpClient.Verify();
            Assert.Equal(repositorySubscriptionLogModels.First().PlanName, actual.First().PlanName);
        }

        [Fact]
        public async Task Test_QueryActivityLogsAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var repositoryActivityLogModels = new List<RepositoryActivityLogModel> { new RepositoryActivityLogModel { SurveyName = "surveyName" } };
            var content = new StringContent(JsonConvert.SerializeObject(repositoryActivityLogModels));

            var repositoryId = 1;
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}/Logs/Activities");

            mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldDeliveryRepositoriesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = await target.QueryActivityLogsAsync(repositoryId);
            mockedHttpClient.Verify();
            Assert.Equal(repositoryActivityLogModels.First().SurveyName, actual.First().SurveyName);
        }

        [Fact]
        public async Task Test_PostAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var createRepositoryModel = new CreateRepositoryModel { Name = "respositoryName" };
            var content = new StringContent(JsonConvert.SerializeObject(new { Id = 2 }));

            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories");

            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(endpointUri, createRepositoryModel))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldDeliveryRepositoriesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var newRepository = await target.PostAsync(createRepositoryModel);

            mockedHttpClient.Verify();
            Assert.Equal(2, newRepository.Id);
        }

        [Fact]
        public async Task Test_PostRepositorySubscriptionAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var createRepositorySubscriptionModel = new CreateRepositorySubscriptionModel { PlanId = 2 };
            var content = new StringContent(JsonConvert.SerializeObject(createRepositorySubscriptionModel));

            var repositoryId = 1;
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}/Subscriptions");

            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(endpointUri, createRepositorySubscriptionModel))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            var target = new NfieldDeliveryRepositoriesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            await target.PostRepositorySubscriptionAsync(repositoryId, createRepositorySubscriptionModel);

            mockedHttpClient.Verify();
        }

        [Fact]
        public async Task Test_DeleteAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var repositoryId = 1;

            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}");

            mockedHttpClient
                .Setup(client => client.DeleteAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            var target = new NfieldDeliveryRepositoriesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            await target.DeleteAsync(repositoryId);

            mockedHttpClient.Verify();
        }
    }
}

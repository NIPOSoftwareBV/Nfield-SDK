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
using Nfield.SDK.Models.Delivery;
using Nfield.SDK.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldDeliveryRepositoryUsersService"/>
    /// </summary>
    public class NfieldDeliveryRepositoryUsersServiceTests : NfieldServiceTestsBase
    {
        [Fact]
        public async Task Test_QueryAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var repositoryUserModels = new List<RepositoryUserModel> { new RepositoryUserModel { Id = 2, Name = "repositoryUser" } };
            var content = new StringContent(JsonConvert.SerializeObject(repositoryUserModels));

            var repositoryId = 1;
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}/Users");

            mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldDeliveryRepositoryUsersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = await target.QueryAsync(repositoryId);
            mockedHttpClient.Verify();
            Assert.Equal(repositoryUserModels.First().Name, actual.First().Name);
        }

        [Fact]
        public async Task Test_GetAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var repositoryUserModel = new RepositoryUserModel { Id = 2, Name = "repositoryUser" };
            var content = new StringContent(JsonConvert.SerializeObject(repositoryUserModel));

            var repositoryId = 1;
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}/Users/{repositoryUserModel.Id}");

            mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldDeliveryRepositoryUsersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = await target.GetAsync(repositoryId, repositoryUserModel.Id);
            mockedHttpClient.Verify();
            Assert.Equal(repositoryUserModel.Name, actual.Name);
        }

        [Fact]
        public async Task Test_PostAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var repositoryUserName = "repositoryUserName";
            var content = new StringContent(repositoryUserName);

            var repositoryId = 1;
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}/Users");

            mockedHttpClient
                .Setup(client => client.PostAsync(endpointUri, It.Is<StringContent>(stringContent => stringContent.ReadAsStringAsync().Result.Equals(repositoryUserName))))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            var target = new NfieldDeliveryRepositoryUsersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            await target.PostAsync(repositoryId, repositoryUserName);

            mockedHttpClient.Verify();
        }

        [Fact]
        public async Task Test_ResetUsertAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            //var repositoryUserModel = new RepositoryUserModel { Id = 2, Name = "repositoryUser" };
            //var content = new StringContent(JsonConvert.SerializeObject(repositoryUserModel));

            var repositoryId = 1;
            var userId = 2;
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}/Users/{userId}/Reset");

            mockedHttpClient
                .Setup(client => client.PostAsync(endpointUri, null))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            var target = new NfieldDeliveryRepositoryUsersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            await target.PostAsync(repositoryId, userId);

            mockedHttpClient.Verify();
        }

        [Fact]
        public async Task Test_DeleteAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var repositoryId = 1;
            var userId = 2;

            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}/Users/{userId}");

            mockedHttpClient
                .Setup(client => client.DeleteAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            var target = new NfieldDeliveryRepositoryUsersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            await target.DeleteAsync(repositoryId, userId);

            mockedHttpClient.Verify();
        }
    }
}

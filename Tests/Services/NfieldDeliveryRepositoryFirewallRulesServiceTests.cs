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
    /// Tests for <see cref="NfieldDeliveryRepositoryFirewallRulesService"/>
    /// </summary>
    public class NfieldDeliveryRepositoryFirewallRulesServiceTests : NfieldServiceTestsBase
    {
        [Fact]
        public async Task Test_QueryAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var firewallRuleModels = new List<FirewallRuleModel> { new FirewallRuleModel { Id = 2, Name = "firewallRule" } };
            var content = new StringContent(JsonConvert.SerializeObject(firewallRuleModels));

            var repositoryId = 1;
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}/FirewallRules");

            mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldDeliveryRepositoryFirewallRulesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = await target.QueryAsync(repositoryId);
            mockedHttpClient.Verify();
            Assert.Equal(firewallRuleModels.First().Name, actual.First().Name);
        }

        [Fact]
        public async Task  Test_GetAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var firewallRuleModel = new FirewallRuleModel { Id = 2, Name = "firewallRule" };
            var content = new StringContent(JsonConvert.SerializeObject(firewallRuleModel));

            var repositoryId = 1;
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}/FirewallRules/{firewallRuleModel.Id}");

            mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldDeliveryRepositoryFirewallRulesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = await target.GetAsync(repositoryId, firewallRuleModel.Id);
            mockedHttpClient.Verify();
            Assert.Equal(firewallRuleModel.Name, actual.Name);
        }

        [Fact]
        public async Task Test_PostAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var firewallRuleModel = new FirewallRuleModel { Name = "firewallRule" };
            //var content = new StringContent(JsonConvert.SerializeObject(firewallRuleModel));

            var repositoryId = 1;
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}/FirewallRules");

            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(endpointUri, firewallRuleModel))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            var target = new NfieldDeliveryRepositoryFirewallRulesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            await target.PostAsync(repositoryId, firewallRuleModel);

            mockedHttpClient.Verify();
        }

        [Fact]
        public async Task Test_DeleteAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var repositoryId = 1;
            var firewallRuleId = 2;

            var endpointUri = new Uri(ServiceAddress, $"Delivery/Repositories/{repositoryId}/FirewallRules/{firewallRuleId}");

            mockedHttpClient
                .Setup(client => client.DeleteAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            var target = new NfieldDeliveryRepositoryFirewallRulesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            await target.DeleteAsync(repositoryId, firewallRuleId);

            mockedHttpClient.Verify();
        }
    }
}

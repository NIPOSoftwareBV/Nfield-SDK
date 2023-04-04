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
    /// Tests for <see cref="NfieldDeliverySurveyPropertiesService"/>
    /// </summary>
    public class NfieldDeliverySurveyPropertiesServiceTests : NfieldServiceTestsBase
    {
        [Fact]
        public async Task Test_QueryAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var domainSurveyPropertyModels = new List<DomainSurveyPropertyModel> { new DomainSurveyPropertyModel { Id = 2, Key = "surveyProperty" } };
            var content = new StringContent(JsonConvert.SerializeObject(domainSurveyPropertyModels));

            var surveyId = "surveyId";
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Surveys/{surveyId}/Properties");

            mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldDeliverySurveyPropertiesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = await target.QueryAsync(surveyId);
            mockedHttpClient.Verify();
            Assert.Equal(domainSurveyPropertyModels.First().Key, actual.First().Key);
        }

        [Fact]
        public async Task Test_GetByIdAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var domainSurveyPropertyModel = new DomainSurveyPropertyModel { Id = 2, Key = "surveyProperty" };
            var content = new StringContent(JsonConvert.SerializeObject(domainSurveyPropertyModel));

            var surveyId = "surveyId";
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Surveys/{surveyId}/Properties/{domainSurveyPropertyModel.Id}");

            mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldDeliverySurveyPropertiesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = await target.GetByIdAsync(surveyId, domainSurveyPropertyModel.Id);
            mockedHttpClient.Verify();
            Assert.Equal(domainSurveyPropertyModel.Key, actual.Key);
        }

        [Fact]
        public async Task Test_PostAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var domainSurveyPropertyModel = new CreateDomainSurveyPropertyModel { Key = "surveyProperty" };
            var content = new StringContent(JsonConvert.SerializeObject(domainSurveyPropertyModel));

            var surveyId = "surveyId";
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Surveys/{surveyId}/Properties");

            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(endpointUri, domainSurveyPropertyModel))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            var target = new NfieldDeliverySurveyPropertiesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            await target.PostAsync(surveyId, domainSurveyPropertyModel);

            mockedHttpClient.Verify();
        }

        [Fact]
        public async Task Test_PutSurveyPropertyAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var domainSurveyPropertyModel = new UpdateDomainSurveyPropertyModel { Key = "surveyProperty" };
            var content = new StringContent(JsonConvert.SerializeObject(domainSurveyPropertyModel));

            var surveyId = "surveyId";
            var propertyId = 2;
            var endpointUri = new Uri(ServiceAddress, $"Delivery/Surveys/{surveyId}/Properties/{propertyId}");

            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(endpointUri, domainSurveyPropertyModel))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            var target = new NfieldDeliverySurveyPropertiesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            await target.PutSurveyPropertyAsync(surveyId, propertyId, domainSurveyPropertyModel);

            mockedHttpClient.Verify();
        }

        [Fact]
        public async Task Test_DeleteAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var surveyId = "surveyId";
            var propertyId = 2;

            var endpointUri = new Uri(ServiceAddress, $"Delivery/Surveys/{surveyId}/Properties/{propertyId}");

            mockedHttpClient
                .Setup(client => client.DeleteAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            var target = new NfieldDeliverySurveyPropertiesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            await target.DeleteAsync(surveyId, propertyId);

            mockedHttpClient.Verify();
        }
    }
}

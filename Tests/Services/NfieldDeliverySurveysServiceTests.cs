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
    /// Tests for <see cref="NfieldDeliverySurveysService"/>
    /// </summary>
    public class NfieldDeliverySurveysServiceTests : NfieldServiceTestsBase
    {
        [Fact]
        public async Task Test_QueryAsync()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var domainSurveyModels = new List<DomainSurveyModel> { new DomainSurveyModel { Id = 2, Name = "surveyName" } };
            var content = new StringContent(JsonConvert.SerializeObject(domainSurveyModels));

            var endpointUri = new Uri(ServiceAddress, $"Delivery/Surveys");

            mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, content)).Verifiable();

            var target = new NfieldDeliverySurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = await target.QueryAsync();
            mockedHttpClient.Verify();
            Assert.Equal(domainSurveyModels.First().Name, actual.First().Name);
        }
    }
}

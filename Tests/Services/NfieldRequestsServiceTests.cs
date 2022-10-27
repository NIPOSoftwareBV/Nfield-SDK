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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Nfield.Services
{
    public class NfieldRequestsServiceTests : NfieldServiceTestsBase 
    {
        #region  QueryAsync

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithRequests()
        {
            var header1 = new RequestHeader
            {
                Id = 1,
                Name = "Header1",
                Value = "Api 0 header 0"
            };
            var header2 = new RequestHeader
            {
                Id = 2,
                Name = "Header2",
                Value = "Api 1 header 1"
            };
            var expectedRequests = new[]
            {
                new Request
                {
                    Name = "TestRequest",
                    Description = "Description",
                    PayloadTemplate = "Template",
                    RequestHttpMethod = RequestHttpMethod.POST,
                    Headers = new List<RequestHeader>
                    {
                        header1
                    }
                },
                new Request
                {
                    Name = "AnotherTestRequest",
                    Description = "Another Description",
                    PayloadTemplate = "Another Template",
                    RequestHttpMethod = RequestHttpMethod.PATCH,
                    Headers = new List<RequestHeader>
                    {
                        header2
                    }
                }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "requests/")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedRequests))));

            var target = new NfieldRequestsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualRequests = target.QueryAsync().Result.ToArray();

            Assert.Equal(expectedRequests[0].Name, actualRequests[0].Name);
            Assert.Equal(expectedRequests[0].Description, actualRequests[0].Description);
            Assert.Equal(expectedRequests[0].PayloadTemplate, actualRequests[0].PayloadTemplate);
            Assert.Equal(expectedRequests[0].RequestHttpMethod, actualRequests[0].RequestHttpMethod);
            Assert.Equal(expectedRequests[0].Headers.First().Id, actualRequests[0].Headers.First().Id);
            Assert.Equal(expectedRequests[0].Headers.First().Name, actualRequests[0].Headers.First().Name);
            Assert.Equal(expectedRequests[0].Headers.First().Value, actualRequests[0].Headers.First().Value);
            Assert.Equal(expectedRequests[1].Name, actualRequests[1].Name);
            Assert.Equal(expectedRequests[1].Description, actualRequests[1].Description);
            Assert.Equal(expectedRequests[1].PayloadTemplate, actualRequests[1].PayloadTemplate);
            Assert.Equal(expectedRequests[1].RequestHttpMethod, actualRequests[1].RequestHttpMethod);
            Assert.Equal(expectedRequests[1].Headers.First().Id, actualRequests[1].Headers.First().Id);
            Assert.Equal(expectedRequests[1].Headers.First().Name, actualRequests[1].Headers.First().Name);
            Assert.Equal(expectedRequests[1].Headers.First().Value, actualRequests[1].Headers.First().Value);
            Assert.Equal(2, actualRequests.Count());
        }

        #endregion

        #region AddAsync

        [Fact]
        public void TestAddAsync_RequestIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldRequestsService();
            Assert.ThrowsAsync<ArgumentNullException>(() => target.RemoveAsync(null));
        }

        [Fact]
        public void TestAddAsync_ServerAccepts_ReturnsRequest()
        {
            var header = new RequestHeader
            {
                Name = "Header",
                Value = "Api header"
            };
            var request = new Request
            {
                Name = "New Request",
                Description = "New Description",
                PayloadTemplate = "Template",
                RequestHttpMethod = RequestHttpMethod.GET,
                Headers = new List<RequestHeader>
                {
                    header
                }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(request));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, "requests/"), request))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldRequestsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddAsync(request).Result;

            Assert.Equal(request.Name, actual.Name);
            Assert.Equal(request.Description, actual.Description);
            Assert.Equal(request.PayloadTemplate, actual.PayloadTemplate);
            Assert.Equal(request.RequestHttpMethod, actual.RequestHttpMethod);
            Assert.Equal(request.Headers.First().Name, actual.Headers.First().Name);
            Assert.Equal(request.Headers.First().Value, actual.Headers.First().Value);
        }

        #endregion

        #region RemoveAsync

        [Fact]
        public void TestRemoveAsync_RequestIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldRequestsService();
            Assert.ThrowsAsync<ArgumentNullException>(() => target.RemoveAsync(null));
        }

        [Fact]
        public void TestRemoveAsync_ServerRemovedRequest_DoesNotThrow()
        {
            var request = new Request
            {
                Id = 12,
                Name = "ApiToDelete"
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, $"requests/12")))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldRequestsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // assert: no throw
            target.RemoveAsync(request).Wait();
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_RequestArgumentIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldRequestsService();
            Assert.ThrowsAsync<ArgumentNullException>(() => target.UpdateAsync(null));
        }

        [Fact]
        public void TestUpdateAsync_RequestExists_ReturnsRequest()
        {
            const string RequestName = "ApiToUpdate";
            var request = new Request
            {
                Name = RequestName,
                Description = "updated description",
                PayloadTemplate = "updated template",
                RequestHttpMethod = RequestHttpMethod.POST
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, "requests/"), request))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(request))));

            var target = new NfieldRequestsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.UpdateAsync(request).Result;

            Assert.Equal(request.Description, actual.Description);
            Assert.Equal(request.PayloadTemplate, actual.PayloadTemplate);
            Assert.Equal(request.RequestHttpMethod, actual.RequestHttpMethod);
        }

        #endregion
    }
}

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
    public class NfieldExternalApisServiceTests : NfieldServiceTestsBase
    {
        #region  QueryAsync

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithExternalApis()
        {
            var header1 = new ExternalApiHeader
            {
                HeaderId = 1,
                Name = "Header1",
                Value = "Api 0 header 0"
            };
            var header2 = new ExternalApiHeader
            {
                HeaderId = 2,
                Name = "Header2",
                Value = "Api 1 header 1"
            };
            var expectedExternalApis = new[]
            {
                new ExternalApi
                {
                    Name = "TestExternalApi",
                    Description = "Description",
                    Headers = new List<ExternalApiHeader>
                    {
                        header1
                    }
                },
                new ExternalApi
                {
                    Name = "AnotherTestExternalApi",
                    Description = "Another Description",
                    Headers = new List<ExternalApiHeader>
                    {
                        header2
                    }
                }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "externalapis/")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedExternalApis))));

            var target = new NfieldExternalApisService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualExternalApis = target.QueryAsync().Result.ToArray();

            Assert.Equal(expectedExternalApis[0].Name, actualExternalApis[0].Name);
            Assert.Equal(expectedExternalApis[0].Description, actualExternalApis[0].Description);
            Assert.Equal(expectedExternalApis[0].Headers.First().HeaderId, header1.HeaderId);
            Assert.Equal(expectedExternalApis[0].Headers.First().Name, header1.Name);
            Assert.Equal(expectedExternalApis[0].Headers.First().Value, header1.Value);
            Assert.Equal(expectedExternalApis[1].Name, actualExternalApis[1].Name);
            Assert.Equal(expectedExternalApis[1].Description, actualExternalApis[1].Description);
            Assert.Equal(expectedExternalApis[1].Headers.First().HeaderId, header2.HeaderId);
            Assert.Equal(expectedExternalApis[1].Headers.First().Name, header2.Name);
            Assert.Equal(expectedExternalApis[1].Headers.First().Value, header2.Value);
            Assert.Equal(2, actualExternalApis.Count());
        }

        #endregion

        #region RemoveAsync

        [Fact]
        public void TestRemoveAsync_ExternalApiIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldExternalApisService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(null)));
        }

        [Fact]
        public void TestRemoveAsync_ServerRemovedExternalApi_DoesNotThrow()
        {
            const string ExternalApiName = "ApiToDelete";
            var externalApi = new ExternalApi { Name = ExternalApiName };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, "externalapis/" + ExternalApiName)))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldExternalApisService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // assert: no throw
            target.RemoveAsync(externalApi).Wait();
        }

        #endregion
    }
}

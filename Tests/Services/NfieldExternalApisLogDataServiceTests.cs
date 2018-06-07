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
using Nfield.SDK.Models;
using Nfield.SDK.Services.Implementation;
using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Nfield.Services
{
    public class NfieldExternalApisLogDataServiceTests : NfieldServiceTestsBase
    {
        [Fact]
        public void TestPostAsync_LogDownloadDataRequestArgumentIsNull_Throws()
        {
            var target = new NfieldExternalApisLogDataService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.PostAsync(null)));
        }

        [Fact]
        public void TestPostAsync_ValidInput_PostsData()
        {
            var logDownload = new ExternalApiLogDownload();
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient.Setup(client => client.PostAsJsonAsync<ExternalApiLogDownload>(It.IsAny<string>(), It.IsAny<ExternalApiLogDownload>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(""))));

            var target = new NfieldExternalApisLogDataService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.PostAsync(logDownload);

            mockedHttpClient.Verify(client => client.PostAsJsonAsync<ExternalApiLogDownload>(It.IsAny<string>(), logDownload), Times.Once());

            // finish this test !!!
        }
    }
}

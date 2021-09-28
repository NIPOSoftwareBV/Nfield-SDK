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
using Nfield.Infrastructure;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Formatting;
using Nfield.TestHelpers;

namespace Nfield.Services
{
    public abstract class NfieldServiceTestsBase
    {
        protected readonly Uri ServiceAddress = new Uri("http://localhost/nfieldapi/");

        protected Task<HttpResponseMessage> CreateTask(HttpStatusCode httpStatusCode, HttpContent content = null)
        {
            return Task.Factory.StartNew(() => new HttpResponseMessage(httpStatusCode) { Content = content });
        }

        protected void UnwrapAggregateException(Task task)
        {
            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        internal Mock<INfieldHttpClient> CreateHttpClientMock(Mock<INfieldConnectionClient> mockedNfieldConnection)
        {
            var mockedHttpClient = new Mock<INfieldHttpClient>();

            mockedNfieldConnection.SetupGet(connection => connection.Client).Returns(mockedHttpClient.Object);
            mockedNfieldConnection.SetupGet(connection => connection.NfieldServerUri).Returns(ServiceAddress);

            //setup the mocked HttpClient to return httpStatusCode for all methods that send a request to the server

            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(It.IsAny<Uri>(), It.IsAny<object>()))
                .Returns(CreateTask(HttpStatusCode.BadRequest));

            mockedHttpClient
                .Setup(client => client.PostAsync(It.IsAny<Uri>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.BadRequest));

            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(It.IsAny<Uri>(), It.IsAny<object>()))
                .Returns(NfieldTaskHelpers.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("") }));

            mockedHttpClient
                .Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>()))
                .Returns(CreateTask(HttpStatusCode.BadRequest));

            mockedHttpClient
                .Setup(client => client.GetAsync(It.IsAny<Uri>()))
                .Returns(CreateTask(HttpStatusCode.BadRequest));

            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(It.IsAny<Uri>(), It.IsAny<object>()))
                .Returns(NfieldTaskHelpers.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("") }));

            return mockedHttpClient;
        }

        internal INfieldConnectionClient InitMockClientGet<T>(Uri url, T responseObjectContent)
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var responseContent = new ObjectContent<T>(responseObjectContent, new JsonMediaTypeFormatter());
            mockedHttpClient
                .Setup(client => client.GetAsync(url))
                .Returns(CreateTask(HttpStatusCode.OK, responseContent));

            return mockedNfieldConnection.Object;
        }

        internal INfieldConnectionClient InitMockClientPut<T1, T2>(Uri url, T1 requestContent, T2 responseObjectContent)
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var responseContent = new ObjectContent<T2>(responseObjectContent, new JsonMediaTypeFormatter());
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(url, requestContent))
                .Returns(CreateTask(HttpStatusCode.OK, responseContent));

            return mockedNfieldConnection.Object;
        }

        internal INfieldConnectionClient InitMockClientPatch<T1, T2>(Uri url, T1 requestContent, T2 responseObjectContent)
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var responseContent = new ObjectContent<T2>(responseObjectContent, new JsonMediaTypeFormatter());
            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(url, requestContent))
                .Returns(CreateTask(HttpStatusCode.OK, responseContent));

            return mockedNfieldConnection.Object;
        }

    }
}

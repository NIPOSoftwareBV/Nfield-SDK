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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Nfield.Infrastructure
{
    /// <summary>
    /// Tests for <see cref="NfieldConnection"/>
    /// </summary>
    public class NfieldConnectionTests
    {
        #region SignInAsync Tests

        [Fact]
        public void TestSignInAsync_CredentialsAreIncorrect_ReturnsFalse()
        {
            var mockedHttpClient = new Mock<INfieldHttpClient>();
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(INfieldHttpClient)))
                .Returns(mockedHttpClient.Object);
            mockedHttpClient
                .Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.BadRequest));

            var target = new NfieldConnection();
            var result = target.SignInAsync("", "", "").Result;

            Assert.False(result);
        }

        [Fact]
        public void TestSignInAsync_CredentialsAreCorrect_ReturnsTrue()
        {
            Uri ServerUri = new Uri(@"http://localhost/");

            const string Domain = "Domain";
            const string Username = "UserName";
            const string Password = "Password";

            var mockedHttpClient = new Mock<INfieldHttpClient>();
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(INfieldHttpClient)))
                .Returns(mockedHttpClient.Object);
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"Domain", Domain},
                    {"Username", Username},
                    {"Password", Password}
                });
            mockedHttpClient
                .Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.BadRequest));
            mockedHttpClient
                .Setup(httpClient => httpClient.PostAsync(ServerUri + @"/SignIn", It.Is<HttpContent>(c => CheckContent(c, content))))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldConnection();
            target.NfieldServerUri = ServerUri;
            var result = target.SignInAsync(Domain, Username, Password).Result;

            Assert.True(result);
        }

        #endregion

        #region GetService Tests

        [Fact]
        public void TestGetService_RequestedServiceTypeIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldConnection();
            Assert.Throws(typeof(ArgumentNullException), () => target.GetService(null));
        }

        [Fact]
        public void TestGetService_ServiceDoesNotExist_ReturnsNull()
        {
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(INfieldConnectionClientObject)))
                .Returns(null);

            var target = new NfieldConnection();
            var result = target.GetService<INfieldConnectionClientObject>();

            Assert.Null(result);
        }

        [Fact]
        public void TestGetService_ServiceExists_ReturnsService()
        {
            var stubbedNfieldConnectionClientObject = new Mock<INfieldConnectionClientObject>().Object;
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(INfieldConnectionClientObject)))
                .Returns(stubbedNfieldConnectionClientObject);

            var target = new NfieldConnection();
            var result = target.GetService<INfieldConnectionClientObject>();

            Assert.Equal(result, stubbedNfieldConnectionClientObject);
        }

        [Fact]
        public void TestGetService_ServiceExistsAndImplementsINfieldConnectionClientObject_CallsInitializeConnectionOnService()
        {
            var mockedNfieldConnectionClientObject = new Mock<INfieldConnectionClientObject>();
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(INfieldConnectionClientObject)))
                .Returns(mockedNfieldConnectionClientObject.Object);

            var target = new NfieldConnection();
            var result = target.GetService<INfieldConnectionClientObject>();

            mockedNfieldConnectionClientObject.Verify(client => client.InitializeNfieldConnection(target));
        }

        #endregion

        #region Dispose Tests

        [Fact]
        public void TestDispose_HasClient_CallsDisposeOnClient()
        {
            var mockedHttpClient = new Mock<INfieldHttpClient>();
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(INfieldHttpClient)))
                .Returns(mockedHttpClient.Object);
            mockedHttpClient
                .Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldConnection();
            var result = target.SignInAsync("", "", "").Result;
            target.Dispose();

            mockedHttpClient.Verify(client => client.Dispose());
        }

        #endregion

        private Task<HttpResponseMessage> CreateTask(HttpStatusCode httpStatusCode)
        {
            return Task.Factory.StartNew(() => new HttpResponseMessage(httpStatusCode));
        }

        private bool CheckContent(HttpContent actual, HttpContent expected)
        {
            var result = actual.Equals(expected);
            var actualAsString = actual.ReadAsStringAsync().Result;
            var expectedAsString = expected.ReadAsStringAsync().Result;
            return actualAsString == expectedAsString;
        }
    }
}

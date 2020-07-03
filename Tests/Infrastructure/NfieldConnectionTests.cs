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
        #region GetService Tests

        [Fact]
        public void TestGetService_RequestedServiceTypeIsNull_ThrowsArgumentNullException()
        {
            var target = NfieldConnectionFactory.Create(new Uri("http://fake/"));
            Assert.Throws<ArgumentNullException>(() => target.GetService(null));
        }

        [Fact]
        public void TestGetService_ServiceDoesNotExist_ReturnsNull()
        {
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(INfieldConnectionClientObject)))
                .Returns(null);

            var target = NfieldConnectionFactory.Create(new Uri("http://fake/"));
            var result = target.GetService<INfieldConnectionClientObject>();

            Assert.Null(result);
        }

        [Fact]
        public void TestGetService_ServiceExists_ReturnsService()
        {
            var stubbedNfieldConnectionClientObject = new Mock<INfieldConnectionClientObject>().Object;
            var stubbedHttpClientObject = new Mock<INfieldHttpClient>().Object;
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(INfieldConnectionClientObject)))
                .Returns(stubbedNfieldConnectionClientObject);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(INfieldHttpClient)))
                .Returns(stubbedHttpClientObject);
            var target = NfieldConnectionFactory.Create(new Uri("http://fake/"));
            var result = target.GetService<INfieldConnectionClientObject>();

            Assert.Equal(result, stubbedNfieldConnectionClientObject);
        }

        [Fact]
        public void TestGetService_ServiceExistsAndImplementsINfieldConnectionClientObject_CallsInitializeConnectionOnService()
        {
            var mockedNfieldConnectionClientObject = new Mock<INfieldConnectionClientObject>();
            var stubbedHttpClientObject = new Mock<INfieldHttpClient>().Object;
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(INfieldConnectionClientObject)))
                .Returns(mockedNfieldConnectionClientObject.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(INfieldHttpClient)))
                .Returns(stubbedHttpClientObject);
            var target = (NfieldConnection)NfieldConnectionFactory.Create(new Uri("http://fake/"));
            var result = target.GetService<INfieldConnectionClientObject>();

            mockedNfieldConnectionClientObject.Verify(client => client.InitializeNfieldConnection(target));
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

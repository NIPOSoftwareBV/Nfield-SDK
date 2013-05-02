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
using System.Net.Http.Formatting;
using Nfield.Exceptions;
using Nfield.Infrastructure.NipoSoftware.Nfield.Manager.Api.Helpers;
using Xunit;

namespace Nfield.Extensions
{
    /// <summary>
    /// Tests for <see cref="HttpResponseMessageExtensions"/>
    /// </summary>
    public class HttpResponseMessageExtensionsTests
    {
        [Fact]
        public void TestValidateResponse_ServerReturnsOnlyHttpStatusCode_ThrowsNfieldHttpResponseException()
        {
            // Arrange
            const HttpStatusCode httpStatusCode = HttpStatusCode.NotFound;
            var serverResponse = new HttpResponseMessage(httpStatusCode);

            // Act
            Assert.ThrowsDelegate actCode = () => serverResponse.ValidateResponse();

            // Assert
            var ex = Assert.Throws<NfieldHttpResponseException>(actCode);
            Assert.Equal(ex.HttpStatusCode, httpStatusCode);
        }

        [Fact]
        public void TestValidateResponse_ServerReturnsNfieldErrorCodeAndMessage_ThrowsNfieldErrorExceptionWithErrorCodeAndMessage()
        {
            // Arrange
            const HttpStatusCode httpStatusCode = HttpStatusCode.NotFound;
            const NfieldErrorCode nfieldErrorCode = NfieldErrorCode.ArgumentIsNull;
            const string message = "Message #1";

            var serverResponse = new HttpResponseMessage(httpStatusCode);
            var httpErrorMock = new Dictionary<string, object>()
                {
                    { "NfieldErrorCode", nfieldErrorCode },
                    { "Message", message }
                };
            HttpContent content = new ObjectContent<Dictionary<string, object>>(httpErrorMock, new JsonMediaTypeFormatter());
            serverResponse.Content = content;

            // Act
            Assert.ThrowsDelegate actCode = () => serverResponse.ValidateResponse();

            // Assert
            var ex = Assert.Throws<NfieldErrorException>(actCode);
            Assert.Equal(ex.HttpStatusCode, httpStatusCode);
            Assert.Equal(ex.NfieldErrorCode, nfieldErrorCode);
            Assert.Equal(ex.Message, message);
        }

        [Fact]
        public void TestValidateResponse_ServerReturnsSucces_DontThrow()
        {
            // Arrange
            const HttpStatusCode httpStatusCode = HttpStatusCode.OK; // Succes
            var serverResponse = new HttpResponseMessage(httpStatusCode);

            // Act
            Assert.ThrowsDelegate actCode = () => serverResponse.ValidateResponse();

            // Assert
            Assert.DoesNotThrow(actCode);
        }
    }
}

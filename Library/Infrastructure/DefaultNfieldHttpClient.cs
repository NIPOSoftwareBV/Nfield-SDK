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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Nfield.Extensions;

namespace Nfield.Infrastructure
{
    /// <summary>
    /// A wrapper around <see cref="HttpClient"/> that also adds an authentication header to the response
    /// if it was received in the request.
    /// </summary>
    internal sealed class DefaultNfieldHttpClient : NfieldHttpClientBase
    {
        private string _token;

        public DefaultNfieldHttpClient(HttpClient client) : base(client)
        { }

        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            if (_token != null) // always null for the first request (SignIn)
            {
                // set the header on the client, so our consumers (who may have provided it)
                // can benefit from the header.
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _token);
            }

            var response = await Client.SendAsync(request).ConfigureAwait(false);

            IEnumerable<string> headerValues;
            if (response.Headers.TryGetValues("X-AuthenticationToken", out headerValues))
            {
                // auth token may have been 'null', or may have been refreshed on the server, so set it always
                _token = headerValues.First();
            }

            return response.ValidateResponse();
        }
    }
}

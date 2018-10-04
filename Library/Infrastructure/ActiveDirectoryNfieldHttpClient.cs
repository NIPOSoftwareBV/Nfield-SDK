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


using Nfield.Exceptions;
using Nfield.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nfield.Infrastructure
{
    internal sealed class BearerTokenNfieldHttpClient : NfieldHttpClientBase
    {
        private readonly string _domainName;
        private readonly Func<Task<string>> _provideTokenAsync;

        public BearerTokenNfieldHttpClient(HttpClient client, string domainName, Func<Task<string>> provideTokenAsync)
            : base(client)
        {
            _domainName = domainName;
            _provideTokenAsync = provideTokenAsync;
        }

        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var token = await _provideTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("X-Nfield-Domain", _domainName);

            var response = await Client.SendAsync(request);

            try
            {
                return response.ValidateResponse();
            }
            catch (NfieldHttpResponseException ex)
            {
                if (ex.Message.Contains("X-NFIELD-DOMAIN"))
                {
                    // message is not very useful in the SDK case
                    throw new NfieldHttpResponseException(ex.HttpStatusCode, $"Invalid domain '{_domainName}'");
                }

                throw;
            }
        }
    }
}

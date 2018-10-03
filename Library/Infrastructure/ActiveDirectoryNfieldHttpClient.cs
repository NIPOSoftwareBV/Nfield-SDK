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


using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nfield.Infrastructure
{
    internal sealed class BearerTokenNfieldHttpClient : NfieldHttpClientBase
    {
        private readonly string _domainName;
        private readonly string _token;

        public BearerTokenNfieldHttpClient(string domainName, string token)
        {
            _domainName = domainName;
            _token = token;
        }

        public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            request.Headers.Add("X-Nfield-Domain", _domainName);

            return Client.SendAsync(request);
        }
    }
}

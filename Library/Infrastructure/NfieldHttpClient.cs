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
using Newtonsoft.Json;
using System.Text;
using Nfield.Extensions;

namespace Nfield.Infrastructure
{
    /// <summary>
    /// A wrapper around <see cref="HttpClient"/> that also adds an authentication header to the response
    /// if it was received in the request.
    /// </summary>
    internal sealed class NfieldHttpClient : INfieldHttpClient
    {

        #region INfieldHttpClient Members

        public string AuthToken { get; set; }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return SendAndHandleAuthToken(client => client.SendAsync(request));
        }


        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return SendAndHandleAuthToken(client => client.PostAsync(requestUri, content));
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return SendAndHandleAuthToken(client => client.GetAsync(requestUri));
        }

        public Task<HttpResponseMessage> PostAsJsonAsync<TContent>(string requestUri, TContent content)
        {
            return SendAndHandleAuthToken(client => client.PostAsJsonAsync(requestUri, content));
        }

        public Task<HttpResponseMessage> PutAsJsonAsync<TContent>(string requestUri, TContent content)
        {
            return SendAndHandleAuthToken(client => client.PutAsJsonAsync(requestUri, content));
        }

        public Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return SendAndHandleAuthToken(client => client.DeleteAsync(requestUri));
        }

        public Task<HttpResponseMessage> DeleteAsJsonAsync<TContent>(string requestUri, TContent content)
        {
            var request = new HttpRequestMessage(new HttpMethod("DELETE"), requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json")
            };

            return SendAndHandleAuthToken(client => client.SendAsync(request));
        }

        public Task<HttpResponseMessage> PatchAsJsonAsync<TContent>(string requestUri, TContent content)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json")
            };
            return SendAndHandleAuthToken(client => client.SendAsync(request));
        }

        public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            return SendAndHandleAuthToken(client => client.PutAsync(requestUri, content));
        }

        #endregion

        private Task<HttpResponseMessage> SendAndHandleAuthToken(Func<HttpClient, Task<HttpResponseMessage>> call)
        {

            var request = new TaskFactory().StartNew(() =>
            {
                using (var client = new HttpClient())
                {
                    if (AuthToken != null)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", AuthToken);
                    }

                    var response = call(client).Result;

                    IEnumerable<string> headerValues;
                    if (response.Headers.TryGetValues("X-AuthenticationToken", out headerValues))
                    {
                        // auth token may have been 'null', or may have been refreshed on the server, so set it always
                        AuthToken = headerValues.First();
                    }

                    return response.ValidateResponse();
                }

            });

            return request;
        }
    }
}

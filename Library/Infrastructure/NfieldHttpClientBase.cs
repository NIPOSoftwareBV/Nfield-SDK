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
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace Nfield.Infrastructure
{
    internal abstract class NfieldHttpClientBase : INfieldHttpClient
    {
        protected HttpClient Client { get; }

        public NfieldHttpClientBase()
        {
            Client = new HttpClient();
        }

        public Task<HttpResponseMessage> DeleteAsJsonAsync<TContent>(string requestUri, TContent content)
            => SendAsync(new HttpRequestMessage(HttpMethod.Put, requestUri) { Content = GetHttpContentForValue(content) });

        public Task<HttpResponseMessage> DeleteAsync(string requestUri)
            => SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri));

        public Task<HttpResponseMessage> GetAsync(string requestUri)
            => SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri));

        public Task<HttpResponseMessage> PatchAsJsonAsync<TContent>(string requestUri, TContent content)
            => SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = GetHttpContentForValue(content) });

        public Task<HttpResponseMessage> PostAsJsonAsync<TContent>(string requestUri, TContent content)
            => SendAsync(new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = GetHttpContentForValue(content) });

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
            => SendAsync(new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = content });

        public Task<HttpResponseMessage> PutAsJsonAsync<TContent>(string requestUri, TContent content)
            => SendAsync(new HttpRequestMessage(HttpMethod.Put, requestUri) { Content = GetHttpContentForValue(content) });

        public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
            => SendAsync(new HttpRequestMessage(HttpMethod.Put, requestUri) { Content = content });

        public abstract Task<HttpResponseMessage> SendAsync(HttpRequestMessage message);

        public void Dispose() => Client.Dispose();

        private HttpContent GetHttpContentForValue<TContent>(TContent content)
        {
            return new StringContent(
                JsonConvert.SerializeObject(content, Formatting.None),
                Encoding.UTF8,
                "application/json"
            );
        }
    }
}

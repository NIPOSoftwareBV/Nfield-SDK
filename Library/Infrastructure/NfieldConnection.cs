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
using Nfield.Extensions;

namespace Nfield.Infrastructure
{
    internal class NfieldConnection : INfieldConnectionV2, INfieldConnectionClient
    {
        private readonly HttpClient _httpClient;
        public INfieldHttpClient Client { get; internal /* for tests */ set; }

        public NfieldConnection(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Implementation of IServiceProvider

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <returns>
        /// A service object of type <paramref name="serviceType"/>.-or- null if there is no service object of type <paramref name="serviceType"/>.
        /// </returns>
        /// <param name="serviceType">An object that specifies the type of service object to get. </param>
        public object GetService(Type serviceType)
        {
            if (serviceType == null) {
                throw new ArgumentNullException("serviceType");
            }

            var serviceInstance = DependencyResolver.Current.Resolve(serviceType);

            var nfieldConnectionClientObject = serviceInstance as INfieldConnectionClientObject;
            
            if (nfieldConnectionClientObject == null) return serviceInstance;
            
            nfieldConnectionClientObject.InitializeNfieldConnection(this);

            return serviceInstance;
        }

        #endregion

        #region Implementation of INfieldConnection

        public Uri NfieldServerUri { get; internal set; }

        /// <summary>
        /// Sign into the specified domain, using the specified username and password
        /// </summary>
        /// <returns><c>true</c> if sign-in was successful, <c>false</c> otherwise.</returns>
        public Task<bool> SignInAsync(string domainName, string username, string password)
        {
            Client = new DefaultNfieldHttpClient(_httpClient);

            var data = new Dictionary<string, string>
                {
                    {"Domain", domainName},
                    {"Username", username},
                    {"Password", password}
                };
            var content = new FormUrlEncodedContent(data);

            // client will update the Token
            return Client.PostAsync(new Uri(NfieldServerUri, "SignIn"), content)
                .ContinueWith(responseMessageTask =>
                {
                    var result = responseMessageTask.Result;
                    return result.StatusCode == HttpStatusCode.OK;
                }).FlattenExceptions();
        }

        public void RegisterTokenProvider(string domainName, Func<Task<string>> provideTokenAsync)
        {
            Client = new BearerTokenNfieldHttpClient(_httpClient, domainName, provideTokenAsync);
        }

        /// <summary>
        /// Return the specified service <typeparamref name="TService"/> provided by Nfield.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns>An implementation of the specified service.</returns>
        public TService GetService<TService>()
        {
            return (TService)GetService(typeof(TService));
        }

        #endregion

        #region Implementation of IDisposable

        ~NfieldConnection()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing || Client == null)
                return;

            
            Client = null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion
    }

}
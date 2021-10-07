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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Utilities;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldDomainResponseCodesService"/>
    /// </summary>
    internal class NfieldDomainResponseCodesService : INfieldDomainResponseCodesService, INfieldConnectionClientObject
    {

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }



        /// <summary>
        /// <see cref="INfieldDomainResponseCodesService.QueryAsync(string)"/>
        /// </summary>
        public Task<IQueryable<DomainResponseCode>> QueryAsync(string domainId)
        {
            Ensure.ArgumentNotNullOrEmptyString(domainId, nameof(domainId));

            var uri = DomainResponseCodeUrl(domainId, null);

            return Client.GetAsync(uri)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(
                    stringTask => JsonConvert.DeserializeObject<List<DomainResponseCode>>(stringTask.Result).AsQueryable())
                .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldDomainResponseCodesService.QueryAsync(string, int)"/>
        /// </summary>
        public Task<DomainResponseCode> QueryAsync(string domainId, int code)
        {
            Ensure.ArgumentNotNullOrEmptyString(domainId, nameof(domainId));

            return
                Client.GetAsync(DomainResponseCodeUrl(domainId, code))
                    .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                    .ContinueWith(
                        stringTask => JsonConvert.DeserializeObject<DomainResponseCode>(stringTask.Result))
                    .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldDomainResponseCodesService.AddAsync"/>
        /// </summary>
        public Task<DomainResponseCode> AddAsync(string domainId, DomainResponseCode responseCode)
        {
            Ensure.ArgumentNotNullOrEmptyString(domainId, nameof(domainId));
            Ensure.ArgumentNotNull(responseCode, nameof(responseCode));

            var uri = DomainResponseCodeUrl(domainId, null);

            return Client.PostAsJsonAsync(uri, responseCode)
                .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(task => JsonConvert.DeserializeObject<DomainResponseCode>(task.Result))
                .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldDomainResponseCodesService.UpdateAsync"/>
        /// </summary>
        public Task<DomainResponseCode> UpdateAsync(string domainId, int responseCodeId, DomainResponseCodeData responseCodeData)
        {
            Ensure.ArgumentNotNullOrEmptyString(domainId, nameof(domainId));
            Ensure.ArgumentNotNull(responseCodeData, nameof(responseCodeData));          

            return
                Client.PatchAsJsonAsync(DomainResponseCodeUrl(domainId, responseCodeId), responseCodeData)
                    .ContinueWith(
                        responseMessageTask =>
                            responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                    .ContinueWith(
                        stringTask => JsonConvert.DeserializeObject<DomainResponseCode>(stringTask.Result))
                    .FlattenExceptions();
        }


        /// <summary>
        /// <see cref="INfieldDomainResponseCodesService.RemoveAsync"/>
        /// </summary>
        public Task RemoveAsync(string domainId, int code)
        {
            if (string.IsNullOrEmpty(domainId))
            {
                throw new ArgumentNullException("domainId");
            }

            return
                Client.DeleteAsync(DomainResponseCodeUrl(domainId, code))
                      .FlattenExceptions();
        }

        #region Implementation of INfieldConnectionClientObject

        /// <summary>
        /// <see cref="INfieldConnectionClientObject.ConnectionClient"/>
        /// </summary>
        public INfieldConnectionClient ConnectionClient { get; internal set; }

        /// <summary>
        /// <see cref="INfieldConnectionClientObject.InitializeNfieldConnection"/>
        /// </summary>
        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        /// <summary>
        /// Constructs and returns the url for domain response code 
        /// based on supplied <paramref name="domainId"/>  and <paramref name="code"/>
        /// </summary>
        private Uri DomainResponseCodeUrl(string domainId, int? code)
        {
            var codeString = code.HasValue ? code.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            return new Uri(ConnectionClient.NfieldServerUri, $"Domains/{domainId}/ResponseCodes/{codeString}");
        }
    }   
}
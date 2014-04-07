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
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldAddressesService"/>
    /// </summary>
    internal class NfieldAddressesService : INfieldAddressesService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldLanguagesService

        /// <summary>
        /// See <see cref="INfieldAddressesService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<Address>> QueryAsync(string surveyId, string samplingPointId)
        {
            CheckSurveyIdAndSamplingPointId(surveyId, samplingPointId);

            return Client.GetAsync(AdressesApi(surveyId, samplingPointId,  null).AbsoluteUri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<Address>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldAddressesService.AddAsync"/>
        /// </summary>
        public Task<Address> AddAsync(string surveyId, string samplingPointId, Address address)
        {
            CheckSurveyIdAndSamplingPointId(surveyId, samplingPointId);

            return Client.PostAsJsonAsync(AdressesApi(surveyId, samplingPointId, null).AbsoluteUri, address)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObjectAsync<Address>(task.Result).Result)
                         .FlattenExceptions();
        }

        public Task DeleteAsync(string surveyId, string samplingPointId, string addressId)
        {
            CheckSurveyIdAndSamplingPointId(surveyId, samplingPointId);
            if (addressId == null)
                throw new ArgumentNullException("addressId");
            if (addressId.Trim().Length == 0)
                throw new ArgumentException("addressId cannot be empty");

            var uri = AdressesApi(surveyId, samplingPointId, addressId).AbsoluteUri;

            return Client.DeleteAsync(uri).FlattenExceptions();
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private static void CheckSurveyIdAndSamplingPointId(string surveyId, string samplingPointId)
        {
            if (surveyId == null)
                throw new ArgumentNullException("surveyId");
            if (surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be empty");
            if (samplingPointId == null)
                throw new ArgumentNullException("samplingPointId");
            if (samplingPointId.Trim().Length == 0)
                throw new ArgumentException("samplingPointId cannot be empty");
        }

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri AdressesApi(string surveyId, string samplingPointId, string id)
        {
            StringBuilder uriText = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            uriText.AppendFormat("Surveys/{0}/SamplingPoints/{1}/Addresses",
                surveyId, samplingPointId);
            if (!string.IsNullOrEmpty(id))
                uriText.AppendFormat("/{0}", id);
            return new Uri(uriText.ToString());
        }
    }

    internal class UpdateAddress
    {
        public string Details { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }
}

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
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldFieldworkOfficesService"/>
    /// </summary>
    internal class NfieldFieldworkOfficesService : INfieldFieldworkOfficesService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldFieldworkOfficesService

        /// <summary>
        /// See <see cref="INfieldFieldworkOfficesService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<FieldworkOffice>> QueryAsync()
        {
            return ConnectionClient.Client.GetAsync(OfficesApi.AbsoluteUri)
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask =>
                 JsonConvert.DeserializeObject<List<FieldworkOffice>>(stringTask.Result).AsQueryable())
             .FlattenExceptions();
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private Uri OfficesApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri.AbsoluteUri + "offices/"); }
        }

    }
}

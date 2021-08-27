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
            return ConnectionClient.Client.GetAsync(OfficesApi)
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask =>
                 JsonConvert.DeserializeObject<List<FieldworkOffice>>(stringTask.Result).AsQueryable())
             .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldFieldworkOfficesService.AddAsync"/>
        /// </summary>
        public Task<FieldworkOffice> AddAsync(FieldworkOffice office)
        {
            if (office == null)
            {
                throw new ArgumentNullException(nameof(office));
            }

            if (!string.IsNullOrEmpty(office.OfficeId))
            {
                throw new ArgumentException(nameof(office.OfficeId));
            }

            return ConnectionClient.Client.PostAsJsonAsync(OfficesApi, office)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<FieldworkOffice>(task.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldFieldworkOfficesService.RemoveAsync"/>
        /// </summary>
        public Task RemoveAsync(FieldworkOffice office)
        {
            if (office == null)
            {
                throw new ArgumentNullException(nameof(office));
            }

            return
                ConnectionClient.Client.DeleteAsync(new Uri(OfficesApi, office.OfficeId))
                      .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldFieldworkOfficesService.UpdateAsync"/>
        /// </summary>
        public Task<FieldworkOffice> UpdateAsync(FieldworkOffice office)
        {
            if (office == null)
            {
                throw new ArgumentNullException(nameof(office));
            }

            if (string.IsNullOrEmpty(office.OfficeId))
            {
                throw new ArgumentException(nameof(office.OfficeId));
            }

            var updatedOffice = new UpdateOffice
            {
                OfficeName = office.OfficeName,
                Description = office.Description
            };

            return ConnectionClient.Client.PatchAsJsonAsync(new Uri(OfficesApi, office.OfficeId), updatedOffice)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask => JsonConvert.DeserializeObject<FieldworkOffice>(stringTask.Result))
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
            get { return new Uri(ConnectionClient.NfieldServerUri, "offices/"); }
        }

        internal class UpdateOffice
        {
            public string OfficeName { get; set; }
            public string Description { get; set; }
        }

    }
}

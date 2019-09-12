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

using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldCatiInterviewersService"/>
    /// </summary>
    internal class NfieldCatiInterviewersService : INfieldCatiInterviewersService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldCatiInterviewersService

        /// <summary>
        /// See <see cref="INfieldCatiInterviewersService.AddAsync"/>
        /// </summary>
        public async Task<CatiInterviewer> AddAsync(CatiInterviewer catiInterviewer)
        {
            using (var response = await Client.PostAsJsonAsync(CatiInterviewersApi, catiInterviewer))
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CatiInterviewer>(result);
            }
        }

        /// <summary>
        /// See <see cref="INfieldCatiInterviewersService.RemoveAsync"/>
        /// </summary>
        public async Task RemoveAsync(CatiInterviewer catiInterviewer)
        {
            if (catiInterviewer == null)
            {
                throw new ArgumentNullException("catiInterviewer");
            }

            using (await Client.DeleteAsync(new Uri(CatiInterviewersApi, catiInterviewer.InterviewerId)))
            {
            }
        }

        /// <summary>
        /// See <see cref="INfieldCatiInterviewersService.QueryAsync"/>
        /// </summary>
        public async Task<IQueryable<CatiInterviewer>> QueryAsync()
        {
            using (var response = await Client.GetAsync(CatiInterviewersApi))
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CatiInterviewer>>(result).AsQueryable();
            }
        }

        /// <summary>
        /// See <see cref="INfieldCatiInterviewersService.ChangePasswordAsync"/>
        /// </summary>
        public async Task<CatiInterviewer> ChangePasswordAsync(CatiInterviewer catiInterviewer, string password)
        {
            if (catiInterviewer == null)
            {
                throw new ArgumentNullException("catiInterviewer");
            }
            using (var response = await Client.PutAsJsonAsync(new Uri(CatiInterviewersApi, catiInterviewer.InterviewerId), (object)new { Password = password }))
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CatiInterviewer>(result);
            }
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; private set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri CatiInterviewersApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri, "catiinterviewers/"); }
        }
    }
}

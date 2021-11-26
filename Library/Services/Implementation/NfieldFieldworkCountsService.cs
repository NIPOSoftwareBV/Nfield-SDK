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
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldFieldworkCountsService"/>
    /// </summary>
    internal class NfieldFieldworkCountsService : INfieldFieldworkCountsService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldFieldworkCountsService

        /// <summary>
        /// See <see cref="INfieldFieldworkCountsService.GetAsync(string)"/>
        /// </summary>
        public Task<FieldworkCounts> GetAsync(string surveyId)
        {
            return ConnectionClient.Client.GetAsync(FieldworkCountsApi(surveyId))
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask =>
                 JsonConvert.DeserializeObject<FieldworkCounts>(stringTask.Result))
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

        private Uri FieldworkCountsApi(string surveyId)
        {
             return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/FieldworkCounts"); 
        }
    }
}

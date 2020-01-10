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
    /// Implementation of <see cref="INfieldSurveyResourcesService"/>
    /// </summary>
    internal class NfieldSurveyResourcesService : INfieldSurveyResourcesService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyResourcesService

        private Uri SurveyResourcesApi => new Uri(ConnectionClient.NfieldServerUri, "SurveyResources/");

        /// <summary>
        /// See <see cref="INfieldSurveyResourcesService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<SurveyResource>> QueryAsync()
        {
            return Client.GetAsync(SurveyResourcesApi)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<SurveyResource>>(stringTask.Result).AsQueryable())
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

        private INfieldHttpClient Client => ConnectionClient.Client;
    }
  
}
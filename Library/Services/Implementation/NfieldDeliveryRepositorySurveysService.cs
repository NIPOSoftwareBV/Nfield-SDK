﻿//    This file is part of Nfield.SDK.
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
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.SDK.Models.Delivery;
using Nfield.Services;

namespace Nfield.SDK.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldDeliveryRepositorySurveysService"/>
    /// </summary>
    internal class NfieldDeliveryRepositorySurveysService : INfieldDeliveryRepositorySurveysService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldDeliveryRepositorySurveysService

        public Task<IQueryable<RepositorySurveyModel>> QueryAsync(long repositoryId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}/Surveys");

            return ConnectionClient.Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<RepositorySurveyModel>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        public Task PostAsync(long repositoryId, string[] nfieldSurveyIds)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}/Surveys");

            return ConnectionClient.Client.PostAsJsonAsync(uri, nfieldSurveyIds)
                         .FlattenExceptions();          
        }

        public Task PutReinitiateAsync(long repositoryId, string surveyId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}/Surveys/{surveyId}/reinitiate");
            return ConnectionClient.Client.PutAsync(uri, null)
                        .FlattenExceptions();
        }

        public Task DeleteAsync(long repositoryId, string surveyId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Repositories/{repositoryId}/Surveys/{surveyId}");

            return ConnectionClient.Client.DeleteAsync(uri).FlattenExceptions();
        }


        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }        

        #endregion

    }
}

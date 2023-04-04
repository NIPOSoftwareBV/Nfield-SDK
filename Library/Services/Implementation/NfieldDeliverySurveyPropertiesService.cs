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
using Nfield.SDK.Models.Delivery;
using Nfield.Services;

namespace Nfield.SDK.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldDeliverySurveyPropertiesService"/>
    /// </summary>
    internal class NfieldDeliverySurveyPropertiesService : INfieldDeliverySurveyPropertiesService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldDeliverySurveyPropertiesService

        public Task<IQueryable<DomainSurveyPropertyModel>> QueryAsync(string surveyId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Surveys/{surveyId}/Properties");

            return ConnectionClient.Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<DomainSurveyPropertyModel>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        public Task<DomainSurveyPropertyModel> GetByIdAsync(string surveyId, long propertyId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Surveys/{surveyId}/Properties/{propertyId}");

            return ConnectionClient.Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<DomainSurveyPropertyModel>(stringTask.Result))
                         .FlattenExceptions();
        }

        public Task PostAsync(string surveyId, CreateDomainSurveyPropertyModel model)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Surveys/{surveyId}/Properties");

            return ConnectionClient.Client.PostAsJsonAsync(uri, model)
                         //.ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         //.ContinueWith(task => JsonConvert.DeserializeObject<CreateDomainSurveyPropertyModel>(task.Result))
                         .FlattenExceptions();
        }

        public Task PutSurveyPropertyAsync(string surveyId, long propertyId, UpdateDomainSurveyPropertyModel model)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Surveys/{surveyId}/Properties/{propertyId}");

            return ConnectionClient.Client.PutAsJsonAsync(uri, model)
                         //.ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         //.ContinueWith(task => JsonConvert.DeserializeObject<UpdateDomainSurveyPropertyModel>(task.Result))
                         .FlattenExceptions();
        }

        public Task DeleteAsync(string surveyId, long propertyId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Delivery/Surveys/{surveyId}/Properties/{propertyId}");

            return ConnectionClient.Client.DeleteAsync(uri).FlattenExceptions();
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        #endregion

    }
}

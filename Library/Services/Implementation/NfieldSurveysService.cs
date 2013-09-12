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
    /// Implementation of <see cref="INfieldSurveysService"/>
    /// </summary>
    internal class NfieldSurveysService : INfieldSurveysService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveysService

        /// <summary>
        /// See <see cref="INfieldSurveysService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<Survey>> QueryAsync()
        {
            return Client.GetAsync(SurveysApi.AbsoluteUri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<Survey>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.SamplingPointsQueryAsync"/>
        /// </summary>
        public Task<IQueryable<SamplingPoint>> SamplingPointsQueryAsync(string surveyId)
        {
            string uri = string.Format(@"{0}/{1}/{2}", SurveysApi.AbsoluteUri, surveyId, SamplingPointsControllerName);
            
            return Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<SamplingPoint>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.SamplingPointQueryAsync"/>
        /// </summary>
        public Task<SamplingPoint> SamplingPointQueryAsync(string surveyId, string samplingPointId)
        {
            string uri = string.Format(@"{0}/{1}/{2}/{3}", SurveysApi.AbsoluteUri, surveyId, SamplingPointsControllerName, samplingPointId);
            
            return Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<SamplingPoint>(stringTask.Result))
                         .FlattenExceptions();            
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.SamplingPointUpdateAsync"/>
        /// </summary>
        public Task<SamplingPoint> SamplingPointUpdateAsync(string surveyId, SamplingPoint samplingPoint)
        {
            var updatedSamplingPoint = new UpdateSamplingPoint
            {
                Name = samplingPoint.Name,
                Description = samplingPoint.Description,
                FieldworkOfficeId = samplingPoint.FieldworkOfficeId
            };

            string uri = string.Format(@"{0}/{1}/{2}/{3}", SurveysApi.AbsoluteUri, surveyId, SamplingPointsControllerName, samplingPoint.SamplingPointId);

            return Client.PatchAsJsonAsync(uri, updatedSamplingPoint)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask => JsonConvert.DeserializeObjectAsync<SamplingPoint>(stringTask.Result).Result)
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.SamplingPointAddAsync"/>
        /// </summary>
        public Task<SamplingPoint> SamplingPointAddAsync(string surveyId, SamplingPoint samplingPoint)
        {
            string uri = string.Format(@"{0}/{1}/{2}/{3}", SurveysApi.AbsoluteUri, surveyId, SamplingPointsControllerName, samplingPoint.SamplingPointId);
            return Client.PostAsJsonAsync(uri, samplingPoint)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObjectAsync<SamplingPoint>(task.Result).Result)
                         .FlattenExceptions();            
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.SamplingPointDeleteAsync"/>
        /// </summary>
        public Task SamplingPointDeleteAsync(string surveyId, SamplingPoint samplingPoint)
        {
            string uri = string.Format(@"{0}/{1}/{2}/{3}", SurveysApi.AbsoluteUri, surveyId, SamplingPointsControllerName, samplingPoint.SamplingPointId);
            return Client.DeleteAsync(uri)
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

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private static string SamplingPointsControllerName
        {
            get { return "samplingpoints";  }
        }

        private Uri SurveysApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri.AbsoluteUri + @"/surveys"); }
        }

        private Uri SamplingPointsApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri.AbsoluteUri + @"/" + SamplingPointsControllerName); }
        }
    }

    /// <summary>
    /// Update model for a sampling point
    /// Instruction is not allowed to be updated (because this is a link to a pdf in blob storage)
    /// </summary>
    internal class UpdateSamplingPoint
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FieldworkOfficeId { get; set; }
    }

}

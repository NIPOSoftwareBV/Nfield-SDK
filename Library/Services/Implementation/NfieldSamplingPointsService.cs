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
    internal class NfieldSamplingPointsService : INfieldSamplingPointsService, INfieldConnectionClientObject
    {
        #region INfieldSamplingPointsService Members

        /// <summary>
        /// See <see cref="INfieldSamplingPointsService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<SamplingPoint>> QueryAsync(string surveyId)
        {
            CheckSurveyId(surveyId);

            return Client.GetAsync(SamplingPointsApi(surveyId))
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<SamplingPoint>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }
        /// <summary>
        /// See <see cref="INfieldSamplingPointsService.GetAsync(string, string)"/>
        /// </summary>
        public Task<SamplingPoint> GetAsync(string surveyId, string SamplingPointId)
        {
            return Client.GetAsync(SamplingPointsApi(surveyId, SamplingPointId))
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<SamplingPoint>(stringTask.Result))
                         .FlattenExceptions();
        }
        /// <summary>
        /// See <see cref="INfieldSamplingPointsService.RemoveAsync(string, string)"/>
        /// </summary>
        public Task<bool> RemoveAsync(string surveyId, string SamplingPointId)
        {
            return Client.DeleteAsync(SamplingPointsApi(surveyId, SamplingPointId))                         
                                       .FlattenExceptions()
                                       .ContinueWith(t => t.Result.StatusCode == System.Net.HttpStatusCode.OK);
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.UpdateAsync"/>
        /// </summary>
        public Task<SamplingPoint> UpdateAsync(string surveyId, SamplingPoint samplingPoint)
        {
            if (samplingPoint?.SamplingPointId == null)
            {
                throw new ArgumentNullException("samplingPoint");
            }           

            return Client.PatchAsJsonAsync(SamplingPointsApi(surveyId, samplingPoint.SamplingPointId), samplingPoint)
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask => JsonConvert.DeserializeObject<SamplingPoint>(stringTask.Result))
             .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveysService.UpdateAsync"/>
        /// </summary>
        public Task<SamplingPoint> CreateAsync(string surveyId, SamplingPoint samplingPoint)
        {
            if (samplingPoint == null)
            {
                throw new ArgumentNullException("samplingPoint");
            }

            return Client.PostAsJsonAsync(SamplingPointsApi(surveyId), samplingPoint)
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask => JsonConvert.DeserializeObject<SamplingPoint>(stringTask.Result))
             .FlattenExceptions();
        }

        public Task<SamplingPoint> ActivateAsync(string surveyId, string samplingPointId, int? target)
        {
            return Client.PatchAsJsonAsync(SamplingPointsApi(surveyId, samplingPointId, activate:true), new { Target = target})
            .ContinueWith(
                responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
            .ContinueWith(
                stringTask => JsonConvert.DeserializeObject<SamplingPoint>(stringTask.Result))
            .FlattenExceptions();
        }

        public Task<bool> ActivateAsync(string surveyId, IEnumerable<string> samplingPointIds)
        {
            return Client.PostAsJsonAsync(SamplingPointsApi(surveyId, activate:true),
                new { SamplingPointIds = samplingPointIds.ToArray() })     
            .FlattenExceptions()
            .ContinueWith(t => t.Result.StatusCode == System.Net.HttpStatusCode.OK);
        }

        public Task<SamplingPoint> ReplaceAsync(string surveyId, string samplingPointId, string newSamplingPointId, int? target)
        {
            return Client.PatchAsJsonAsync(SamplingPointsApi(surveyId, samplingPointId, replace:true),
                new { SpareSamplingPointId = newSamplingPointId, Target = target })
           .ContinueWith(
               responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
           .ContinueWith(
               stringTask => JsonConvert.DeserializeObject<SamplingPoint>(stringTask.Result))
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

        private static void CheckSurveyId(string surveyId)
        {
            if (surveyId == null)
                throw new ArgumentNullException("surveyId");
            if (surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be empty");
        }

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri SamplingPointsApi(string surveyId, string samplingPointId = null, bool activate = false, bool replace = false)
        {
            var path = new StringBuilder();
            path.AppendFormat("Surveys/{0}", surveyId);
            if (activate && samplingPointId == null)
            {
                path.Append("/ActivateSamplingPoints");
            }
            else
            {
                path.Append("/SamplingPoints");
                if (!string.IsNullOrEmpty(samplingPointId))
                    path.AppendFormat("/{0}", samplingPointId);
                if (activate)
                    path.Append("/Activate");
                else if (replace)
                    path.Append("/Replace");
            }

            return new Uri(ConnectionClient.NfieldServerUri, path.ToString());
        }
        
    }
}

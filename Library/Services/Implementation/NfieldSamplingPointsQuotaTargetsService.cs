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


namespace Nfield.SDK.Services.Implementation
{
    internal class NfieldSamplingPointsQuotaTargetsService : INfieldSamplingPointsQuotaTargetsService, INfieldConnectionClientObject
    {

        /// <summary>
        /// See <see cref="INfieldSamplingPointsQuotaTargetsService.GetAsync"/>
        /// </summary>
        public Task<SamplingPointQuotaTarget> GetAsync(string surveyId, string samplingPointId, string quotaLevelId)
        {
            return ConnectionClient.Client.GetAsync(SamplingPointsQuotaTargetsApi(surveyId, samplingPointId, quotaLevelId))
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask =>
                 JsonConvert.DeserializeObject<SamplingPointQuotaTarget>(stringTask.Result))
             .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSamplingPointsQuotaTargetsService.GetAsync"/>
        /// </summary>
        public Task<IQueryable<SamplingPointQuotaTarget>> QueryAsync(string surveyId, string samplingPointId)
        {
            return ConnectionClient.Client.GetAsync(SamplingPointsQuotaTargetsApi(surveyId, samplingPointId))
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask =>
                 JsonConvert.DeserializeObject<List<SamplingPointQuotaTarget>>(stringTask.Result).AsQueryable())
             .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSamplingPointsQuotaTargetsService.PatchAsync(string, string, string, int?)"/>
        /// </summary>
        public Task<SamplingPointQuotaTarget> PatchAsync(string surveyId, string samplingPointId, string quotaLevelId, int? target)
        {
            return ConnectionClient.Client.PatchAsJsonAsync(SamplingPointsQuotaTargetsApi(surveyId, samplingPointId, quotaLevelId), new { Target = target })
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask =>
                 JsonConvert.DeserializeObject<SamplingPointQuotaTarget>(stringTask.Result))
             .FlattenExceptions();
        }

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        /// <summary>
        /// Constructs and returns the url for sampling points quota targets code 
        /// based on supplied <paramref name="surveyId"/>  and <paramref name="samplingPointId"/>
        /// </summary>
        private Uri SamplingPointsQuotaTargetsApi(string surveyId, string samplingPointId, string quotaLevelId = "")
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/SamplingPoints/{samplingPointId}/QuotaTargets/{quotaLevelId}");
        }
    }
}

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
using Nfield.Infrastructure;
using Nfield.Models;
using System;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveySamplingMethodService"/>
    /// </summary>
    internal class NfieldSurveySamplingMethodService : INfieldSurveySamplingMethodService, INfieldConnectionClientObject
    {
        private INfieldHttpClient Client => ConnectionClient.Client;

        #region Implementation of INfieldSurveySamplingMethodService

        public async Task<SamplingMethodType> GetAsync(string surveyId)
        {
            if (surveyId == null)
                throw new ArgumentNullException(nameof(surveyId));

            var result = await Client.GetAsync(SurveySamplingMethodUri(surveyId)).ConfigureAwait(false);

            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            var samplingMethodModel = JsonConvert.DeserializeObject<SamplingMethodModel>(content);

            return (SamplingMethodType)Enum.Parse(typeof(SamplingMethodType), samplingMethodModel.SamplingMethod);
        }

        public async Task UpdateAsync(string surveyId, SamplingMethodType samplingMethod)
        {
            if (surveyId == null)
                throw new ArgumentNullException(nameof(surveyId));

            var samplingMethodModel = new SamplingMethodModel
            {
                SamplingMethod = samplingMethod.ToString()
            };

            await Client.PatchAsJsonAsync(SurveySamplingMethodUri(surveyId), samplingMethodModel).ConfigureAwait(false);
        }

        private Uri SurveySamplingMethodUri(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/SamplingMethod/");
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

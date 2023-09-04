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
    /// Implementation of <see cref="INfieldDataRetentionService"/>
    /// </summary>
    internal class NfieldDataRetentionService : INfieldDataRetentionService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldDataRetentionService

        public async Task<DataRetentionSettings> GetAsync(string surveyId)
        {
            using (var response = await Client.GetAsync(DataRetentionEndpoint(surveyId)).ConfigureAwait(false))
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<DataRetentionSettings>(content);
            }
        }

        public async Task PutAsync(string surveyId, int retentionPeriod)
        {
            await Client.PutAsJsonAsync(DataRetentionEndpoint(surveyId), new { RetentionPeriod = retentionPeriod }).ConfigureAwait(false);
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; private set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        #endregion

        #region Helpers

        private Uri DataRetentionEndpoint(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"surveys/{surveyId}/DataRetentionSettings");
        }

        #endregion
    }
}

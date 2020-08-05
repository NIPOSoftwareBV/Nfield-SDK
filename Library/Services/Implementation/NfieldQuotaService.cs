using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.SDK.Models;
using Nfield.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nfield.SDK.Services.Implementation
{
    internal class NfieldQuotaService : INfieldQuotaService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        #region Implementation of INfieldQuotaService

        /// <summary>
        /// See <see cref="INfieldQuotaService.GetQuotaFrameVersionsAsync"/>
        /// </summary>
        public async Task<IEnumerable<QuotaFrameVersion>> GetQuotaFrameVersionsAsync(string surveyId)
        {
            ValidateSurveyId(surveyId);

            var result = await Client.GetAsync(QuotaFrameVersionsUri(surveyId)).ConfigureAwait(false);

            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<IEnumerable<QuotaFrameVersion>>(content);
        }

        /// <summary>
        /// See <see cref="INfieldQuotaService.UpdateQuotaTargetsAsync"/>
        /// </summary>
        public async Task UpdateQuotaTargetsAsync(string surveyId, string eTag, IEnumerable<QuotaFrameLevelTarget> targets)
        {
            ValidateSurveyId(surveyId);

            await Client.PutAsJsonAsync(QuotaFrameTargetsUri(surveyId, eTag), targets).ConfigureAwait(false);
        }

        /// <summary>
        /// See <see cref="INfieldQuotaService.GetQuotaFrameAsync"/>
        /// </summary>
        public async Task<QuotaFrame> GetQuotaFrameAsync(string surveyId, string eTag)
        {
            ValidateSurveyId(surveyId);

            var result = await Client.GetAsync(QuotaFrameUri(surveyId, eTag)).ConfigureAwait(false);

            var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<QuotaFrame>(content);
        }

        #endregion
        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri QuotaFrameVersionsUri(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/QuotaVersions");
        }

        private Uri QuotaFrameTargetsUri(string surveyId, string eTag)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/Quota/{eTag}");
        }

        private Uri QuotaFrameUri(string surveyId, string eTag)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/QuotaVersions/{eTag}");
        }

        private static void ValidateSurveyId(string surveyId)
        {
            if (surveyId == null)
                throw new ArgumentNullException(nameof(surveyId));

            if (surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be empty");
        }
    }
}

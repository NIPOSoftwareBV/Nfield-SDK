using Newtonsoft.Json;
using Nfield.Extensions;
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
        public Task<IEnumerable<QuotaFrameVersion>> GetQuotaFrameVersionsAsync(string surveyId)
        {
            ValidateSurveyId(surveyId);

            return Client.GetAsync(QuotaFrameVersionsUri(surveyId))
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<IEnumerable<QuotaFrameVersion>>(task.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldQuotaService.UpdateQuotaTargetsAsync"/>
        /// </summary>
        public Task UpdateQuotaTargetsAsync(string surveyId, string eTag, IEnumerable<QuotaFrameLevelTarget> targets)
        {
            ValidateSurveyId(surveyId);

            return Client.PutAsJsonAsync(QuotaFrameTargetsUri(surveyId, eTag), targets)
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldQuotaService.GetQuotaFrameAsync"/>
        /// </summary>
        public Task<QuotaFrame> GetQuotaFrameAsync(string surveyId, string eTag)
        {
            ValidateSurveyId(surveyId);

            return Client.GetAsync(QuotaFrameUri(surveyId, eTag))
             .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(task => JsonConvert.DeserializeObject<QuotaFrame>(task.Result))
             .FlattenExceptions();
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

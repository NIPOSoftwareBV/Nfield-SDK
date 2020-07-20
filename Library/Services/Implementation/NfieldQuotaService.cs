using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
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

        public Task<IEnumerable<QuotaFrameVersion>> GetQuotaFrameVersionsAsync(string surveyId)
        {
            ValidateParams(surveyId);

            return Client.GetAsync(QuotaFrameVersionsApi(surveyId))
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<IEnumerable<QuotaFrameVersion>>(task.Result))
                         .FlattenExceptions();
        }

        public Task SaveQuotaTargetsAsync(string surveyId, string quotaETag, IEnumerable<QuotaFrameLevelTarget> targets)
        {
            ValidateParams(surveyId);

            return Client.PutAsJsonAsync(EditingQuotaFrameTargetsApi(surveyId, quotaETag), targets)
                         .FlattenExceptions();
        }

        #endregion
        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri QuotaFrameVersionsApi(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/QuotaVersions");
        }

        private Uri EditingQuotaFrameTargetsApi(string surveyId, string version)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/QuotaVersions/{version}/QuotaTargets");
        }

        private static void ValidateParams(string surveyId)
        {
            if (surveyId == null)
                throw new ArgumentNullException(nameof(surveyId));

            if (surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be empty");
        }
    }
}

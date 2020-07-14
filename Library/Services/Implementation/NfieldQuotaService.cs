using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services;
using Nfield.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
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

        #endregion
        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri QuotaFrameVersionsApi(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/QuotaVersions");
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

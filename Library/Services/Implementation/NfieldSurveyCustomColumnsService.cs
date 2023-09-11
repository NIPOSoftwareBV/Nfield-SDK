using Newtonsoft.Json;
using Nfield.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nfield.SDK.Services.Implementation
{
    internal class NfieldSurveyCustomColumnsService : INfieldSurveyCustomColumnsService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyPackageService

        public async Task<IEnumerable<string>> GetSurveyCustomColumnsAsync(string surveyId)
        {
            CheckSurveyId(surveyId);

            var response = await ConnectionClient.Client.GetAsync(SurveyCustomColumnsApi(surveyId));
            var stringResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<string>>(stringResult);
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
            {
                throw new ArgumentNullException("surveyId");
            }
            if (surveyId.Trim().Length == 0)
            {
                throw new ArgumentException("surveyId cannot be empty");
            }
        }

        private Uri SurveyCustomColumnsApi(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/CustomColumns");
        }
    }
}

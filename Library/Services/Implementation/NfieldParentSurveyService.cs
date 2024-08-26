using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Utilities;

namespace Nfield.Services.Implementation
{
    internal class NfieldParentSurveyService : INfieldParentSurveyService, INfieldConnectionClientObject
    {
        public INfieldConnectionClient ConnectionClient { get; internal set; }
        public void InitializeNfieldConnection(INfieldConnectionClient connection) => ConnectionClient = connection;

        #region Implementation of INfieldParentSurveyService

        public async Task<IQueryable<Survey>> GetParentSurveysAsync()
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, "ParentSurveys");

            var response = await ConnectionClient.Client.GetAsync(uri).ConfigureAwait(false);
            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Survey>>(stringResponse).AsQueryable();
        }

        public async Task<Survey> AddParentSurveyAsync(ParentSurvey survey)
        {
            Ensure.ArgumentNotNull(survey, nameof(survey));

            var uri = new Uri(ConnectionClient.NfieldServerUri, "ParentSurveys");
            var response = await ConnectionClient.Client.PostAsJsonAsync(uri, survey).ConfigureAwait(false);

            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Survey>(stringResponse);
        }


        #endregion
    }
}

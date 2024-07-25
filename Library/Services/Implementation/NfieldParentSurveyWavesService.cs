﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Utilities;

namespace Nfield.Services.Implementation
{
    internal class NfieldParentSurveyWavesService : INfieldParentSurveyWavesService, INfieldConnectionClientObject
    {
        public INfieldConnectionClient ConnectionClient { get; internal set; }
        public void InitializeNfieldConnection(INfieldConnectionClient connection) => ConnectionClient = connection;

        #region Implementation of INfieldParentSurveyWavesService

        public async Task<IQueryable<Survey>> GetParentSurveyWavesAsync(string parentSurveyId)
        {
            Ensure.ArgumentNotNullOrEmptyString(parentSurveyId, nameof(parentSurveyId));

            var uri = WavesUrl(parentSurveyId);

            var response = await ConnectionClient.Client.GetAsync(uri).ConfigureAwait(false);
            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Survey>>(stringResponse).AsQueryable();
        }

        public async Task<Survey> AddWaveAsync(string parentSurveyId, WaveSurvey survey)
        {
            Ensure.ArgumentNotNullOrEmptyString(parentSurveyId, nameof(parentSurveyId));
            Ensure.ArgumentNotNull(survey, nameof(survey));

            var uri = WavesUrl(parentSurveyId);
            var response = await ConnectionClient.Client.PostAsJsonAsync(uri, survey).ConfigureAwait(false);

            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Survey>(stringResponse);
        }

        private Uri WavesUrl(string parentSurveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"ParentSurveys/{parentSurveyId}/Waves/");
        }

        #endregion
    }
}

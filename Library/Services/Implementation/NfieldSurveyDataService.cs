using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveyDataService"/>
    /// </summary>
    internal class NfieldSurveyDataService : INfieldSurveyDataService, INfieldConnectionClientObject
    {
        /// <summary>
        /// See <see cref="INfieldSurveyDataService.PostAsync"/>
        /// </summary>
        public Task<BackgroundTask> PostAsync(SurveyDownloadDataRequest surveyDownloadDataRequest)
        {
            if (surveyDownloadDataRequest == null)
            {
                throw new ArgumentNullException("surveyDownloadDataRequest");
            }
            var uri = SurveyDataUrl(surveyDownloadDataRequest.SurveyId);

            return Client.PostAsJsonAsync(uri, surveyDownloadDataRequest)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObjectAsync<BackgroundTask>(task.Result).Result)
                         .FlattenExceptions();
        }

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private string SurveyDataUrl(string surveyId)
        {
            var result = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            result.AppendFormat(CultureInfo.InvariantCulture, @"Surveys/{0}/Data/", surveyId);

            return result.ToString();
        }

    }
}

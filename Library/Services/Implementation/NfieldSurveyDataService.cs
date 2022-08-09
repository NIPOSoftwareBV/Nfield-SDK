using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;

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
                throw new ArgumentNullException(nameof(surveyDownloadDataRequest));
            }

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyDownloadDataRequest.SurveyId}/data");

            return Client.PostAsJsonAsync(uri, surveyDownloadDataRequest)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<BackgroundTask>(task.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldSurveyDataService.PrepareDownload"/>
        /// </summary>
        public async Task<string> PrepareDownload(string surveyId, SurveyDataRequest surveyDataRequest)
        {
            CheckSurveyId(surveyId);

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/DataDownload");

            return await Client.PostAsJsonAsync(uri, surveyDataRequest)
                        .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                        .ContinueWith(task => JsonConvert.DeserializeObject<BackgroundActivityStatus>(task.Result))
                        .ContinueWith(task => ConnectionClient.GetActivityResultAsync<string>(task.Result.ActivityId, "DownloadDataUrl").Result)
                        .FlattenExceptions().ConfigureAwait(false);
        }

        /// <summary>
        /// See <see cref="INfieldSurveyDataService.PrepareInterviewDownload"/>
        /// </summary>
        public async Task<string> PrepareInterviewDownload(string surveyId, int interviewId, SurveyDataInterviewRequest surveyDataInterviewRequest)
        {
            CheckSurveyId(surveyId);

            var uri = new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/DataDownload/{interviewId}");

            return await Client.PostAsJsonAsync(uri, surveyDataInterviewRequest)
                        .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                        .ContinueWith(task => JsonConvert.DeserializeObject<BackgroundActivityStatus>(task.Result))
                        .ContinueWith(task => ConnectionClient.GetActivityResultAsync<string>(task.Result.ActivityId, "DownloadDataUrl").Result)
                        .FlattenExceptions().ConfigureAwait(false);
        }

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
                throw new ArgumentNullException(nameof(surveyId));
            if (surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be empty");
        }

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

    }
}

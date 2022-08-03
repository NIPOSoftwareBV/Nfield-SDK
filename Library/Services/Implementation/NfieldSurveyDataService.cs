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
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"surveys/{surveyDownloadDataRequest.SurveyId}/data");

            return Client.PostAsJsonAsync(uri, surveyDownloadDataRequest)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<BackgroundTask>(task.Result))
                         .FlattenExceptions();
        }

        public async Task<string> PrepareDownload(string surveyId, SurveyDownloadDataRequest surveyDownloadDataRequest)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"surveys/{surveyId}/DataDownload");

            return await Client.PostAsJsonAsync(uri, surveyDownloadDataRequest)
                        .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                        .ContinueWith(task => JsonConvert.DeserializeObject<BackgroundActivityStatus>(task.Result))
                        .ContinueWith(task => ConnectionClient.GetActivityResultAsync<string>(task.Result.ActivityId, "DownloadDataUrl").Result)
                        .FlattenExceptions().ConfigureAwait(false);
        }


        public async Task<string> PrepareInterviewDownload(string surveyId, int interviewId)
        {
            var uri = new Uri(ConnectionClient.NfieldServerUri, $"surveys/{surveyId}/DataDownload/{interviewId}");

            return await Client.PostAsJsonAsync(uri, new object())
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

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

    }
}

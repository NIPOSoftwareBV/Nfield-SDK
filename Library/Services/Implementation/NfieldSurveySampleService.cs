using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nfield.Extensions;
using Nfield.Infrastructure;

namespace Nfield.Services.Implementation
{
    internal class NfieldSurveySampleService : INfieldSurveySampleService
    {

        public Task<string> GetAsync(string surveyId)
        {
            CheckRequiredStringArgument(surveyId, nameof(surveyId));

            var uri = SurveySampleUrl(surveyId);

            return Client.GetAsync(uri)
                .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                .FlattenExceptions();
        }

        public Task<string> PostAsync(string surveyId, string sample)
        {
            CheckRequiredStringArgument(surveyId, nameof(surveyId));
            CheckRequiredStringArgument(sample, nameof(sample));

            var uri = SurveySampleUrl(surveyId);
            var sampleContent = new StringContent(sample);
            return Client.PostAsync(uri, sampleContent)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .FlattenExceptions();
        }

        public Task<string> DeleteAsync(string surveyId, string sampleRecordId)
        {
            CheckRequiredStringArgument(surveyId, nameof(surveyId));
            CheckRequiredStringArgument(sampleRecordId, nameof(sampleRecordId));

            var uri = SurveySampleUrl(surveyId, sampleRecordId);
            var filters = $"[{{\"Name\":\"RespondentKey\",\"Op\":\"eq\",\"Value\":\"{sampleRecordId}\"}}]";
            return Client.DeleteAsJsonAsync(uri, filters)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .FlattenExceptions();
        }

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private INfieldHttpClient Client => ConnectionClient.Client;

        private string SurveySampleUrl(string surveyId, string sampleRecordId)
        {
            var result = new StringBuilder(SurveySampleUrl(surveyId));
            result.AppendFormat(CultureInfo.InvariantCulture, @"/{0}", sampleRecordId);

            return result.ToString();
        }

        private string SurveySampleUrl(string surveyId)
        {
            var result = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            result.AppendFormat(CultureInfo.InvariantCulture, @"Surveys/{0}/Sample", surveyId);

            return result.ToString();
        }

        private static void CheckRequiredStringArgument(string argument, string name)
        {
            if (argument == null)
                throw new ArgumentNullException(name);
            if (argument.Trim().Length == 0)
                throw new ArgumentException($"{name} cannot be empty");
        }
    }
}

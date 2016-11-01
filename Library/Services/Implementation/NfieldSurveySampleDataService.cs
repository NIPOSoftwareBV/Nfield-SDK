using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSurveySampleDataService"/>
    /// </summary>
    internal class NfieldSurveySampleDataService : INfieldSurveySampleDataService, INfieldConnectionClientObject
    {
        /// <summary>
        /// See <see cref="INfieldSurveySampleDataService.GetAsync"/>
        /// </summary>
        public Task<BackgroundTask> GetAsync(string surveyId, string fileName)
        {
            CheckRequiredStringArgument(surveyId, nameof(surveyId));
            CheckRequiredStringArgument(fileName, nameof(fileName));

            var uri = SurveySampleDataUrl(surveyId, fileName);

            return Client.GetAsync(uri)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<BackgroundTask>(task.Result))
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

        private string SurveySampleDataUrl(string surveyId, string fileName)
        {
            var result = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            result.AppendFormat(CultureInfo.InvariantCulture, @"Surveys/{0}/SampleData/{1}", surveyId, fileName);

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

using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    internal class NfieldSurveyInvitationImagesService : INfieldSurveyInvitationImagesService, INfieldConnectionClientObject
    {
        public Task<AddInvitationImageResult> AddImageAsync(string surveyId, string filename, Stream content)
        {
            CheckRequiredStringArgument(surveyId, nameof(surveyId));
            CheckRequiredStringArgument(filename, nameof(filename));
            CheckRequiredArgument(content, nameof(content));

            var uri = SurveyInvitationImagesTemplatesUrl(surveyId, filename);
            var imageContent = new StreamContent(content);
            return Client.PostAsync(uri, imageContent)
                    .ContinueWith(task => JsonConvert.DeserializeObject<AddInvitationImageResult>(
                        task.Result.Content.ReadAsStringAsync().Result))
                    .FlattenExceptions();
        }

        private INfieldHttpClient Client => ConnectionClient.Client;
        public INfieldConnectionClient ConnectionClient { get; internal set; }
        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        private string SurveyInvitationImagesTemplatesUrl(string surveyId, string filename)
        {
            var result = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            result.AppendFormat(CultureInfo.InvariantCulture, @"Surveys/{0}/InvitationImages/{1}", surveyId, filename);
            return result.ToString();
        }

        private static void CheckRequiredStringArgument(string argument, string name)
        {
            CheckRequiredArgument(argument, name);
            if (argument.Trim().Length == 0)
                throw new ArgumentException($"{name} cannot be empty");
        }

        private static void CheckRequiredArgument(object argument, string name)
        {
            if (argument == null)
                throw new ArgumentNullException(name);
        }
    }
}

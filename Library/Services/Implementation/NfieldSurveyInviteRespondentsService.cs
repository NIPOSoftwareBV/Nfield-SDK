//    This file is part of Nfield.SDK.
//
//    Nfield.SDK is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Nfield.SDK is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public License
//    along with Nfield.SDK.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    internal class NfieldSurveyInviteRespondentsService : INfieldSurveyInviteRespondentsService, INfieldConnectionClientObject
    {
        public Task<InviteRespondentsStatus> SendInvitationsAsync(string surveyId, InvitationBatch batch)
        {
            CheckRequiredStringArgument(surveyId, nameof(surveyId));
            CheckRequiredArgument(batch, nameof(batch));
            CheckBatchFields(batch, nameof(batch));

            var uri = SurveyInviteRespondentsUrl(surveyId);
            if (batch.Filters == null && batch.RespondentKeys != null)
            {
                batch.Filters = new[]
                {
                    new SampleFilter {Name = "RespondentKey", Op = "in", Value = string.Join(",", batch.RespondentKeys)}
                };
            }

            return Client.PostAsJsonAsync(uri, batch)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(stringResult => JsonConvert.DeserializeObject<InviteRespondentsStatus>(stringResult.Result))
                .FlattenExceptions();
        }

        private string SurveyInviteRespondentsUrl(string surveyId)
        {
            var result = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            result.AppendFormat(CultureInfo.InvariantCulture, @"Surveys/{0}/InviteRespondents", surveyId);

            return result.ToString();
        }

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private INfieldHttpClient Client => ConnectionClient.Client;

        private static void CheckRequiredStringArgument(string argument, string name)
        {
            if (argument == null)
                throw new ArgumentNullException(name);
            if (argument.Trim().Length == 0)
                throw new ArgumentException($"{name} cannot be empty");
        }

        private static void CheckRequiredArgument(object argument, string name)
        {
            if (argument == null)
                throw new ArgumentNullException(name);
        }

        private void CheckBatchFields(InvitationBatch batch, string name)
        {
            if ( batch.RespondentKeys != null && batch.Filters != null)
            {
                throw new ArgumentException("Either specify a filter or a list of respondentKeys, but not both.", name);
            }
        }


    }
}
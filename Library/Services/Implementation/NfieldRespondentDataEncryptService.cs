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
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    internal class NfieldRespondentDataEncryptService : INfieldRespondentDataEncryptService, INfieldConnectionClientObject
    {
        private INfieldHttpClient Client => ConnectionClient.Client;

        #region Implementations
        public INfieldConnectionClient ConnectionClient { get; private set; }
        
        /// <summary>
        /// Encrypts the data.
        /// </summary>
        /// <param name="surveyId">The survey identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Task<string> EncryptData(string surveyId, DataCryptographyModel model)
        {
            if (string.IsNullOrEmpty((surveyId)))
            {
                throw new ArgumentNullException(nameof(surveyId));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var uri = SurveyDataEncryptionUrl(surveyId);
            var result = Client.PostAsJsonAsync(uri, model).
                ContinueWith(task => task.Result.Content.ReadAsAsync<string>().Result)
                .FlattenExceptions();
            return result;
        }

        /// <summary>
        /// Initializes the nfield connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }
        #endregion

        /// <summary>
        /// Surveys the data encryption URL.
        /// </summary>
        /// <param name="surveyId">The survey identifier.</param>
        /// <returns></returns>
        private string SurveyDataEncryptionUrl(string surveyId)
        {
            var result = new StringBuilder(ConnectionClient.NfieldServerUri.AbsoluteUri);
            result.AppendFormat(CultureInfo.InvariantCulture, @"Surveys/{0}/RespondentDataEncrypt", surveyId);

            return result.ToString();
        }
    }
}

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

using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    internal class NfieldRespondentSurveysService : INfieldRespondentSurveysService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        #region Implementation of INfieldRespondentSurveysService

        /// <summary>
        /// See <see cref="INfieldRespondentSurveysService.GetAsync"/>
        /// </summary>
        public Task<IList<Survey>> GetAsync(string searchValue)
        {
            CheckRequiredStringArgument(searchValue);

            var uri = RespondentSurveysUrl(searchValue);
            return Client.GetAsync(uri)
                .ContinueWith(
                    responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(
                    stringTask =>
                        JsonConvert.DeserializeObject<IList<Survey>>(stringTask.Result))
                .FlattenExceptions();
        }

        #endregion

        #region Private Methods

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private string RespondentSurveysUrl(string searchValue)
        {
            return $"{ConnectionClient.NfieldServerUri.AbsoluteUri}/RespondentSurveys/{searchValue}";
        }

        private void CheckRequiredStringArgument(string argument)
        {
            var name = nameof(argument);

            if (argument == null)
                throw new ArgumentNullException(name);
            if (argument.Trim().Length == 0)
                throw new ArgumentException($"{name} cannot be empty");
        }

        #endregion
    }
}

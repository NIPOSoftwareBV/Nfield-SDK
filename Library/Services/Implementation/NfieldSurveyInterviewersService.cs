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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    internal class NfieldSurveyInterviewersService : INfieldSurveyInterviewersService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyInterviewersService

        public Task<IQueryable<SurveyInterviewerAssignmentModel>> GetAsync(string surveyId)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }

            var uri = SurveyInterviewersUrl(surveyId);

            return Client.GetAsync(uri)
                .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                .ContinueWith(
                    stringTask => JsonConvert.DeserializeObject<List<SurveyInterviewerAssignmentModel>>(stringTask.Result).AsQueryable())
                .FlattenExceptions();
        }

        public Task AddAsync(string surveyId, string interviewerId)
        {
            if (string.IsNullOrEmpty(surveyId))
            {
                throw new ArgumentNullException("surveyId");
            }
            if (string.IsNullOrEmpty(interviewerId))
            {
                throw new ArgumentNullException("interviewerId");
            }

            var uri = SurveyInterviewersUrl(surveyId);
            var model = new SurveyInterviewerAddModel { InterviewerId = interviewerId };

            return Client.PostAsJsonAsync(uri, model).FlattenExceptions();
        }

        #endregion

        /// <summary>
        /// Constructs and returns the url for survey interviewers
        /// based on supplied <paramref name="surveyId"/>
        /// </summary>
        private string SurveyInterviewersUrl(string surveyId)
        {
            return string.Format(CultureInfo.InvariantCulture,
                "{0}/Surveys/{1}/Interviewers/",
                ConnectionClient.NfieldServerUri.AbsoluteUri, surveyId);
        }

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion
    }
}
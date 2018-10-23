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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldSamplingPointInterviewerAssignmentsService"/>
    /// </summary>
    internal class NfieldSamplingPointInterviewerAssignmentsService :
        INfieldSamplingPointInterviewerAssignmentsService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSamplingPointInterviewerAssignmentsService

        public Task<IQueryable<InterviewerSamplingPointAssignmentModel>> QueryAsync(string surveyId, string samplingPointId)
        {
            CheckParameters(surveyId, samplingPointId);

            var uri = AssignmentsApi(surveyId, samplingPointId, null);

            return Client.GetAsync(uri)
                 .ContinueWith(
                     responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                 .ContinueWith(
                     stringTask =>
                     JsonConvert.DeserializeObject<List<InterviewerSamplingPointAssignmentModel>>(stringTask.Result).AsQueryable())
                 .FlattenExceptions();
        }

        public Task AssignAsync(string surveyId, string samplingPointId, string interviewerId)
        {
            CheckParameters(surveyId, samplingPointId, interviewerId);

            var uri = AssignmentsApi(surveyId, samplingPointId, interviewerId);

            return Client.PostAsync(uri, null ).FlattenExceptions();
        }

        public Task AssignAsync(string surveyId, SamplingPointInterviewerAssignmentsModel model)
        {
            CheckParameters(surveyId, model);

            var uri = AssignmentsApi(surveyId);

            return Client.PostAsJsonAsync(uri, model).FlattenExceptions();
        }

        public Task UnassignAsync(string surveyId, string samplingPointId, string interviewerId)
        {
            CheckParameters(surveyId, samplingPointId, interviewerId);

            var uri = AssignmentsApi(surveyId, samplingPointId, interviewerId);

            return Client.DeleteAsync(uri).FlattenExceptions();
        }

        public Task UnassignAsync(string surveyId, SamplingPointInterviewerAssignmentsModel model)
        {
            CheckParameters(surveyId, model);

            var uri = AssignmentsApi(surveyId);

            return Client.DeleteAsJsonAsync(uri, model).FlattenExceptions();
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private static void CheckParameters(string surveyId, string samplingPointId)
        {
            if (surveyId == null)
                throw new ArgumentNullException("surveyId");
            if (surveyId.Trim().Length == 0)
                throw new ArgumentException("surveyId cannot be empty");
            if (samplingPointId == null)
                throw new ArgumentNullException("samplingPointId");
            if (samplingPointId.Trim().Length == 0)
                throw new ArgumentException("samplingPointId cannot be empty");
        }

        private static void CheckParameters(string surveyId, string samplingPointId, string interviewerId)
        {
            CheckParameters(surveyId, samplingPointId);
            if (interviewerId == null)
                throw new ArgumentNullException("interviewerId");
            if (interviewerId.Trim().Length == 0)
                throw new ArgumentException("interviewerId cannot be empty");
        }

        private static void CheckParameters(string surveyId, SamplingPointInterviewerAssignmentsModel model)
        {
            if (surveyId == null)
                throw new ArgumentNullException("surveyId");

            if (model == null)
                throw new ArgumentNullException("model");
        }

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri AssignmentsApi(string surveyId, string samplingPointId, string interviewerId)
        {
            var path = new StringBuilder();
            path.AppendFormat("Surveys/{0}/SamplingPoints/{1}/Assignments",
                surveyId, samplingPointId);
            if (!string.IsNullOrEmpty(interviewerId))
                path.AppendFormat("/{0}", interviewerId);

            return new Uri(ConnectionClient.NfieldServerUri, path.ToString());
        }

        private Uri AssignmentsApi(string surveyId)
        {
            return new Uri(ConnectionClient.NfieldServerUri, $"Surveys/{surveyId}/SamplingPointsAssignments");
        }

    }
}
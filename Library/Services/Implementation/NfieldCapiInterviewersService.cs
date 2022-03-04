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
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;
using Nfield.Quota.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldCapiInterviewersService"/>
    /// </summary>

    internal class NfieldCapiInterviewersService : INfieldCapiInterviewersService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldCapiInterviewersService

        /// <summary>
        /// See <see cref="INfieldCapiInterviewersService.AddAsync"/>
        /// </summary>
        public Task<CapiInterviewer> AddAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException(nameof(interviewer));
            }

            var newCapiInterviewer = new NewCapiInterviewer
            {
                UserName = interviewer.UserName,
                Password = interviewer.Password,
                EmailAddress = interviewer.EmailAddress,
                FirstName = interviewer.FirstName,
                LastName = interviewer.LastName,
                TelephoneNumber = interviewer.TelephoneNumber,
                IsSupervisor = interviewer.IsSupervisor
            };

            return Client.PostAsJsonAsync(CapiInterviewersApi, newCapiInterviewer)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<CapiInterviewer>(task.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldCapiInterviewersService.RemoveAsync"/>
        /// </summary>
        public Task RemoveAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException(nameof(interviewer));
            }

            return
                Client.DeleteAsync(new Uri(CapiInterviewersApi, interviewer.InterviewerId))
                      .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldCapiInterviewersService.UpdateAsync"/>
        /// </summary>
        public Task<CapiInterviewer> UpdateAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException(nameof(interviewer));
            }

            var updatedInterviewer = new EditCapiInterviewer
            {
                EmailAddress = interviewer.EmailAddress,
                FirstName = interviewer.FirstName,
                LastName = interviewer.LastName,
                TelephoneNumber = interviewer.TelephoneNumber,
                IsSupervisor = interviewer.IsSupervisor
            };

            return Client.PatchAsJsonAsync(new Uri(CapiInterviewersApi, interviewer.InterviewerId), updatedInterviewer)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask => JsonConvert.DeserializeObject<CapiInterviewer>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldCapiInterviewersService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<Interviewer>> QueryAsync()
        {
            return Client.GetAsync(CapiInterviewersApi)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<Interviewer>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldCapiInterviewersService.InterviewerByClientIdAsync"/>
        /// </summary>
        public Task<Interviewer> InterviewerByClientIdAsync(string clientInterviewerId)
        {

            var uri = new Uri(CapiInterviewersApi, $"GetByClientId/{clientInterviewerId}");

            return Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<Interviewer>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldCapiInterviewersService.ChangePasswordAsync"/>
        /// </summary>
        public Task<CapiInterviewer> ChangePasswordAsync(Interviewer interviewer, string password)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException(nameof(interviewer));
            }

            return Client.PutAsJsonAsync(new Uri(CapiInterviewersApi, interviewer.InterviewerId), new ResetPasswordModel { Password = password })
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask => JsonConvert.DeserializeObject<CapiInterviewer>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldCapiInterviewersService.QueryOfficesOfInterviewerAsync"/>
        /// </summary>
        public Task<IEnumerable<string>> QueryOfficesOfInterviewerAsync(string interviewerId)
        {
            var uri = new Uri(CapiInterviewersApi, $"{interviewerId}/Offices");

            return Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<IEnumerable<string>>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldCapiInterviewersService.AddInterviewerToFieldworkOfficesAsync"/>
        /// </summary>
        public Task AddInterviewerToFieldworkOfficesAsync(string interviewerId, string officeId)
        {
            var uri = new Uri(CapiInterviewersApi, $"{interviewerId}/Offices");

            return Client.PostAsJsonAsync(uri, new InterviewerFieldworkOfficeModel { OfficeId = officeId }).FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldCapiInterviewersService.RemoveInterviewerFromFieldworkOfficesAsync"/>
        /// </summary>
        public Task RemoveInterviewerFromFieldworkOfficesAsync(string interviewerId, string fieldworkOfficeId)
        {
            var uri = new Uri(CapiInterviewersApi, $"{interviewerId}/Offices/{fieldworkOfficeId}");

            return Client.DeleteAsync(uri).FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldCapiInterviewersService.QueryLogsAsync"/>
        /// </summary>
        public async Task<string> QueryLogsAsync(LogQueryModel query)
        {
            Ensure.ArgumentNotNull(query, nameof(query));

            var uri = new Uri(ConnectionClient.NfieldServerUri, "InterviewersWorklog");

            return await Client.PostAsJsonAsync(uri, query)
                          .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                          .ContinueWith(task => JsonConvert.DeserializeObject<BackgroundActivityStatus>(task.Result))
                          .ContinueWith(task => ConnectionClient.GetActivityResultAsync<string>(task.Result.ActivityId, "DownloadDataUrl").Result)
                          .FlattenExceptions().ConfigureAwait(true);
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        #region private methods
        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri CapiInterviewersApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri, "CapiInterviewers/"); }
        }
        #endregion

        internal class ResetPasswordModel
        {

            /// <summary>
            /// The new password specified for the interviewer
            /// </summary>
            public string Password { get; set; }

        }
    }
}

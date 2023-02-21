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
using System.Linq;
using System.Net.Http;
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
        public Task<CapiInterviewer> AddAsync(CreateCapiInterviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException(nameof(interviewer));
            }

            return Client.PostAsJsonAsync(CapiInterviewersApi, interviewer)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<CapiInterviewer>(task.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldCapiInterviewersService.RemoveAsync"/>
        /// </summary>
        public Task RemoveAsync(CapiInterviewer interviewer)
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
        public Task<CapiInterviewer> UpdateAsync(CapiInterviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException(nameof(interviewer));
            }

            return Client.PatchAsJsonAsync(new Uri(CapiInterviewersApi, interviewer.InterviewerId), interviewer)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask => JsonConvert.DeserializeObject<CapiInterviewer>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldCapiInterviewersService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<CapiInterviewer>> QueryAsync()
        {
            return Client.GetAsync(CapiInterviewersApi)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<CapiInterviewer>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldCapiInterviewersService.InterviewerByClientIdAsync"/>
        /// </summary>
        public Task<CapiInterviewer> InterviewerByClientIdAsync(string clientInterviewerId)
        {

            var uri = new Uri(CapiInterviewersApi, $"GetByClientId/{clientInterviewerId}");

            return Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<CapiInterviewer>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.ChangePasswordAsync"/>
        /// </summary>
        public Task<CapiInterviewer> ChangePasswordAsync(CapiInterviewer interviewer, string password)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            return Client.PutAsJsonAsync(new Uri(CapiInterviewersApi, interviewer.InterviewerId), (object)new { Password = password })
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask => JsonConvert.DeserializeObject<CapiInterviewer>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldCapiInterviewersService.AddInterviewerToFieldworkOfficesAsync"/>
        /// </summary>
        public Task AddInterviewerToFieldworkOfficesAsync(string interviewerId, string officeId)
        {
            var uri = new Uri(CapiInterviewersApi, $"{interviewerId}/FieldworkOffices/{officeId}");

            return Client.PatchAsJsonAsync(uri, string.Empty).FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldCapiInterviewersService.RemoveInterviewerFromFieldworkOfficesAsync(string, string)"/>
        /// </summary>
        public Task RemoveInterviewerFromFieldworkOfficesAsync(string interviewerId, string officeId)
        {
            var uri = new Uri(CapiInterviewersApi, $"{interviewerId}/FieldworkOffices/{officeId}");

            return Client.DeleteAsync(uri).FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldCapiInterviewersService.QueryOfficesAsync"/>
        /// </summary>
        public Task<IEnumerable<string>> QueryOfficesAsync(string interviewerId)
        {
            return Client.GetAsync(new Uri(CapiInterviewersApi, $"{interviewerId}/FieldworkOffices"))
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<IEnumerable<string>>(stringTask.Result))
                         .FlattenExceptions();
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

        #region internal classes
        internal class ResetPasswordModel
        {

            /// <summary>
            /// The new password specified for the interviewer
            /// </summary>
            public string Password { get; set; }
        }

        #endregion
    }
}

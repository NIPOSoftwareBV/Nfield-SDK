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
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldInterviewersService"/>
    /// </summary>
    internal class NfieldInterviewersService : INfieldInterviewersService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldInterviewersService

        /// <summary>
        /// See <see cref="INfieldInterviewersService.AddAsync"/>
        /// </summary>
        public Task<Interviewer> AddAsync(Interviewer interviewer)
        {
            return Client.PostAsJsonAsync(InterviewersApi, interviewer)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObject<Interviewer>(task.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.RemoveAsync"/>
        /// </summary>
        public Task RemoveAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            return
                Client.DeleteAsync(new Uri(InterviewersApi, interviewer.InterviewerId))
                      .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.UpdateAsync"/>
        /// </summary>
        public Task<Interviewer> UpdateAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            var updatedInterviewer = new UpdateInterviewer
            {
                EmailAddress = interviewer.EmailAddress,
                FirstName = interviewer.FirstName,
                LastName = interviewer.LastName,
                TelephoneNumber = interviewer.TelephoneNumber,
                IsSupervisor = interviewer.IsSupervisor
            };

            return Client.PatchAsJsonAsync(new Uri(InterviewersApi, interviewer.InterviewerId), updatedInterviewer)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask => JsonConvert.DeserializeObject<Interviewer>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<Interviewer>> QueryAsync()
        {
            return Client.GetAsync(InterviewersApi)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<List<Interviewer>>(stringTask.Result).AsQueryable())
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.InterviewerByClientIdAsync"/>
        /// </summary>
        public Task<Interviewer> InterviewerByClientIdAsync(string clientInterviewerId)
        {

            var uri = new Uri(InterviewersApi, $"GetByClientId/{clientInterviewerId}");

            return Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<Interviewer>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.ChangePasswordAsync"/>
        /// </summary>
        public Task<Interviewer> ChangePasswordAsync(Interviewer interviewer, string password)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            return Client.PutAsJsonAsync(new Uri(InterviewersApi, interviewer.InterviewerId), (object)new { Password = password })
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask => JsonConvert.DeserializeObject<Interviewer>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldInterviewersService.QueryOfficesOfInterviewerAsync"/>
        /// </summary>
        public Task<IEnumerable<string>> QueryOfficesOfInterviewerAsync(string interviewerId)
        {
            var uri = new Uri(InterviewersApi, $"{interviewerId}/Offices");

            return Client.GetAsync(uri)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask =>
                             JsonConvert.DeserializeObject<IEnumerable<string>>(stringTask.Result))
                         .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldInterviewersService.AddInterviewerToFieldworkOfficesAsync"/>
        /// </summary>
        public Task AddInterviewerToFieldworkOfficesAsync(string interviewerId, string officeId)
        {
            var uri = new Uri(InterviewersApi, $"{interviewerId}/Offices");

            return Client.PostAsJsonAsync(uri, new InterviewerFieldworkOfficeModel { OfficeId = officeId }).FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldInterviewersService.RemoveInterviewerFromFieldworkOfficesAsync"/>
        /// </summary>
        public Task RemoveInterviewerFromFieldworkOfficesAsync(string interviewerId, string fieldworkOfficeId)
        {
            var uri = new Uri(InterviewersApi, $"{interviewerId}/Offices/{fieldworkOfficeId}");

            return Client.DeleteAsync(uri).FlattenExceptions();
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }

        private Uri InterviewersApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri, "interviewers/"); }
        }
    }

    internal class UpdateInterviewer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public bool IsSupervisor { get; set; }
    }
}

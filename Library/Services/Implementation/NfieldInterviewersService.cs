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
            return Client.PostAsJsonAsync(InterviewersApi.AbsoluteUri, interviewer)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObjectAsync<Interviewer>(task.Result).Result)
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
                Client.DeleteAsync(InterviewersApi + interviewer.InterviewerId)
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
                    TelephoneNumber = interviewer.TelephoneNumber
                };

            return Client.PatchAsJsonAsync(InterviewersApi + interviewer.InterviewerId, updatedInterviewer)
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask => JsonConvert.DeserializeObjectAsync<Interviewer>(stringTask.Result).Result)
                         .FlattenExceptions();
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<Interviewer>> QueryAsync()
        {
            return Client.GetAsync(InterviewersApi.AbsoluteUri)
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
        public Task <Interviewer> InterviewerByClientIdAsync(string clientInterviewerId)
        {

            string uri = string.Format(@"{0}/GetByClientId/{1}", InterviewersApi.AbsoluteUri, clientInterviewerId);

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

            return Client.PutAsJsonAsync(InterviewersApi + interviewer.InterviewerId, (object)new { Password = password })
                         .ContinueWith(
                             responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(
                             stringTask => JsonConvert.DeserializeObjectAsync<Interviewer>(stringTask.Result).Result)
                         .FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldInterviewersService.QueryOfficesOfInterviewerAsync"/>
        /// </summary>
        public Task<IEnumerable<string>> QueryOfficesOfInterviewerAsync(string interviewerId)
        {
            var uri = string.Format(@"{0}{1}/Offices", InterviewersApi.AbsoluteUri, interviewerId);

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
            var uri = string.Format(@"{0}{1}/Offices", InterviewersApi.AbsoluteUri, interviewerId);

            return Client.PostAsJsonAsync(uri, new InterviewerFieldworkOfficeModel{OfficeId = officeId}).FlattenExceptions();
        }

        /// <summary>
        /// <see cref="INfieldInterviewersService.RemoveInterviewerFromFieldworkOfficesAsync"/>
        /// </summary>
        public Task RemoveInterviewerFromFieldworkOfficesAsync(string interviewerId, string fieldworkOfficeId)
        {
            var uri = string.Format(@"{0}{1}/Offices/{2}", InterviewersApi.AbsoluteUri, interviewerId, fieldworkOfficeId);

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
            get { return new Uri(ConnectionClient.NfieldServerUri.AbsoluteUri + "interviewers/"); }
        }
    }

    internal class UpdateInterviewer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
    }
}

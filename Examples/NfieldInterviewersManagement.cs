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
using System.Threading.Tasks;
using Nfield.Models;
using Nfield.Services;
using Nfield.Extensions;
using System.Linq;

namespace Nfield.SDK.Samples
{
    /// <summary>
    /// Helper class handling <see cref="Interviewer"/>s management.
    /// This demonstrates various operations that are available on <see cref="INfieldInterviewersService"/>.
    /// </summary>
    public class NfieldInterviewersManagement
    {
        private readonly INfieldInterviewersService _interviewersService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NfieldInterviewersManagement(INfieldInterviewersService interviewersService)
        {
            _interviewersService = interviewersService;
        }

        /// <summary>
        /// Adds an <see cref="Interviewer"/> to the system with a synchronous operation.
        /// </summary>
        public Interviewer AddInterviewer()
        {
            Interviewer interviewer = new Interviewer
                {
                    ClientInterviewerId = "pomn45dr",
                    FirstName = "Steve",
                    LastName = "Balmer",
                    EmailAddress = "steve@hotmail.com",
                    TelephoneNumber = "0207821569",
                    UserName = "steve",
                    Password = "password12"
                };

            return _interviewersService.Add(interviewer);
        }

        /// <summary>
        /// Adds an <see cref="Interviewer"/> to the system with an asynchronous operation.
        /// </summary>
        public Task<Interviewer> AddInterviewerAsync()
        {
            Interviewer interviewer = new Interviewer
            {
                ClientInterviewerId = "ftropo5i",
                FirstName = "Bill",
                LastName = "Gates",
                EmailAddress = "bill@hotmail.com",
                TelephoneNumber = "0206598547",
                UserName = "bill",
                Password = "password12"
            };

            return _interviewersService.AddAsync(interviewer);
        }

        /// <summary>
        /// Updates an <see cref="Interviewer"/> with a synchronous operation.
        /// </summary>
        public Interviewer UpdateInterviewer(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                return null;
            }

            return _interviewersService.Update(interviewer);
        }

        /// <summary>
        /// Updates an <see cref="Interviewer"/> with an asynchronous operation.
        /// </summary>
        public Task<Interviewer> UpdateInterviewerAsync(Interviewer interviewer)
        {
            if(interviewer == null)
            {
                return null;
            }

            return _interviewersService.UpdateAsync(interviewer);
        }

        /// <summary>
        /// Removes an existing <see cref="Interviewer"/> with a synchronous operation.
        /// </summary>
        public void RemoveInterviewer(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                return;
            }

            _interviewersService.Remove(interviewer);
        }

        /// <summary>
        /// Removes an existing <see cref="Interviewer"/> with an asynchronous operation.
        /// </summary>
        public Task RemoveInterviewerAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                return null;
            }

            return _interviewersService.RemoveAsync(interviewer);
        }

        /// <summary>
        /// Performs query operation for available <see cref="Interviewer"/>s synchronously. 
        /// Note that this sample does not return the result, although your real class will do so.
        /// </summary>
        public void QueryForInterviewers()
        {
            IEnumerable<Interviewer> allInterviewers = _interviewersService.Query().ToList();

            IEnumerable<Interviewer> interviewersWithValidTelephone =
                _interviewersService.Query().Where(interviewer => !string.IsNullOrEmpty(interviewer.TelephoneNumber)).ToList();
        }

        /// <summary>
        /// Performs query operation for available <see cref="Interviewer"/>s asynchronously. 
        /// Note that this sample does not return the result, although your real class will do so.
        /// </summary>
        public void QueryForInterviewersAsync()
        {
            // execute async call
            Task<IQueryable<Interviewer>> task = _interviewersService.QueryAsync();

            // do some work here
            // ...

            // get async call result
            IEnumerable<Interviewer> allInterviewers = task.Result.ToList();

            IEnumerable<Interviewer> interviewersWithValidTelephone =
                allInterviewers.Where(interviewer => !string.IsNullOrEmpty(interviewer.TelephoneNumber)).ToList();
        }

        /// <summary>
        /// Changes the password for an <see cref="Interviewer"/> synchronously.
        /// </summary>
        public Interviewer ChangePassword(Interviewer interviewer, string password)
        {
            if (interviewer == null)
            {
                return null;
            }

            return _interviewersService.ChangePassword(interviewer, password);
        }

        /// <summary>
        /// Changes the password for an <see cref="Interviewer"/> asynchronously.
        /// </summary>
        public Task<Interviewer> ChangePasswordAsync(Interviewer interviewer, string password)
        {
            if (interviewer == null)
            {
                return null;
            }

            return _interviewersService.ChangePasswordAsync(interviewer, password);
        }
    }
}
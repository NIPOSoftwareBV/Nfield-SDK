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

using Nfield.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read and update the CAPI interviewer data.
    /// </summary>
    public interface INfieldCapiInterviewersService
    {
        /// <summary>
        /// Adds a new CAPI interviewer.
        /// </summary>
        /// <param name="interviewer">The CAPI interviewer to add.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<CapiInterviewer> AddAsync(CreateCapiInterviewer interviewer);

        /// <summary>
        /// Removes the CAPI interviewer.
        /// </summary>
        /// <param name="interviewer">The CAPI interviewer to remove.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>       
        Task RemoveAsync(CapiInterviewer interviewer);

        /// <summary>
        /// Updates a CAPI interviewer's data.
        /// </summary>
        /// <param name="interviewer">The CAPI interviewer to update.</param>
        /// <exception cref="T:System.AggregateException"></exception> 
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<CapiInterviewer> UpdateAsync(CapiInterviewer interviewer);

        /// <summary>
        /// Gets CAPI interviewer queryable object.
        /// <exception cref="T:System.AggregateException"></exception>
        /// </summary>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>   
        Task<IQueryable<CapiInterviewer>> QueryAsync();

        /// <summary>
        /// Gets the CAPI interviewer by clientId
        /// </summary>
        /// <param name="clientInterviewerId"></param>
        /// <returns></returns>
        Task<CapiInterviewer> InterviewerByClientIdAsync(string clientInterviewerId);

        /// <summary>
        /// Change the password of a CAPI interviewer
        /// </summary>
        /// <param name="interviewer">CAPI interviewer whose password to change</param>
        /// <param name="password">the new password</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>     
        Task<CapiInterviewer> ChangePasswordAsync(CapiInterviewer interviewer, string password);

        /// <summary>
        /// Assigns an CAPI interviewer to a fieldwork office
        /// </summary>
        /// <param name="interviewerId"></param>
        /// <param name="officeId"></param>
        /// <returns></returns>
        Task AddInterviewerToFieldworkOfficesAsync(string interviewerId, string officeId);

        /// <summary>
        /// Unassigns an CAPI interviewer from a fieldwork office
        /// </summary>
        /// <param name="interviewerId"></param>
        /// <param name="officeId"></param>
        /// <returns></returns>
        Task RemoveInterviewerFromFieldworkOfficesAsync(string interviewerId, string officeId);

        /// <summary>
        /// Gets offices for a CAPI interviewer.
        /// <exception cref="T:System.AggregateException"></exception>
        /// </summary>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>   
        Task<IEnumerable<string>> QueryOfficesAsync(string interviewerId);
    }
}
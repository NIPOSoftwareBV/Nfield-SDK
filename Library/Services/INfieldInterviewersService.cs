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
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read and update the interviewer data.
    /// </summary>
    public interface INfieldInterviewersService
    {
        /// <summary>
        /// Adds a new interviewer.
        /// </summary>
        /// <param name="interviewer">The interviewer to add.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<Interviewer> AddAsync(Interviewer interviewer);

        /// <summary>
        /// Removes the interviewer.
        /// </summary>
        /// <param name="interviewer">The interviewer to remove.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>       
        Task RemoveAsync(Interviewer interviewer);

        /// <summary>
        /// Updates interviewers data.
        /// </summary>
        /// <param name="interviewer">The interviewer to update.</param>
        /// <exception cref="T:System.AggregateException"></exception> 
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<Interviewer> UpdateAsync(Interviewer interviewer);

        /// <summary>
        /// Gets interviewer queryable object.
        /// <exception cref="T:System.AggregateException"></exception>
        /// </summary>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>   
        Task<IQueryable<Interviewer>> QueryAsync();

        /// <summary>
        /// Change the password of an interviewer
        /// </summary>
        /// <param name="interviewer">interviewer whose password to change</param>
        /// <param name="password">the new password</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>     
        Task<Interviewer> ChangePasswordAsync(Interviewer interviewer, string password);
    }
}
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
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read and update the cati interviewer data.
    /// </summary>
    public interface INfieldCatiInterviewersService
    {
        /// <summary>
        /// Adds a new cati interviewer.
        /// </summary>
        /// <param name="catiInterviewer">The cati interviewer to add.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<CatiInterviewer> AddAsync(CatiInterviewer catiInterviewer);

        /// <summary>
        /// Removes the cati interviewer.
        /// </summary>
        /// <param name="catiInterviewer">The cati interviewer to remove.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>       
        Task RemoveAsync(CatiInterviewer catiInterviewer);

        /// <summary>
        /// Gets cati interviewers queryable object.
        /// <exception cref="T:System.AggregateException"></exception>
        /// </summary>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>   
        Task<IQueryable<CatiInterviewer>> QueryAsync();

        /// <summary>
        /// Change the password of a cati interviewer
        /// </summary>
        /// <param name="catiInterviewer">cati interviewer whose password to change</param>
        /// <param name="password">the new password</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>     
        Task<CatiInterviewer> ChangePasswordAsync(CatiInterviewer catiInterviewer, string password);
    }
}

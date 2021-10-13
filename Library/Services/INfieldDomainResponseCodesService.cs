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
using System.Linq;
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to create, retrieve, update and delete <see cref="DomainResponseCode"/>.
    /// </summary>
    public interface INfieldDomainResponseCodesService
    {

        /// <summary>
        /// Gets all the domain response codes.
        /// </summary>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        /// <returns>A list with all domains response codes</returns>
        Task<IQueryable<DomainResponseCode>> QueryAsync();

        /// <summary>
        /// Gets that has the specified <paramref name="code"/>.
        /// </summary>
        /// <param name="code">The value of the domain response code</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        /// <returns>A <see cref="DomainResponseCode"/> or null if the domain does not exists 
        /// or the domain does not  response code with this code</returns>
        Task<DomainResponseCode> QueryAsync(int code);

        /// <summary>
        /// Adds the supplied <paramref name="responseCode"/> to the domain
        /// </summary>
        /// <param name="responseCode">The <see cref="DomainResponseCode"/> to add</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<DomainResponseCode> AddAsync(DomainResponseCode responseCode);

        /// <summary>
        /// Updates the supplied supplied <paramref name="responseCode"/>
        /// </summary>
        /// <param name="responseCode">The <see cref="DomainResponseCode"/> to update</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<DomainResponseCode> UpdateAsync(DomainResponseCode responseCode);

        /// <summary>
        /// Removes the domain response code with the supplied <paramref name="code"/> 
        /// </summary>
        /// <param name="domainId">The domain to remove response code from</param>
        /// <param name="code">The code of the response code to remove</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task RemoveAsync(int code);
    }
}
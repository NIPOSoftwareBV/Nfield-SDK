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
    /// Represents a set of methods to read and manage fieldwork offices.
    /// </summary>
    public interface INfieldFieldworkOfficesService
    {
        /// <summary>
        /// Gets fieldwork office queryable object.
        /// <exception cref="T:System.AggregateException"></exception>
        /// </summary>
        /// The aggregate exception can contain:
        /// <exception cref="Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Exceptions.NfieldHttpResponseException"></exception>   
        Task<IQueryable<FieldworkOffice>> QueryAsync();

        /// <summary>
        /// Adds a new fieldwork office.
        /// </summary>
        /// <param name="office">The fieldwork office to add.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Exceptions.NfieldHttpResponseException"></exception>
        Task<FieldworkOffice> AddAsync(FieldworkOffice office);

        /// <summary>
        /// Removes a fieldwork office.
        /// </summary>
        /// <param name="office">The fieldwork office to remove.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Exceptions.NfieldHttpResponseException"></exception>       
        Task RemoveAsync(FieldworkOffice office);

        /// <summary>
        /// Updates the details of a fieldwork office.
        /// </summary>
        /// <param name="office">The fieldwork office to update.</param>
        /// <exception cref="T:System.AggregateException"></exception> 
        /// The aggregate exception can contain:
        /// <exception cref="Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Exceptions.NfieldHttpResponseException"></exception>
        Task<FieldworkOffice> UpdateAsync(FieldworkOffice office);
    }
}

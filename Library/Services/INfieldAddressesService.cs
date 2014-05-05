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

using System.Linq;
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of list, update and delete addresses.
    /// </summary>
    public interface INfieldAddressesService
    {
        /// <summary>
        /// Gets all addresses for a surveys sampling point
        /// </summary>
        /// <param name="surveyId">The survey for which to return addresses</param>
        /// <param name="samplingPointId">The sampling point for which to return addresses</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<IQueryable<Address>> QueryAsync(string surveyId, string samplingPointId);

        /// <summary>
        /// Adds a new address.
        /// </summary>
        /// <param name="surveyId">The survey for which to add the address</param>
        /// <param name="samplingPointId">The sampling point for which to add the address</param>
        /// <param name="address">The address to add.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<Address> AddAsync(string surveyId, string samplingPointId, Address address);

        /// <summary>
        /// Deletes an address.
        /// </summary>
        /// <param name="surveyId">The survey for which to add the address</param>
        /// <param name="samplingPointId">The sampling point for which to add the address</param>
        /// <param name="addressId">The id of the address to delete.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task DeleteAsync(string surveyId, string samplingPointId, string addressId);
    }
}

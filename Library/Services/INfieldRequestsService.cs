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
    /// A service to manage request configuration
    /// </summary>
    public interface INfieldRequestsService
    {

        /// <summary>
        /// Gets request queryable object.
        /// </summary>
        Task<IQueryable<Request>> QueryAsync();

        /// <summary>
        /// Adds a new request.
        /// </summary>
        Task<Request> AddAsync(Request request);

        /// <summary>
        /// Removes the request.
        /// </summary>
        Task RemoveAsync(Request request);

        /// <summary>
        /// Updates the request.
        /// 
        /// All properties are updated.
        /// Existing headers not present in 'request' are deleted (matched on RequestHeader.Id)
        /// Existing headers present in 'request' with RequestHeader.IsObfuscated set to false
        /// have there value updated.
        /// Headers in 'request' where RequestHeader.Id is 0 are added.
        /// </summary>
        Task<Request> UpdateAsync(Request request);
    }
}

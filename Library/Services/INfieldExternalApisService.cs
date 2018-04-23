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
    public interface INfieldExternalApisService
    {

        /// <summary>
        /// Gets external api queryable object.
        /// </summary>
        Task<IQueryable<ExternalApi>> QueryAsync();

        /// <summary>
        /// Adds a new external api.
        /// </summary>
        Task<ExternalApi> AddAsync(ExternalApi externalApi);

        /// <summary>
        /// Removes the external api.
        /// </summary>
        Task RemoveAsync(ExternalApi externalApi);

        /// <summary>
        /// Updates the external api.
        /// 
        /// All properties are updated (except the name).
        /// All headers are replaced
        /// </summary>
        Task<ExternalApi> UpdateAsync(ExternalApi externalApi);
    }
}

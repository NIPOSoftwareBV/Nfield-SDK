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
using Nfield.SDK.Models.Delivery;

namespace Nfield.Services
{
    /// <summary>
    /// Set of methods to manage the respository surveys
    /// </summary>
    public interface INfieldDeliveryRepositorySurveysService
    {
        /// <summary>
        /// Returns the surveys of the given repository.
        /// </summary>
        /// <param name="repositoryId">The repository id.</param>
        /// <returns>The repository surveys.</returns>         
        Task<IQueryable<RepositorySurveyModel>> QueryAsync(long repositoryId);
    }
}

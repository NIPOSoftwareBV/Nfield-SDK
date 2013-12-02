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
    /// Represents a set of methods to read and update survey data.
    /// </summary>
    public interface INfieldLanguagesService
    {
        #region CRUD on Language

        /// <summary>
        /// Gets language queryable object.
        /// </summary>
        Task<IQueryable<Language>> QueryAsync(string surveyId);

        /// <summary>
        /// Adds a new language.
        /// </summary>
        Task<Language> AddAsync(Language language);

        /// <summary>
        /// Removes the language.
        /// </summary>
        Task RemoveAsync(Language language);

        /// <summary>
        /// Updates the language.
        /// 
        /// The only property that can be changed is Name
        /// </summary>
        Task<Language> UpdateAsync(Language language);

        #endregion
    }
}

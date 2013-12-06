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
    /// Represents a set of methods to read and update survey languages.
    /// </summary>
    public interface INfieldLanguagesService
    {
        #region CRUD on Language

        /// <summary>
        /// Gets language queryable object.
        /// </summary>
        /// <param name="surveyId">The survey for which languages are returned</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<IQueryable<Language>> QueryAsync(string surveyId);

        /// <summary>
        /// Adds a new language.
        /// </summary>
        /// <param name="surveyId">the survey to add this language to</param>
        /// <param name="language">the language to add</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<Language> AddAsync(string surveyId, Language language);

        /// <summary>
        /// Removes the language.
        /// </summary>
        /// <param name="surveyId">The survey to remove this language from</param>
        /// <param name="language">The languuage to remove</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task RemoveAsync(string surveyId, Language language);

        /// <summary>
        /// Updates the language.
        /// 
        /// The only property that can be changed is Name
        /// </summary>
        /// <param name="surveyId">The survey on which to update this language</param>
        /// <param name="language">The language to update</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task UpdateAsync(string surveyId, Language language);

        #endregion
    }
}

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
    /// Represents a set of methods to read and update survey translations.
    /// </summary>
    public interface INfieldTranslationsService
    {
        #region CRUD on Language

        /// <summary>
        /// Gets translation queryable object.
        /// </summary>
        /// <param name="surveyId">The survey for which translations are returned</param>
        /// <param name="languageId">The language to return</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<IQueryable<Translation>> QueryAsync(string surveyId, int languageId);

        /// <summary>
        /// Adds a new translatione.
        /// </summary>
        /// <param name="surveyId">the survey to add this translation to</param>
        /// <param name="languageId">The language to add this translation to</param>
        /// <param name="translation">the translation to add</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<Translation> AddAsync(string surveyId, int languageId, Translation translation);

        /// <summary>
        /// Removes the translation.
        /// </summary>
        /// <param name="surveyId">The survey to remove this translation from</param>
        /// <param name="languageId">The language to remove this translation from</param>
        /// <param name="translation">The translation to remove</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task RemoveAsync(string surveyId, int languageId, Translation translation);

        /// <summary>
        /// Updates the translation.
        /// 
        /// The only property that can be changed is Text
        /// </summary>
        /// <param name="surveyId">The survey on which to update this translation</param>
        /// <param name="languageId">The language to update</param>
        /// <param name="translation">The translation to update</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task UpdateAsync(string surveyId, int languageId, Translation translation);

        /// <summary>
        /// Gets the default for all texts
        /// </summary>
        Task<IQueryable<Translation>> DefaultTextsAsync { get; }

        #endregion
    }
}

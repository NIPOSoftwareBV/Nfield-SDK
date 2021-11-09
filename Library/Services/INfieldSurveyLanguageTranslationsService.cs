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
using Nfield.SDK.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read and update survey languages.
    /// </summary>
    public interface INfieldSurveyLanguageTranslationsService
    {
        #region CRUD on Language Translations

        /// <summary>
        /// Gets all languages of a survey, as a queryable object.
        /// </summary>
        /// <param name="surveyId">The survey for which language names and ids are returned</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        /// <returns>A list of language names and their ids.</returns>
        Task<IQueryable<Language>> QueryAsync(string surveyId);

        /// <summary>
        /// Adds a new survey language and all of its translations.
        /// </summary>
        /// <param name="surveyId">the survey to add the language to</param>
        /// <param name="translations">The translations to set</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        /// <returns>The new translations for the language</returns>
        Task<SurveyLanguageTranslations> AddAsync(string surveyId, SurveyLanguageTranslations translations);

        /// <summary>
        /// Updates translations of a survey language.
        /// </summary>
        /// <param name="surveyId">The survey on which to update the language</param>
        /// <param name="languageId">The language to update</param>
        /// <param name="translations">The translations to change</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        /// <returns>The updated translations for the language</returns>
        Task<SurveyLanguageTranslations> UpdateAsync(string surveyId, int languageId, SurveyLanguageTranslations translations);

        /// <summary>
        /// Removes a survey language and all of its translations.
        /// </summary>
        /// <param name="surveyId">The survey to remove this language from</param>
        /// <param name="languageId">The survey language to remove</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task RemoveAsync(string surveyId, int languageId);

        #endregion
    }
}

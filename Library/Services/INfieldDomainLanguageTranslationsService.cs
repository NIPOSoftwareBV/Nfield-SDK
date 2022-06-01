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
using Nfield.SDK.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read and update domain languages.
    /// Using LanguageTranslations to keep the same name used for surveys
    /// </summary>
    public interface INfieldDomainLanguageTranslationsService
    {
        #region CRUD on Language Translations

        /// <summary>
        /// Gets all languages of a domain, as a queryable object.
        /// </summary>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        /// <returns>A list of languages with their translations</returns>
        Task<IQueryable<DomainLanguageTranslations>> QueryAsync();

        /// <summary>
        /// Adds a new domain language and all of its translations.
        /// </summary>
        /// <param name="translations">The translations to set</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        /// <returns>The new translations for the language</returns>
        Task<DomainLanguageTranslations> AddAsync(DomainLanguageTranslations translations);

        /// <summary>
        /// Updates translations of a domain language.
        /// </summary>
        /// <param name="languageId">The language to update</param>
        /// <param name="translations">The translations to change</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        /// <returns>The updated translations for the language</returns>
        Task<DomainLanguageTranslations> UpdateAsync(int languageId, DomainLanguageTranslations translations);

        /// <summary>
        /// Removes a domain language and all of its translations.
        /// </summary>
        /// <param name="languageId">The domain language to remove</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task RemoveAsync(int languageId);

        #endregion
    }
}

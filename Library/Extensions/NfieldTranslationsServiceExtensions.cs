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
using Nfield.Models;
using Nfield.Services;
using Nfield.Services.Implementation;

namespace Nfield.Extensions
{
    /// <summary>
    /// Extensions to make the asynchronous methods of <see cref="NfieldTranslationsService"/> synchronous
    /// </summary>
    public static class NfieldTranslationsServiceExtensions
    {
        /// <summary>
        /// A synchronous version of <see cref="INfieldLanguagesService.AddAsync"/>
        /// </summary>
        public static Translation Add(this INfieldTranslationsService translationsService, string surveyId, int languageId, Translation translation)
        {
            return translationsService.AddAsync(surveyId, languageId, translation).Result;
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldTranslationsService.RemoveAsync"/>
        /// </summary>
        public static void Remove(this INfieldTranslationsService languagesService, string surveyId, int languageId, Translation translation)
        {
            languagesService.RemoveAsync(surveyId, languageId, translation).Wait();
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldTranslationsService.UpdateAsync"/>
        /// </summary>
        public static void Update(this INfieldTranslationsService languagesService, string surveyId, int languageId, Translation translation)
        {
            languagesService.UpdateAsync(surveyId, languageId, translation).Wait();
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldTranslationsService.QueryAsync"/>
        /// </summary>
        public static IQueryable<Translation> Query(this INfieldTranslationsService languagesService, string surveyId, int languageId)
        {
            return languagesService.QueryAsync(surveyId, languageId).Result;
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldTranslationsService.DefaultTextsAsync"/>
        /// </summary>
        public static IQueryable<Translation> DefaultTexts(this INfieldTranslationsService languagesService)
        {
            return languagesService.DefaultTextsAsync.Result;
        }
    }
}

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
    /// Extensions to make the asynchronous methods of <see cref="NfieldLanguagesService"/> synchronous
    /// </summary>
    public static class NfieldLanguagesServiceExtensions
    {
        /// <summary>
        /// A synchronous version of <see cref="INfieldLanguagesService.AddAsync"/>
        /// </summary>
        public static Language Add(this INfieldLanguagesService languagesService, string surveyId, Language language)
        {
            return languagesService.AddAsync(surveyId, language).Result;
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldLanguagesService.RemoveAsync"/>
        /// </summary>
        public static void Remove(this INfieldLanguagesService languagesService, string surveyId, Language language)
        {
            languagesService.RemoveAsync(surveyId, language).Wait();
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldLanguagesService.UpdateAsync"/>
        /// </summary>
        public static void Update(this INfieldLanguagesService languagesService, string surveyId, Language language)
        {
            languagesService.UpdateAsync(surveyId, language).Wait();
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldLanguagesService.QueryAsync"/>
        /// </summary>
        public static IQueryable<Language> Query(this INfieldLanguagesService languagesService, string surveyId)
        {
            return languagesService.QueryAsync(surveyId).Result;
        }
    }
}

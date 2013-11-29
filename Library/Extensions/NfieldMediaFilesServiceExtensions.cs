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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nfield.Models;
using Nfield.Services;
using Nfield.Services.Implementation;

namespace Nfield.Extensions
{
    /// <summary>
    /// Extensions to make the asynchronous methods of <see cref="INfieldMediaFilesService"/> synchronous
    /// </summary>
    public static class NfieldMediaFilesServiceExtensions
    {
        /// <summary>
        /// A synchronous version of <see cref="INfieldMediaFilesService.QueryAsync"/>
        /// </summary>
        public static IQueryable<string> Query(this INfieldMediaFilesService mediaFilesService,
                string surveyId)
        {
            return mediaFilesService.QueryAsync(surveyId).Result;
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldMediaFilesService.RemoveAsync"/>
        /// </summary>
        public static void Remove(this INfieldMediaFilesService mediaFilesService,
                string surveyId, string fileName)
        {
            mediaFilesService.RemoveAsync(surveyId, fileName).Wait();
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldMediaFilesService.AddOrUpdateAsync"/>
        /// </summary>
        public static void AddOrUpdate(this INfieldMediaFilesService mediaFilesService,
                string surveyId, string fileName, byte[] content)
        {
            mediaFilesService.AddOrUpdateAsync(surveyId, fileName, content).Wait();
        }
    }
}

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
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read the template themes data.
    /// </summary>
    public interface INfieldThemesService
    {
        /// <summary>
        /// Gets theme file.
        /// <exception cref="T:System.AggregateException"></exception>
        /// </summary>
        /// <param name="theme">Theme data to download it</param>
        /// <param name="filePath">Path where the theme will be saved.</param>
        /// <param name="overwrite">Overwrite existing file or not.</param>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>   
        Task DownloadThemeAsync(Theme theme, string filePath, bool overwrite);

        /// <summary>
        /// This method upload a theme.
        /// </summary>
        /// <param name="theme">Theme data to upload it</param>
        /// <param name="filePath">Path of theme file</param>
        /// /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>       
        Task UploadThemeAsync(Theme theme, string filePath);

        /// <summary>
        /// Removes the theme.
        /// </summary>
        /// <param name="theme">The theme to remove.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>       
        Task RemoveAsync(Theme theme);
    }
}

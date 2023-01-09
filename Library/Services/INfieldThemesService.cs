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

using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read the template themes data.
    /// </summary>
    public interface INfieldThemesService
    {
        /// <summary>
        /// Gets Download URL for the theme file.
        /// <exception cref="T:System.AggregateException"></exception>
        /// </summary>
        /// <param name="themeId">The theme id to download</param>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>   
        Task<string> DownloadThemeAsync(string themeId);

        /// <summary>
        /// This method upload a theme.
        /// </summary>
        /// <param name="templateId">The template the theme belongs to</param>
        /// <param name="themeName">The name of the theme upload</param>
        /// <param name="filePath">Path of theme file</param>
        /// /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>       
        Task UploadThemeAsync(string templateId, string themeName, string filePath);

        /// <summary>
        /// This method upload a theme.
        /// </summary>
        /// <param name="templateId">The template the theme belongs to</param>
        /// <param name="themeName">The name of the theme upload</param>
        /// <param name="themeContent">The content of the theme</param>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>   
        Task UploadThemeAsync(string templateId, string themeName, byte[] themeContent);

        /// <summary>
        /// Removes the theme.
        /// </summary>
        /// <param name="themeId">The theme id to remove.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>       
        Task RemoveAsync(string themeId);
    }
}

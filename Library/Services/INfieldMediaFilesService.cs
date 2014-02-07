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

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to write, update and delete survey media files.
    /// </summary>
    public interface INfieldMediaFilesService
    {
        /// <summary>
        /// Gets all media file names for a survey
        /// </summary>
        /// <param name="surveyId">The survey for which to return media files</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<IQueryable<string>> QueryAsync(string surveyId);

        /// <summary>
        /// Gets the needed media file for a survey
        /// </summary>
        /// <param name="surveyId">The survey for which to return media files</param>
        /// <param name="fileName">The name of the needed file</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<byte[]> GetAsync(string surveyId, string fileName);

        /// <summary>
        /// Removes the survey media file.
        /// </summary>
        /// <param name="surveyId">The survey to remove this file from</param>
        /// <param name="fileName">The file to remove</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task RemoveAsync(string surveyId, string fileName);

        /// <summary>
        /// Adds or updates the survey media file.
        /// </summary>
        /// <param name="surveyId">The survey for which to add or update the file</param>
        /// <param name="fileName">The name of the file</param>
        /// <param name="content">The content of the file</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task AddOrUpdateAsync(string surveyId, string fileName, byte[] content);
    }
}

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

using System.IO;
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to upload landing page zip files.
    /// </summary>
    public interface INfieldSurveyLandingPageService
    {
        /// <summary>
        /// Uploads a landing page zip file for a specific survey from a file path.
        /// </summary>
        /// <param name="surveyId">The ID of the survey.</param>
        /// <param name="filePath">The path to the zip file.</param>
        /// <returns>The activity ID of the upload operation.</returns>
        /// <exception cref="T:System.AggregateException"></exception>
        Task<SurveyLandingPageUploadStatusResponseModel> UploadLandingPageAsync(string surveyId, string filePath);

        /// <summary>
        /// Uploads a landing page zip file for a specific survey.
        /// </summary>
        /// <param name="surveyId">The ID of the survey.</param>
        /// <param name="fileName">The name of the zip file.</param>
        /// <param name="content">The content of the zip file as a stream.</param>
        /// <returns>The activity ID of the upload operation.</returns>
        /// <exception cref="T:System.AggregateException"></exception>
        Task<SurveyLandingPageUploadStatusResponseModel> UploadLandingPageAsync(string surveyId, string fileName, Stream content);

        /// <summary>
        /// Exports the landing page of a specific survey.
        /// </summary>
        /// <param name="surveyId">The ID of the survey.</param>
        /// <returns>The status and download URL of the exported landing page.</returns>
        /// <exception cref="T:System.AggregateException"></exception>
        Task<SurveyLandingPageExportStatusResponseModel> ExportLandingPageAsync(string surveyId);
    }
}

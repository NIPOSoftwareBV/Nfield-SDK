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
using System.IO;
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read and update the script for a survey.
    /// </summary>
    public interface INfieldSurveyScriptService
    {
        /// <summary>
        /// Gets the script for survey.
        /// </summary>
        Task<SurveyScript> GetAsync(string surveyId);

        /// <summary>
        /// Upload script for survey.
        /// The repeated upload for the survey just replace the previous script. 
        /// </summary>
        /// <returns>A Task that can be used to find out the result of the action</returns>
        /// <exception cref="ArgumentNullException">In case that <paramref name="surveyScript"/> is null</exception>
        Task<SurveyScript> PostAsync(string surveyId, SurveyScript surveyScript);

        /// <summary>
        /// Uploads the file that contains the instructions for the various interviewers.
        /// The repeated upload for the survey just replace the previous script.
        /// </summary>
        /// <param name="surveyId">The id of the survey the instructions are about</param>
        /// <param name="filePath">The path to the file</param>
        /// <returns>A Task that can be used to find out the result of the action</returns>
        /// <exception cref="FileNotFoundException">In case that the file does not exists</exception>
        Task<SurveyScript> PostAsync(string surveyId, string filePath);
    }
}

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
    /// Represents a set of methods to download, upload and delete sample.
    /// </summary>
    public interface INfieldSurveySampleService
    {
        /// <summary>
        /// start task to download sample
        /// </summary>
        /// <param name="surveyId">The id of the survey</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>task</returns>
        Task<string> GetAsync(string surveyId);

        /// <summary>
        /// upload csv sample
        /// new respondents will be added
        /// existing repondents will be updated
        /// </summary>
        /// <param name="surveyId">The id of the survey</param>
        /// <param name="sample">sample</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>An message indicating the status of the action</returns>
        Task<SampleUploadStatus> PostAsync(string surveyId, string sample);

        /// <summary>
        /// delete a sample record
        /// </summary>
        /// <param name="surveyId">The id of the survey</param>
        /// <param name="respondentKey">the id of the sample record</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>An message indicating the status of the action</returns>
        Task<string> DeleteAsync(string surveyId, string respondentKey);
    }
}

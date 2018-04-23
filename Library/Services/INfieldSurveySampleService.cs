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
        /// <returns><see cref="SampleUploadStatus"/></returns>
        Task<SampleUploadStatus> PostAsync(string surveyId, string sample);

        /// <summary>
        /// delete a sample record
        /// </summary>
        /// <param name="surveyId">The id of the survey</param>
        /// <param name="respondentKey">the id of the sample record</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>number of deleted sample records</returns>
        Task<int> DeleteAsync(string surveyId, string respondentKey);

        /// <summary>
        /// Block the specified respondent in the survey
        /// </summary>
        /// <param name="surveyId">The id of the survey</param>
        /// <param name="respondentKey">The id of the respondent to be blocked</param>
        /// <returns>The number of respondents successfully blocked</returns>
        Task<int> BlockAsync(string surveyId, string respondentKey);

        /// <summary>
        /// Reset the specified respondent in the survey
        /// </summary>
        /// <param name="surveyId">The id of the survey</param>
        /// <param name="respondentKey">The id of the respondent to be reset</param>
        /// <returns>The number of respondents successfully reset</returns>
        Task<int> ResetAsync(string surveyId, string respondentKey);

        /// <summary>
        /// Clears the specified columns for the specified respondent in the survey.
        /// </summary>
        /// <param name="surveyId">The id of the survey</param>
        /// <param name="respondentKey">The id of the respondent to be cleared</param>
        /// <param name="columnsToClear">The name of the columns to be cleared</param>
        /// <returns>The number of respondents successfully clear</returns>
        Task<int> ClearByRespondentAsync(string surveyId, string respondentKey, IEnumerable<string> columnsToClear);

        /// <summary>
        /// Clears the specified columns for the specified interview in the survey.
        /// </summary>
        /// <param name="surveyId">The id of the survey</param>
        /// <param name="interviewId">The interview id to be cleared</param>
        /// <param name="columnsToClear">The name of the columns to be cleared</param>
        /// <returns>The number of respondents successfully clear</returns>
        Task<int> ClearByInterviewAsync(string surveyId, int interviewId, IEnumerable<string> columnsToClear);

        /// <summary>
        /// Updates the specified custom columns for the specified record in the survey.
        /// </summary>
        /// <param name="surveyId">The id of the survey</param>
        /// <param name="sampleRecordId">The id of the sample record to be update</param>
        /// <param name="columnsToUpdate">The name and values of the columns to be updated</param>
        /// <returns>True if success, false otherwise</returns>
        Task<bool> UpdateAsync(string surveyId, int sampleRecordId, IEnumerable<SampleColumnUpdate> columnsToUpdate);
    }
}

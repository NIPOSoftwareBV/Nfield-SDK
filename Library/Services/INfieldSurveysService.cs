﻿//    This file is part of Nfield.SDK.
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

using Nfield.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read and update survey data.
    /// </summary>
    public interface INfieldSurveysService
    {
        #region CRUD on Survey

        /// <summary>
        /// Gets survey queryable object.
        /// </summary>
        Task<IQueryable<Survey>> QueryAsync();

        /// <summary>
        /// Adds a new survey.
        /// </summary>
        Task<Survey> AddAsync(Survey survey);

        /// <summary>
        /// Adds a new survey based on a blueprint survey.
        /// </summary>
        Task<Survey> AddFromBlueprintAsync(string blueprintSurveyId, string surveyName);

        /// <summary>
        /// Updates an existing blueprint survey based on a survey
        /// </summary>
        Task UpdateBlueprintFromSurveyAsync(string blueprintSurveyId, string surveyId, CopyableSurveyConfiguration includedConfiguration = CopyableSurveyConfiguration.All);

        /// <summary>
        /// Removes the survey.
        /// </summary>
        Task RemoveAsync(Survey survey);

        /// <summary>
        /// Updates the survey.
        /// 
        /// The only properties that can be changed are
        /// SurveyName, ClientName and Description
        /// </summary>
        Task<Survey> UpdateAsync(Survey survey);

        #endregion

        #region Interviewer Instructions file

        /// <summary>
        /// Uploads the file that contains the instructions for the various interviewers.
        /// The repeated upload of the file for the survey just replace the previous file. 
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <param name="surveyId">The id of the survey the instructions are about</param>
        /// <returns>A Task that can be used to find out the result of the action</returns>
        /// <exception cref="FileNotFoundException">In case that the file does not exists</exception>
        Task UploadInterviewerFileInstructionsAsync(string filePath, string surveyId);

        /// <summary>
        /// Uploads the file that contains the instructions for the various interviewers.
        /// The repeated upload of the file for the survey just replace the previous file. 
        /// </summary>
        /// <param name="fileContent">The content of the file</param>
        /// <param name="fileName">The file name</param>
        /// <param name="surveyId">The id of the survey the instructions are about</param>
        /// <returns>A Task that can be used to find out the result of the action</returns>
        Task UploadInterviewerFileInstructionsAsync(byte[] fileContent, string fileName, string surveyId);

        /// <summary>
        /// Downloads the file that contains the instructions for the various interviewers.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        Task<InterviewerInstruction> DownloadInterviewerFileInstructionsAsync(string surveyId);

        /// <summary>
        /// Delete the file that contains the instructions for the various interviewers.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        Task DeleteInterviewerFileInstructionsAsync(string surveyId);

        #endregion

        #region Quota for a survey

        /// <summary>
        /// Gets quota frame definition for survey.
        /// </summary>
        Task<SDK.Models.QuotaFrame> QuotaTargetsQueryAsync(string surveyId);

        /// <summary>
        /// Gets quota frame definition for survey, using eTag version
        /// </summary>
        Task<SDK.Models.QuotaFrame> QuotaTargetsQueryAsync(string surveyId, string eTag);

        #endregion

        #region Counts
        /// <summary>
        /// Get counts for given survey
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        Task<SurveyCounts> CountsQueryAsync(string surveyId);

        #endregion

        #region Sampling Points for a survey

        /// <summary>
        /// Gets sampling point's quota targets queryable object
        /// </summary>
        /// <param name="surveyId">id of the survey</param>
        /// <param name="samplingPointId">id of the sampling point</param>
        /// <returns></returns>
        Task<IQueryable<SamplingPointQuotaTarget>> SamplingPointQuotaTargetsQueryAsync(string surveyId, string samplingPointId);

        /// <summary>
        /// Gets a specific sampling point's quota target
        /// </summary>
        /// <param name="surveyId">id of the survey</param>
        /// <param name="samplingPointId">id of the sampling point</param>
        /// <param name="levelId">id of the qouta level</param>
        /// <returns></returns>
        Task<SamplingPointQuotaTarget> SamplingPointQuotaTargetQueryAsync(string surveyId, string samplingPointId, string levelId);

        /// <summary>
        /// Updates sampling point's quota target
        /// </summary>
        /// <param name="surveyId">id of the survey</param>
        /// <param name="samplingPointId">id of the sampling point</param>
        /// <param name="samplingPointQuotaTarget"></param>
        /// <returns></returns>
        Task<SamplingPointQuotaTarget> SamplingPointQuotaTargetUpdateAsync(string surveyId, string samplingPointId,
            SamplingPointQuotaTarget samplingPointQuotaTarget);

        /// <summary>
        /// Method used to upload an image file associated 
        /// with a sampling point (e.g. a map).
        /// The upload of a new image file for an existing
        /// sampling point will replace the image.
        /// </summary>
        /// <param name="surveyId">The id of the survey that the sampling point belongs to</param>
        /// <param name="samplingPointId">The id of the sampling point</param>
        /// <param name="filePath">The full path of the image file</param>
        /// <returns>An message indicating the status of the action</returns>
        Task<string> SamplingPointImageAddAsync(string surveyId, string samplingPointId, string filePath);

        /// <summary>
        /// Method used to upload an image file associated 
        /// with a sampling point (e.g. a map).
        /// The upload of a new image file for an existing
        /// sampling point will replace the image.
        /// </summary>
        /// <param name="surveyId">The id of the survey that the sampling point belongs to</param>
        /// <param name="samplingPointId">The id of the sampling point</param>
        /// <param name="filename">name of the image file</param>
        /// <param name="content">The content of the image file</param>
        /// <returns>An message indicating the status of the action</returns>
        Task<string> SamplingPointImageAddAsync(string surveyId, string samplingPointId, string filename, byte[] content);

        /// <summary>
        /// Downloads the image file associated with a sampling point
        /// </summary>
        /// <param name="surveyId">The id of the survey that the sampling point belongs to</param>
        /// <param name="samplingPointId">The id of the sampling point</param>
        /// <returns></returns>
        Task<SamplingPointImage> SamplingPointImageGetAsync(string surveyId, string samplingPointId);

        /// <summary>
        /// Delete the image file associated with a sampling point
        /// </summary>
        /// <param name="surveyId">The id of the survey that the sampling point belongs to</param>
        /// <param name="samplingPointId">The id of the sampling point</param>
        /// <returns></returns>
        Task SamplingPointImageDeleteAsync(string surveyId, string samplingPointId);

        #endregion

        #region DialMode

        Task<SDK.Models.DialMode> GetDialModeAsync(string surveyId);

        Task SetDialModeAsync(string surveyId, SDK.Models.DialMode dialMode);

        #endregion
    }

}
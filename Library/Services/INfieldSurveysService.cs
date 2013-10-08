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
using System.Linq;
using System.Threading.Tasks;
using Nfield.Models;

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
        /// Gets an extended survey object.
        /// </summary>
        Task<ExtendedSurvey> GetExtendedAsync(string surveyId);

        /// <summary>
        /// Adds a new survey.
        /// </summary>
        Task<ExtendedSurvey> AddAsync(ExtendedSurvey survey);

        /// <summary>
        /// Removes the survey.
        /// </summary>
        Task RemoveAsync(Survey survey);

        /// <summary>
        /// Updates the survey.
        /// </summary>
        Task<ExtendedSurvey> UpdateAsync(ExtendedSurvey survey);

        #endregion

        #region Quota for a survey

        /// <summary>
        /// Gets quota definition for survey.
        /// </summary>
        Task<QuotaLevel> QuotaQueryAsync(string surveyId);

        #endregion

        #region Sampling Points for a survey

        /// <summary>
        /// Gets sampling point queryable object.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        Task<IQueryable<SamplingPoint>> SamplingPointsQueryAsync(string surveyId);

        /// <summary>
        /// Gets a specific sampling point.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="samplingPointId"></param>
        /// <returns></returns>
        Task<SamplingPoint> SamplingPointQueryAsync(string surveyId, string samplingPointId);

        /// <summary>
        /// Updates a sampling point
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="samplingPoint"></param>
        /// <returns></returns>
        Task<SamplingPoint> SamplingPointUpdateAsync(string surveyId, SamplingPoint samplingPoint);

        /// <summary>
        /// Adds a sampling point
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="samplingPoint"></param>
        /// <returns></returns>
        Task<SamplingPoint> SamplingPointAddAsync(string surveyId, SamplingPoint samplingPoint);

        /// <summary>
        /// Deletes a sampling point.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="samplingPoint"></param>
        /// <returns></returns>
        Task SamplingPointDeleteAsync(string surveyId, SamplingPoint samplingPoint);

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

        #endregion

    }
}

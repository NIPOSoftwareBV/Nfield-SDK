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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nfield.Models;
using Nfield.Quota;
using Nfield.SDK.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read and update survey data.
    /// </summary>
    public interface INfieldSurveyQuotaFrameService
    {
        /// <summary>
        /// Gets quota definition for survey.
        /// </summary>
        /// <param name="surveyId">The survey to get the quota frame</param>
        /// <returns>The quota frame for the requested survey</returns>
        Task<SurveyQuotaFrameModel> QuotaQueryAsync(string surveyId);

        /// <summary>
        /// Assigns the supplied <paramref name="quotaFrame"/> to the survey with the provided <paramref name="surveyId"/>.
        /// When this method is called on a survey that has a quota frame already 
        /// then the frame is completely replaced by the new one.
        /// </summary>
        /// <param name="surveyId">The survey to set the quota frame</param>
        /// <param name="quotaFrame">The new quota frame </param>
        /// <returns>The created/updated quota frame</returns>
        Task<SurveyQuotaFrameModel> CreateOrUpdateQuotaAsync(string surveyId, SurveyQuotaFrameModel quotaFrame);

        /// <summary>
        /// Updates the survey quota targets for the specified quota frame version
        /// </summary>
        /// <param name="surveyId">The survey to set the quota targets for</param>
        /// <param name="eTag">The quota frame version to set the targets for</param>
        /// <param name="targets">List of levels with the new quota frame targets</param>
        /// <returns>The updated quota frame levels target list</returns>
        Task<SurveyQuotaFrameEtagModel> UpdateQuotaTargetsAsync(string surveyId, string eTag, SurveyQuotaFrameEtagModel levelstarget);
    }
}
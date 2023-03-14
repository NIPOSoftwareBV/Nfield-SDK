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

using Nfield.Models;
using System.Threading.Tasks;

namespace Nfield.Services
{

    /// <summary>
    /// Represents a set of methods to get and update  <see cref="surveyGeneralSetting"/>.
    /// </summary>
    public interface INfieldSurveyGeneralSettingsService
    {
        /// <summary>
        /// This method returns SurveyGeneralSetting
        /// </summary>
        /// <param name="surveyId">The id of the survey to get the SurveyGeneralSetting</param>
        Task<SurveyGeneralSettingsResponse> QueryAsync(string surveyId);

        /// <summary>
        /// Updates the supplied <paramref name="generalSetting"/>
        /// </summary>
        /// <param name="generalSetting">The <see cref="surveyGeneralSetting"/> to update</param>
        Task UpdateAsync(string surveyId, SurveyGeneralSettings generalSetting);

        /// <summary>
        /// Get the Survey Owner
        /// </summary>
        /// <param name="surveyId">The id of the survey</param>
        /// <returns>The owner of the survey</returns>
        Task<SurveyGeneralSettingsOwner> GetOwnerAsync(string surveyId);

        /// <summary>
        /// Update the survey owner
        /// </summary>
        /// <param name="surveyId">The id of the survey</param>
        /// <param name="userId">The user id of the new owner of the survey. It can be set to null to remove the active Owner</param>
        /// <returns>The new survey owner</returns>
        Task<SurveyGeneralSettingsOwner> UpdateOwnerAsync(string surveyId, string userId);        
    }
}



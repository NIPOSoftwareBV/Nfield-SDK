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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Service used to list, update, create and delete survey groups.
    /// </summary>
    public interface INfieldSurveyGroupService
    {
        /// <summary>
        /// Retrieve a specific survey group
        /// </summary>
        /// <param name="surveyGroupId"></param>
        Task<SurveyGroup> GetAsync(int surveyGroupId);

        /// <summary>
        /// Lists all survey groups in the domain.
        /// </summary>
        Task<IEnumerable<SurveyGroup>> GetAllAsync();

        /// <summary>
        /// Lists all surveys in the specified survey group.
        /// </summary>
        /// <param name="surveyGroupId">The id of the group to list the surveys for.</param>
        Task<IEnumerable<Survey>> GetSurveysAsync(int surveyGroupId);

        /// <summary>
        /// Creates a new survey group.
        /// </summary>
        /// <param name="model">Details of the group to create.</param>
        Task<SurveyGroup> CreateAsync(SurveyGroupValues model);

        /// <summary>
        /// Updates an existing survey group. Null values are ignored.
        /// </summary>
        /// <param name="model">The new values for the survey group.</param>
        Task<SurveyGroup> UpdateAsync(int surveyGroupId, SurveyGroupValues model);

        /// <summary>
        /// Deletes the specified survey group.
        /// </summary>
        /// <param name="surveyGroupId">The id of the group to delete.</param>
        Task DeleteAsync(int surveyGroupId);
    }
}

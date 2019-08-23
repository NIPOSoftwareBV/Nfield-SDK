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

using Nfield.SDK.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nfield.SDK.Services
{
    interface INfieldSurveyGroupAssignmentsService
    {
        /// <summary>
        /// Lists all survey group native assignments in the domain.
        /// </summary>
        Task<IEnumerable<SurveyGroupNativeAssignment>> GetLocalAssignmentsAsync(int surveyGroupId);

        /// <summary>
        /// Lists all survey group directory assignments in the domain.
        /// </summary>
        Task<IEnumerable<SurveyGroupDirectoryAssignment>> GetDirectoryAssignmentsAsync(int surveyGroupId);

        /// <summary>
        /// Assign native identity to survey group
        /// </summary>
        Task<SurveyGroupNativeAssignment> AssignLocalAsync(int surveyGroupId, string nativeIdentityId);

        /// <summary>
        /// Assign directory identity to survey group
        /// </summary>
        Task<SurveyGroupDirectoryAssignment> AssignDirectoryAsync(int surveyGroupId, DirectoryIdentityModel model);

        /// <summary>
        /// Unassign native identity from survey group
        /// </summary>
        Task UnassignLocalAsync(int surveyGroupId, string nativeIdentityId);

        /// <summary>
        /// Unassign directory identity from survey group
        /// </summary>
        Task UnassignDirectoryAsync(int surveyGroupId, DirectoryIdentityModel model);
    }
}

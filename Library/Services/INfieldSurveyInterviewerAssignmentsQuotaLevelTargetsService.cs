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

using System.Collections.Generic;
using System.Threading.Tasks;
using Nfield.SDK.Models;

namespace Nfield.Services
{
    public interface INfieldSurveyInterviewerAssignmentsQuotaLevelTargetsService
    {
        /// <summary>
        /// Update the quota level targets of the assigned interviewer
        /// </summary>
        /// <param name="surveyId">The id of the survey.</param>
        /// <param name="interiewerId">The id of the interviewer.</param>
        /// <param name="workPackageTargets">Object containing the target and level to set.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task UpdateAsync(string surveyId, string interviewerId, IEnumerable<WorkPackageTarget> workPackageTargets);
    }
}
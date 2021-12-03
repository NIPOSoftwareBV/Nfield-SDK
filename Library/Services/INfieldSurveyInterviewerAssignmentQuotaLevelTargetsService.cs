using System.Collections.Generic;
using System.Threading.Tasks;
using Nfield.SDK.Models;

namespace Nfield.SDK.Services
{
    public interface INfieldSurveyInterviewerAssignmentQuotaLevelTargetsService
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
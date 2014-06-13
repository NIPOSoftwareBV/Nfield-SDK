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
    /// Represents a set of methods to manage assignements of interviewers to surveys.
    /// </summary>
    public interface INfieldSurveyInterviewersService
    {
        /// <summary>
        /// Get the list of interviewers that are added to a survey.
        /// </summary>
        /// <param name="surveyId">The id of the survey.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<IQueryable<SurveyInterviewerAssignmentModel>> GetAsync(string surveyId);

        /// <summary>
        /// Add an interviewer to a survey.
        /// </summary>
        /// <param name="surveyId">The id of the survey for which to add the interviewer.</param>
        /// <param name="interviewerId">The id of the interviewer to add.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task AddAsync(string surveyId, string interviewerId);
    }
}
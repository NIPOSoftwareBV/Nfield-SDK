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
//    along with Nfield.SDK.  If not, see <http://www.gnu.org/licenses/>.using System;

using System.Linq;
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Service to manage interview quality
    /// </summary>
    public interface INfieldInterviewQualityService
    {
        /// <summary>
        /// Query a specific interview
        /// </summary>

        Task<InterviewDetailsModel> QueryAsync(string surveyId, string interviewId);

        /// <summary>
        /// Query all interviews
        /// </summary>

        Task<IQueryable<InterviewDetailsModel>> QueryAsync(string surveyId);

        /// <summary>
        /// Update current interview quality state to new quality state
        /// </summary>
        Task<InterviewDetailsModel> PutAsync(string surveyId, string interviewId, int newQualityState);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="interviewDetails"></param>
        /// <returns></returns>
        Task<InterviewDetailsModel> PutAsync(string surveyId, InterviewDetailsModel interviewDetails);
    }
}

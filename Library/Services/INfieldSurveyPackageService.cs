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
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Service to query and manage survey packages
    /// </summary>
    public interface INfieldSurveyPackageService
    {
        /// <summary>
        /// Returns the survey package based on the package type for the specified survey
        /// </summary>
        /// <param name="surveyId">The id if the survey to get the package for</param>
        /// <param name="interviewPackageType">The interview package type eg, Live or Test</param>
        /// <returns>A Task returning the survey package</returns>
        Task<SurveyPackage> GetSurveyPackageAsync(string surveyId, InterviewPackageType interviewPackageType);
    }
}

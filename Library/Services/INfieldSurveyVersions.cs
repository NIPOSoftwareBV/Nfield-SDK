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

namespace Nfield.Services
{
    /// <summary>
    /// Service to query and get the versions for a specific survey 
    /// </summary>
    public interface INfieldSurveyVersionsService
    {
        /// <summary>
        /// Returns the versions for the specified survey
        /// </summary>
        /// <param name="surveyId">The id of the survey to get the versions.</param>
        /// <returns>A Task returning the survey package</returns>
        System.Threading.Tasks.Task<IEnumerable<SurveyVersion>> GetSurveyPackageAsync(string surveyId);
    }
}

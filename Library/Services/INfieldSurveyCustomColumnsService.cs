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

namespace Nfield.SDK.Services
{
    public interface INfieldSurveyCustomColumnsService
    {
        /// <summary>
        /// Returns the custom column names for the specified survey
        /// </summary>
        /// <param name="surveyId">The id if the survey to get the column names for</param>
        /// <returns>A Task returning the column names</returns>
        Task<IEnumerable<string>> GetSurveyCustomColumnsAsync(string surveyId);
    }
}

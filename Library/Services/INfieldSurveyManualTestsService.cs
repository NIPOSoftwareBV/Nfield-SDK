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
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Service for managing manual test surveys
    /// </summary>
    public interface INfieldSurveyManualTestsService
    {
        /// <summary>
        /// Get All manual tests of a domain
        /// </summary>
        Task<IQueryable<SurveyManualTest>> GetManualTestsAsync();

        /// <summary>
        /// Get manual tests of a specific survey (normally just one)
        /// </summary>
        Task<IQueryable<SurveyManualTest>> GetSurveyManualTestsAsync(string surveyId);

        /// <summary>
        /// Get manual test of a specific survey
        /// </summary>
        Task<SurveyManualTest> GetSurveyManualTestAsync(string surveyId, string manualTestId);

        /// <summary>
        /// Start creating a manual test survey based on a specific survey
        /// </summary>
        Task<ManualTestSurveyResult> StartCreateManualTestSurveyAsync(string surveyId, StartCreateManualTestSurvey request);

        /// <summary>
        /// Start creating a manual test survey based on a specific survey
        /// This one allows to upload sample data directly from a file
        /// </summary>
        Task<ManualTestSurveyResult> StartCreateManualTestSurveyAsync(string surveyId, StartCreateManualTestSurveyFile request);
    }
}

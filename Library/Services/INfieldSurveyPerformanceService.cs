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
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read survey performance metrics.
    /// </summary>
    public interface INfieldSurveyPerformanceService
    {
        /// <summary>
        /// Get the survey performance metrics, for the live interviews, that indicate how many times the survey has reached the warn and block thresholds of their respective metric.
        /// </summary>
        /// <param name="surveyId">The id of the survey</param>
        /// <returns>The survey metrics containing the amount of times the survey reached the thresholds of warn and block</returns>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Exceptions.NfieldHttpResponseException"></exception>
        Task<SurveyMetrics> GetLiveMetricsAsync(string surveyId);

        /// <summary>
        /// Get the survey performance metrics, for the test inteviews, that indicate how many times the survey has reached the warn and block thresholds of their respective metric.
        /// </summary>
        /// <param name="surveyId">The id of the survey</param>
        /// <returns>The survey metrics containing the amount of times the survey reached the thresholds of warn and block</returns>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Exceptions.NfieldHttpResponseException"></exception>
        Task<SurveyMetrics> GetTestMetricsAsync(string surveyId);
    }
}

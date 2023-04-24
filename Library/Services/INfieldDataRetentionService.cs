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
    /// Service for managing the data retention settings of a survey
    /// </summary>
    public interface INfieldDataRetentionService
    {
        /// <summary>
        /// Get the data retention settings for a survey
        /// </summary>
        /// <param name="surveyId">The survyey ID</param>
        /// <returns><see cref="DataRetentionSettings"/></returns>
        Task<DataRetentionSettings> GetAsync(string surveyId);

        /// <summary>
        /// Update the data retention settings for a survey
        /// </summary>
        /// <param name="surveyId">The survyey ID</param>
        /// <param name="retentionPeriod">The survey data retention period in days</param>
        Task PutAsync(string surveyId, int retentionPeriod);
    }
}

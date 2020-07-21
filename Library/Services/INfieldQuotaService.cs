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
using Nfield.Models;
using Nfield.SDK.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read survey quota.
    /// </summary>
    public interface INfieldQuotaService
    {
        /// <summary>
        /// Gets the quota definition for an online survey
        /// </summary>
        /// <param name="surveyId">The survey id</param>
        /// <returns></returns>
        Task<IEnumerable<QuotaFrameVersion>> GetQuotaFrameVersionsAsync(string surveyId);

        /// <summary>
        /// Updates the survey quota targets for the specified quota frame version
        /// </summary>
        /// <param name="surveyId">The survey to set the quota targets for</param>
        /// <param name="quotaETag">The quota frame version to set the targets for</param>
        /// <param name="targets">The new quota frame targets</param>
        Task UpdateQuotaTargetsAsync(string surveyId, string quotaETag, IEnumerable<QuotaFrameLevelTarget> targets);

        /// Gets the specified version of the quota frame  
        /// </summary>
        /// <param name="surveyId">The survey id</param>
        /// <param name="Etag">The version of the quota frame to retrieve</param>
        /// <returns></returns>
        Task<QuotaFrame> GetQuotaFrameAsync(string surveyId, string Etag);
    }
}

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
    /// The survey sampling method service.
    /// Allows to get and update the sampling method of a CAPI survey.
    /// </summary>
    public interface INfieldSurveySamplingMethodService
    {
        /// <summary>
        /// Returns the CAPI survey's sampling method
        /// </summary>
        /// <param name="surveyId">The id of the survey to get the sampling method of</param>
        Task<SamplingMethodType> GetAsync(string surveyId);

        /// <summary>
        /// Updates the CAPI survey's sampling method
        /// </summary>
        /// <param name="surveyId">The id of the survey to update the sampling method of</param>
        /// <param name="samplingMethod">The sampling method to set the survey to</param>
        Task UpdateAsync(string surveyId, SamplingMethodType samplingMethod);
    }
}

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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read and update survey data.
    /// </summary>
    public interface INfieldSurveysService
    {
        /// <summary>
        /// Gets survey queryable object.
        /// </summary>
        Task<IQueryable<Survey>> QueryAsync();

        /// <summary>
        /// Gets sampling point queryable object.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        Task<IQueryable<SamplingPoint>> SamplingPointsQueryAsync(string surveyId);

        /// <summary>
        /// Gets a specific sampling point.
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="samplingPointId"></param>
        /// <returns></returns>
        Task<SamplingPoint> SamplingPointQueryAsync(string surveyId, string samplingPointId);

        /// <summary>
        /// Updates a sampling point
        /// </summary>
        /// <param name="samplingPoint"></param>
        /// <returns></returns>
        Task<SamplingPoint> SamplingPointUpdateAsync(SamplingPoint samplingPoint);

        /// <summary>
        /// Adds a sampling point
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="samplingPoint"></param>
        /// <returns></returns>
        Task<SamplingPoint> SamplingPointAddAsync(string surveyId, SamplingPoint samplingPoint);

        /// <summary>
        /// Deletes a sampling point.
        /// </summary>
        /// <param name="samplingPoint"></param>
        /// <returns></returns>
        Task SamplingPointDeleteAsync(SamplingPoint samplingPoint);


    }
}

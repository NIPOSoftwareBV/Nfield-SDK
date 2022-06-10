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
using System.Linq;
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to manage sampling points.
    /// </summary>
    public interface INfieldSamplingPointsService
    {
        /// <summary>
        /// Gets all sampling points for a survey
        /// </summary>
        /// <param name="surveyId">The survey for which to return sampling points</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<IQueryable<SamplingPoint>> QueryAsync(string surveyId);

        /// <summary>
        /// Get a sampling point for a survey
        /// </summary>
        /// <param name="surveyId">The survey for which to return sampling points</param>
        /// <param name="samplingPointId">sampling point id</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<SamplingPoint> GetAsync(string surveyId, string samplingPointId);

        /// <summary>
        /// Delete a sampling point for a survey
        /// </summary>
        /// <param name="surveyId">The survey for which to delete sampling point</param>
        /// <param name="samplingPointId">sampling point id</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task RemoveAsync(string surveyId, string samplingPointId);


        /// <summary>
        /// Update a sampling point 
        /// </summary>
        /// <param name="surveyId">The survey for which to update sampling point</param>
        /// <param name="samplingPoint">sampling point</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<SamplingPoint> UpdateAsync(string surveyId, SamplingPoint samplingPoint);

        /// <summary>
        /// Create a sampling point 
        /// </summary>
        /// <param name="surveyId">The survey for which to create sampling point</param>
        /// <param name="samplingPoint">sampling point data</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<SamplingPoint> CreateAsync(string surveyId, SamplingPoint samplingPoint);

        /// <summary>
        /// Activate a sampling point 
        /// </summary>
        /// <param name="surveyId">The survey for which to activate sampling point</param>
        /// <param name="samplingPointId">sampling point id</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<SamplingPoint> ActivateAsync(string surveyId, string samplingPointId, int? target);

        /// <summary>
        /// Activate sampling points 
        /// </summary>
        /// <param name="surveyId">The survey for which to activate sampling point</param>
        /// <param name="samplingPointIds">sampling point ids</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task ActivateAsync(string surveyId, IEnumerable<string> samplingPointIds);

        /// <summary>
        /// Replace a sampling point 
        /// </summary>
        /// <param name="surveyId">The survey for which to replace sampling point</param>
        /// <param name="samplingPointId">sampling point id</param>
        /// <param name="samplingPoint">sampling point data</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<SamplingPoint> ReplaceAsync(string surveyId, string samplingPointId, string newSamplingPoint, int? target);


    }
}
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
using System.Linq;
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to create, retrieve, update and delete <see cref="SurveyResponseCode"/>.
    /// </summary>
    public interface INfieldSurveyResponseCodesService
    {

        /// <summary>
        /// Gets all the survey response codes for a survey with the supplied <paramref name="surveyId"/>.
        /// </summary>
        /// <param name="surveyId">The survey id for which to return survey specific response codes</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        /// <returns>A list with all surveys response codes</returns>
        Task<IQueryable<SurveyResponseCode>> QueryAsync(string surveyId);

        /// <summary>
        /// Gets the survey response code for a survey with the supplied <paramref name="surveyId"/> 
        /// and has the specified <paramref name="code"/>.
        /// </summary>
        /// <param name="surveyId">The survey id for which to return survey specific response code</param>
        /// <param name="code">The value of the survey response code</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        /// <returns>A <see cref="SurveyResponseCode"/> or null if the survey does not exists 
        /// or the survey does not  response code with this code</returns>
        Task<SurveyResponseCode> QueryAsync(string surveyId, int code);

        /// <summary>
        /// Adds the supplied <paramref name="responseCode"/> to the system
        /// </summary>
        /// <param name="surveyId">The id of the survey to add the response code to</param>
        /// <param name="responseCode">The <see cref="SurveyResponseCode"/> to add</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<SurveyResponseCode> AddAsync(string surveyId, SurveyResponseCode responseCode);

        /// <summary>
        /// Updates the supplied supplied <paramref name="responseCode"/>
        /// </summary>
        /// <param name="surveyId">The id of the survey the response code belongs to</param>
        /// <param name="responseCode">The <see cref="SurveyResponseCode"/> to update</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task<SurveyResponseCode> UpdateAsync(string surveyId, SurveyResponseCode responseCode);

        /// <summary>
        /// Removes the survey response code with the supplied <paramref name="code"/> 
        /// from the survey with the supplied <paramref name="surveyId"/>
        /// </summary>
        /// <param name="surveyId">The survey to remove response code from</param>
        /// <param name="code">The code of the response code to remove</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task RemoveAsync(string surveyId, int code);

    }
}
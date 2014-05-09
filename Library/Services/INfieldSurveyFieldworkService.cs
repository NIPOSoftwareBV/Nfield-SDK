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
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// The fieldwork management service. Allows to check the fieldwork status, start and Start the fieldwork.
    /// </summary>
    public interface INfieldSurveyFieldworkService
    {
        /// <summary>
        /// This method returns fieldwork status
        /// </summary>
        /// <param name="surveyId">The id of the survey to get the status</param>
        Task<SurveyStatus> GetStatusAsync(string surveyId);

        /// <summary>
        /// This method starts the fieldwork of the survey.
        /// </summary>
        /// <param name="surveyId">The id of the survey to start</param>        
        Task StartFieldworkAsync(string surveyId);

        /// <summary>
        /// This method Starts the fieldwork of the survey.
        /// </summary>
        /// <param name="surveyId">The id of the survey to Start</param>
        /// <param name="model">The settings for the Startping of the fieldwork</param>
        Task StopFieldworkAsync(string surveyId, StopFieldworkModel model);
    }
}

﻿//    This file is part of Nfield.SDK.
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
using Nfield.SDK.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Service for managing survey interview simulations
    /// </summary>
    public interface INfieldSurveyInterviewSimulationService
    {
        /// <summary>
        /// Get hints for survey simulation
        /// </summary>
        /// <param name="surveyId">Id of the simulation survey</param>
        /// <returns>Content of the hints file</returns>
        Task<string> GetHintsAsync(string surveyId);

        /// <summary>
        /// Retrieve all simulations.
        /// </summary>
        /// <returns>List of all simulations</returns>
        Task<IQueryable<SurveyInterviewSimulation>> GetInterviewSimulationsAsync();

        /// <summary>
        /// Retrieve the simulation of a survey.
        /// </summary>
        /// <param name="surveyId">The survey id</param>
        /// <returns>The simulation object</returns>
        Task<SurveyInterviewSimulation> GetSurveyInterviewSimulationAsync(string surveyId);

        /// <summary>
        /// Starts interview simulation.
        /// </summary>
        /// <param name="surveyId">The survey id</param>
        /// <param name="simulationRequest">Parameters to start the simulations</param>
        Task<InterviewSimulationResult> StartSimulationAsync(string surveyId, InterviewSimulation simulationRequest);

        /// <summary>
        /// Starts interview simulation.
        /// This one allows to upload  hints and sample data directly from files.  
        /// </summary>
        /// <param name="surveyId">The survey id</param>
        /// <param name="simulationRequest">Parameters to start the simulations</param>
        Task<InterviewSimulationResult> StartSimulationAsync(string surveyId, InterviewSimulationFiles simulationRequest);
    }
}

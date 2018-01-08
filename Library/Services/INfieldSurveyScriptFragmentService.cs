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
    /// Represents a set of methods to read and update survey script fragments.
    /// </summary>
    public interface INfieldSurveyScriptFragmentService
    {
        #region CRUD on survey script fragments

        /// <summary>
        /// Gets all script fragment names for a survey.
        /// </summary>
        Task<IQueryable<string>> QueryAsync(string surveyId);

        /// <summary>
        /// Gets a survey script fragment object.
        /// </summary>
        Task<string> GetAsync(string surveyId, string fileName);

        /// <summary>
        /// Adds or updates the script fragment for survey.
        /// </summary>
        Task AddOrUpdateAsync(string surveyId, string fileName, string script);

        /// <summary>
        /// Removes the survey script fragment.
        /// </summary>
        Task RemoveAsync(string surveyId, string fileName);

        #endregion
    }
}

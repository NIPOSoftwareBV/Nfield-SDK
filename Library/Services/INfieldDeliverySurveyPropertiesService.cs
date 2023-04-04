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

using System.Linq;
using System.Threading.Tasks;
using Nfield.SDK.Models.Delivery;

namespace Nfield.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface INfieldDeliverySurveyPropertiesService
    {
        /// <summary>
        /// Returns the properties for the selected survey.
        /// </summary>
        /// <param name="surveyId">The Nfield Survey id.</param>
        /// <returns>The properties for the selected survey.</returns>         
        Task<IQueryable<DomainSurveyPropertyModel>> QueryAsync(string surveyId);

        /// <summary>
        /// Returns the property for the selected survey based on the given identifier.
        /// </summary>
        /// <param name="surveyId">The Nfield Survey id.</param>
        /// <param name="propertyId">The property id.</param>
        /// <returns>The property of the survey for the given identifier.</returns>         
        Task<DomainSurveyPropertyModel> GetByIdAsync(string surveyId, long propertyId);

        /// <summary>
        /// Adds the selected properties to the survey.
        /// </summary>
        /// <param name="surveyId">The Nfield Survey id.</param>
        /// <returns><c>NoContentResult</c>, if succeeded. The appropriate exception in case of failure.</returns>
        Task PostAsync(string surveyId, CreateDomainSurveyPropertyModel model);

        /// <summary>
        /// Updates the selected survey property's values.
        /// </summary>
        /// <param name="surveyId">The Nfield Survey Id.</param>
        /// <param name="propertyId">The property id.</param>
        /// <returns><c>NoContentResult</c>, if succeeded. The appropriate exception in case of failure.</returns>
        Task PutSurveyPropertyAsync(string surveyId, long propertyId, UpdateDomainSurveyPropertyModel model);

        /// <summary>
        /// Deletes the selected property from the survey.
        /// </summary>
        /// <param name="surveyId">The Nfield Survey Id.</param>
        /// <param name="propertyId">The property id.</param>
        /// <returns><c>NoContentResult</c>, if succeeded. The appropriate exception in case of failure.</returns>
        Task DeleteAsync(string surveyId, long propertyId);
    }
}

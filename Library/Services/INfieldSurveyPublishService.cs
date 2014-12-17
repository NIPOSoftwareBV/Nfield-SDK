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

using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a couple of methods to read state and publish survey to survey package.
    /// </summary>
    public interface INfieldSurveyPublishService
    {
        /// <summary>
        /// Gets the publish state of the survey in the context.   
        /// </summary>
        Task<SurveyPackageStateModel> GetAsync(string surveyId);

        /// <summary>
        /// Publish the latest survey artifacts to the survey package,
        /// saves the survey package in the blob
        /// upadtes the database
        /// </summary>
        /// <param name="surveyId">The id of the survey.</param>
        /// <param name="surveyPublishTypeUpgrade">A model that handels the interview package type
        /// and force interviews on older packages to upgrade to this package</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldErrorException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldHttpResponseException"></exception>
        Task PutAsync(string surveyId, SurveyPublishTypeUpgradeModel surveyPublishTypeUpgrade);
    }
}

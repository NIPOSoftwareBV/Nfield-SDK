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

namespace Nfield.Services
{
    /// <summary>
    /// Interface for a service that gets the invitation templates for a survey
    /// </summary>
    public interface INfieldSurveyInvitationTemplatesService
    {
        /// <summary>
        /// Add an invitation template for the specific survey
        /// </summary>
        Task<InvitationTemplateModel> AddAsync(string surveyId, InvitationTemplateModel invitationTemplate);

        /// <summary>
        /// Get the invitation templates for the specified survey
        /// </summary>
        Task<IEnumerable<InvitationTemplateModel>> GetAsync(string surveyId);

        /// <summary>
        /// Update an invitation template for the specific survey
        /// </summary>
        Task<InvitationTemplateModel> UpdateAsync(string surveyId, InvitationTemplateModel invitationTemplate);

        /// <summary>
        /// Delete an invitation template for the specific survey
        /// </summary>
        Task<bool> RemoveAsync(string surveyId, string templateId);
    }
}

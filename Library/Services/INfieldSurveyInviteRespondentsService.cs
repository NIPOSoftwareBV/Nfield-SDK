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
using Nfield.Services.Implementation;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to manage invitations
    /// </summary>
    public interface INfieldSurveyInviteRespondentsService
    {
        /// <summary>
        /// Send a batch of invitations to respondents
        /// </summary>
        /// <param name="surveyId">Id of the survey</param>
        /// <param name="batch">Properties of the batch</param>
        /// <returns>Status of the batch send</returns>
        Task<InviteRespondentsStatus> SendInvitationsAsync(string surveyId, InvitationBatch batch);

        /// <summary>
        /// Get the invitation status for the specified batch belonging to the survey
        /// </summary>
        Task<IEnumerable<InvitationStatusDto>> GetInvitationStatusAsync(string surveyId, string batchName);
    }
}

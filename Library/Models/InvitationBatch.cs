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

namespace Nfield.Models
{
    /// <summary>
    /// Class representing a batch of invitations
    /// </summary>
    public class InvitationBatch
    {
        /// <summary>
        /// The name of the batch
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The name of the column that contains the email address
        /// </summary>
        public string EmailColumnName { get; set; }

        /// <summary>
        /// The id of the sample record
        /// </summary>
        public IEnumerable<string> RespondentKeys { get; set; }

        /// <summary>
        /// The scheduled date for the email being send
        /// </summary>
        public DateTime? ScheduledFor { get; set; }

        /// <summary>
        /// The id of the invitation template
        /// </summary>
        public int InvitationTemplateId { get; set; }

        /// <summary>
        /// Respondent filters
        /// </summary>
        internal IEnumerable<SampleFilter> Filters { get; set; }

    }
}

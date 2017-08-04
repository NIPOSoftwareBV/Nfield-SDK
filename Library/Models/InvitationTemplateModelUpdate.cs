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

namespace Nfield.Models
{
    /// <summary>
    /// Model for creating and updating an email invitation template
    /// </summary>
    public class InvitationTemplateModelUpdate
    {
        /// <summary>
        /// Invitation type.
        /// 1 = Invitation.
        /// 2 = Reminder.
        /// </summary>
        public int InvitationType { get; set; }

        /// <summary>
        /// Name of the invitation template
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Subject of the invitation
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Body of the invitation
        /// </summary>
        public string Body { get; set; }
    }
}

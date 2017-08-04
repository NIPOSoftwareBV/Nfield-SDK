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
    /// Extension of <see cref="InvitationTemplateModel"/> that says whether the template is valid
    /// </summary>
    public class InvitationTemplateModelValidated : InvitationTemplateModel
    {
        /// <summary>
        /// Whether the template is valid
        /// </summary>
        public bool IsValid { get; internal set; }

        /// <summary>
        /// The validation error in case the template is invalid
        /// </summary>
        public string ValidationError { get; internal set; }
    }
}

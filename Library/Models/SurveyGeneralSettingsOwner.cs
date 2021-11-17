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
    /// Model to obtain the survey owner.
    /// In the Nfield Manager UI is part of the General Settings.
    /// </summary>
    public class SurveyGeneralSettingsOwner
    {
        /// <summary>
        /// The user object of the owner of the survey 
        /// </summary>
        public User Owner { get; set; }
    }

    /// <summary>
    /// Model for an Nfield User
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique Id of user
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the user
        /// </summary>
        public string UserName { get; set; }
    }
}

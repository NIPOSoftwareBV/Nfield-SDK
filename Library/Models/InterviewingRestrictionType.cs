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

namespace Nfield.SDK.Models
{
    /// <summary>
    /// Specifies what interviews are allowed to continue when the fieldwork is stopped.
    /// </summary>
    public enum InterviewingRestrictionType
    {
        /// <summary>
        /// Not set
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Block all interviews
        /// </summary>
        BlockEverything = 1,

        /// <summary>
        /// Allow only active interviews
        /// </summary>
        AllowOnlyActives = 2,

        /// <summary>
        /// Allow only active and resume interviews
        /// </summary>
        AllowActivesAndResumes = 3,

        /// <summary>
        /// Allow all interviews
        /// </summary>
        AllowEverything = 4,
    }
}

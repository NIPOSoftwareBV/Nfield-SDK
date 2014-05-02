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

namespace Nfield.Models
{
    /// <summary>
    /// The fieldwork status of the survey
    /// </summary>
    public enum SurveyStatus
    {
        /// <summary>
        /// Fieldwork under construction, i.e. fieldwork has not been started yet.
        /// </summary>
        UnderConstruction = 0,

        /// <summary>
        /// Fieldwork started
        /// </summary>
        Started = 1,

        /// <summary>
        /// Fieldwork closed
        /// </summary>
        Closed = 3,

        /// <summary>
        /// Fieldwork finished, i.e. fieldwork cannot be started anymore.
        /// </summary>
        Finished = 4,
    }
}

﻿//    This file is part of Nfield.SDK.
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
    /// Holds the target to complete for a work package quota level ID
    /// </summary>
    public class WorkPackageTargetCounts : WorkPackageTarget
    {
        /// <summary>
        /// Successful
        /// </summary>
        public int Successful { get; set; }

        /// <summary>
        /// Survey Successful
        /// </summary>
        public int SurveySuccessful { get; set; }
    }
}
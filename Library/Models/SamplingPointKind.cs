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
    /// Indicates the kind of the sampling point
    /// </summary>
    public enum SamplingPointKind
    {
        /// <summary>
        /// A regular sampling point that can be used for assignment
        /// </summary>
        Regular = 0,

        /// <summary>
        /// A spare sampling point that can be selected as spare to another sampling point
        /// </summary>
        Spare = 1,

        /// <summary>
        /// A spare sampling point that selected as a spare for another sampling point (either add or replace).
        /// Indicates that the sampling point can be selected for assignment
        /// </summary>
        SpareActive = 2,

        /// <summary>
        /// Indicates that the sampling point was replaced by another one. It is a final state
        /// and the sampling point can not be used for assignment any more.
        /// </summary>
        Replaced = 3

    }
}
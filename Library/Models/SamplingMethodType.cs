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
    /// The sampling method of a CAPI survey
    /// </summary>
    public enum SamplingMethodType
    {
        /// <summary>
        /// Unknown (not set yet)
        /// </summary>
        Unknown,

        /// <summary>
        /// No quota target
        /// </summary>
        FreeIntercept,

        /// <summary>
        /// Joint target(s) for quota
        /// </summary>
        JointTargets,

        /// <summary>
        /// Individual target(s) for quota
        /// </summary>
        IndividualTargets,

        /// <summary>
        /// Survey type Advanced
        /// </summary>
        SamplingPoints,

        /// <summary>
        /// Survey type EuroBarometer
        /// </summary>
        Addresses,

        /// <summary>
        /// Survey type EuroBarometerAdvanced
        /// </summary>
        AddressesWithQuota
    }
}

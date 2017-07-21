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
    /// Status of the invite respondents request
    /// </summary>
    public class InviteRespondentsStatus
    {
        /// <summary>
        /// Number of respondents invited
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Description of the status of the invite operation
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Message detail when <see cref="Status"/> indicates an error
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
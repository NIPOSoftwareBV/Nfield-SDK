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

namespace Nfield.Infrastructure
{
    namespace NipoSoftware.Nfield.Manager.Api.Helpers
    {
        /// <summary>
        /// Nfield error codes
        /// </summary>
        public enum NfieldErrorCode
        {
            /// <summary>
            /// (One of the) provided arguments is null
            /// </summary>
            ArgumentIsNull,

            /// <summary>
            /// At least one of the password rules is violated
            /// </summary>
            PasswordRulesNotMet,

            /// <summary>
            /// The requested changes to the model are not allowed
            /// </summary>
            UnacceptableChangesInSuppliedData,

            /// <summary>
            /// A validation rule has been violated
            /// </summary>
            DataValidationError,

            /// <summary>
            /// Database cannot be updated
            /// </summary>
            DataUpdateError
        }
    }
}

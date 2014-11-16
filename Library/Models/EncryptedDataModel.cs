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
    /// Model that holds the encrypted data and the initialization vector that was used to encrypt it
    /// </summary>
    public class EncryptedDataModel
    {
        /// <summary>
        /// Encrypted data in Base64 string format
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Initialization Vector that is used to encrypt this data
        /// In Base64 string format
        /// </summary>
        public string InitializationVector { get; set; }
    }
}

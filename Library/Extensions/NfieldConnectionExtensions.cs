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

using Nfield.Infrastructure;

namespace Nfield.Extensions
{
    /// <summary>
    /// Extensions to <see cref="NfieldConnection"/> class to make the asynchronous methods synchronous
    /// </summary>
    public static class NfieldConnectionExtensions
    {
        /// <summary>
        /// A synchronous version of <see cref="INfieldConnection.SignInAsync"/>.
        /// </summary>
        public static bool SignIn(this INfieldConnection nfieldConnection, string domainName, string username, string password)
        {
            return nfieldConnection.SignInAsync(domainName, username, password).Result;
        }

    }
}

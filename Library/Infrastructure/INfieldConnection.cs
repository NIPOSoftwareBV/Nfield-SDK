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
using System.Threading.Tasks;

namespace Nfield.Infrastructure
{

    /// <summary>
    /// Represents a connection to an Nfield server. Use the <see cref="INfieldConnection"/> to gain access to the 
    /// various services that Nfield provides.
    /// </summary>
    public interface INfieldConnection : IServiceProvider, IDisposable
    {
        /// <summary>
        /// The server Uri for Nfield.
        /// </summary>
        Uri NfieldServerUri { get; }

        /// <summary>
        /// Sign into the specified domain, using the specified username and password
        /// </summary>
        /// <param name="domainName">The name of the domain to sign in to</param>
        /// <param name="username">The username to sign in</param>
        /// <param name="password">The password to use for authentication</param>
        /// <returns><c>true</c> if sign in was successful, <c>false</c> otherwise.</returns>
        Task<bool> SignInAsync(string domainName, string username, string password);

        /// <summary>
        /// Return the specified service <typeparamref name="TService"/> provided by Nfield.
        /// </summary>
        /// <returns>An implementation of the specified service.</returns>
        TService GetService<TService>();

    }

}
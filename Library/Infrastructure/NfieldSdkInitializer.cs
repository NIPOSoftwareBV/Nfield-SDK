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
using Nfield.Services;
using Nfield.Services.Implementation;

namespace Nfield.Infrastructure
{
    /// <summary>
    /// Used to register the types into the user-defined IoC container.
    /// </summary>
    public static class NfieldSdkInitializer
    {
        /// <summary>
        /// Method that registers all known types by calling the delegates provided.
        /// This method must be called before using the SDK.
        /// </summary>
        /// <param name="registerTransient">Method that registers a Transient type.</param>
        /// <param name="registerSingleton">Method that registers a Singleton.</param>
        /// <param name="registerInstance">Method that registers an instance.</param>
        public static void Initialize(Action<Type, Type> registerTransient, 
                                      Action<Type, Type> registerSingleton,
                                      Action<Type, Object> registerInstance)
        {
            registerTransient(typeof(NfieldConnection), typeof(NfieldConnection));
            registerTransient(typeof(INfieldInterviewersService), typeof(NfieldInterviewersService));
            registerTransient(typeof(INfieldHttpClient), typeof(NfieldHttpClient));
        }

    }
}
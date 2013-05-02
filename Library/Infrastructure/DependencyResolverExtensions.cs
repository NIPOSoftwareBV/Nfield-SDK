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
using System.Collections.Generic;
using System.Linq;

namespace Nfield.Infrastructure
{
    /// <summary>
    /// Extends <see cref="IDependencyResolver"/> with generic implementations of the <see cref="IDependencyResolver.Resolve"/>
    /// and <see cref="IDependencyResolver.ResolveAll"/> methods.
    /// </summary>
    public static class DependencyResolverExtensions
    {

        /// <summary>
        /// Resolve classes of type T
        /// </summary>
        /// <typeparam name="T">type to resolve</typeparam>
        /// <param name="dependencyResolver">resolver</param>
        /// <returns>instance of type T</returns>
        public static T Resolve<T>(this IDependencyResolver dependencyResolver)
        {
            return (T) dependencyResolver.Resolve(typeof (T));
        }

        /// <summary>
        /// Resolve classes of type T
        /// </summary>
        /// <typeparam name="T">type to resolve</typeparam>
        /// <param name="dependencyResolver">resolver</param>
        /// <returns>Enumeration of all instances of type T</returns>
        public static IEnumerable<T> ResolveAll<T>(this IDependencyResolver dependencyResolver)
        {
            return dependencyResolver.ResolveAll(typeof (T))
                                     .Cast<T>();
        }

    }
}

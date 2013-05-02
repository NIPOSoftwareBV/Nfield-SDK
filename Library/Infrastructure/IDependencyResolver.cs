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

namespace Nfield.Infrastructure
{
    /// <summary>
    /// A simple interface that can be implemented to expose a Inversion of Control container to the SDK. Internally the
    /// SDK uses this interface, exposed through <see cref="Nfield.Infrastructure.DependencyResolver"/> to resolve
    /// all dependencies.
    /// </summary>
    public interface IDependencyResolver
    {

        /// <summary>
        /// Resolves a single registered type.
        /// </summary>
        /// <param name="typeToResolve"></param>
        /// <returns></returns>
        object Resolve(Type typeToResolve);

        /// <summary>
        /// Resolves multiple registered instances.
        /// </summary>
        /// <param name="typeToResolve">The type to resolve</param>
        /// <returns></returns>
        IEnumerable<object> ResolveAll(Type typeToResolve);

    }
}

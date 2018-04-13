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

namespace Nfield.Utilities
{
    /// <summary>
    /// Class that contains the basic method parameter checks
    /// </summary>
    public class Ensure
    {
        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if the specified value is <see langword="null"/>
        /// or an empty string.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="name">The name of the parameter, which will appear in the exception message.</param>
        public static void ArgumentNotNullOrEmptyString(string value, string name)
        {
            ArgumentNotNull(value, name);
            if (value.Trim().Length == 0)
                throw new ArgumentException($"{name} cannot be empty");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified value is <see langword="null"/>.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="name">The name of the parameter, which will appear in the exception message.</param>
        public static void ArgumentNotNull(object value, string name)
        {
            if (value == null)
                throw new ArgumentNullException(name);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentEnumerableNotNullOrEmpty"/> if the specified value is <see langword="null"/> or empty (no items in the collection).
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <param name="name">The name of the parameter, which will appear in the exception message.</param>
        public static void ArgumentEnumerableNotNullOrEmpty(IEnumerable<object> value, string name)
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentNullException(name);
            }

            if (!value.GetEnumerator().MoveNext()) //.Any() is not used to avoid a reference to System.Linq
                throw new ArgumentNullException(name);
        }
    }
}

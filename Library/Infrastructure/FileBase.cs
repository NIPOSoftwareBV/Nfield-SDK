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

namespace Nfield.Infrastructure
{
    /// <summary>
    /// provides base class for <see cref="FileWrapper"/> 
    /// </summary>
    public abstract class FileBase
    {
        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="path">The file to check.</param>
        /// <returns>        
        /// true if the caller has the required permissions and path contains the name
        /// of an existing file; otherwise, false. This method also returns false if
        /// path is null, an invalid path, or a zero-length string. If the caller does
        /// not have sufficient permissions to read the specified file, no exception
        /// is thrown and the method returns false regardless of the existence of path.</returns>
        public abstract bool Exists(string path);

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <returns>A string containing all lines of the file.</returns>
        public abstract string ReadAllText(string path);
    }
}

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
using System.IO;

namespace Nfield.Infrastructure
{
    /// <summary>
    /// Provides properties and methods for file 
    /// </summary>
    public class FileWrapper : FileBase
    {
        /// <summary>
        /// <see cref="FileBase.Exists"/>
        /// </summary>
        public override bool Exists(string path)
        {
            return File.Exists(path);
        }
        /// <summary>
        /// <see cref="FileBase.ReadAllText"/>
        /// </summary>
        public override string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }
}

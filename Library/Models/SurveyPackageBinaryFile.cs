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

namespace Nfield.Models
{
    /// <summary>
    /// this class contains the basic information about 
    /// the binary files which is included in the survey 
    /// package. The CAPI client uses this information to 
    /// download the binary files such as the Interviewer 
    /// Instructions or Media files.
    /// </summary>
    public class SurveyPackageBinaryFile
    {
        /// <summary>
        /// The name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The size of the file.  
        /// This is used by the CAPI client to 
        /// display download progress.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// The MD5 of the file.  
        /// This is used by the CAPI client to 
        /// determine if the file has changed.
        /// </summary>
        public string Md5 { get; set; }
    }
}

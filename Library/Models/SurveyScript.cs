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

namespace Nfield.Models
{
    /// <summary>
    /// Model for the ODIN script (fragment) for a survey
    /// </summary>
    public class SurveyScript
    {
        /// <summary>
        /// The ODIN script or script fragment
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        /// The file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Warning messages that resulted from the parse
        /// </summary>
        public IEnumerable<string> WarningMessages { get; set; }

        /// <summary>
        /// Indicates if script with unfixed positions is allowed
        /// </summary>
        public bool UnfixedIsOk { get; set; }

        /// <summary>
        /// Indication to use the legacy parser. The legacy parser is more lenient regarding commands
        /// that are not supported in Nfield.
        ///
        /// Please note that the legacy parser will be deprecated in the future and it
        /// is prudent to update scripts to support the latest parser.
        /// </summary>
        public bool UseLegacyParser { get; set; }
    }
}

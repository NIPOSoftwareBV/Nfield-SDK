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

using System.Collections.Generic;

namespace Nfield.SDK.Models
{
    public class ManualTestSurveyResult
    {
        /// <summary>
        /// Survey id of the original survey
        /// </summary>
        public string OriginalSurveyId { get; set; }

        /// <summary>
        /// Survey id of the manual test survey
        /// </summary>
        public string SimulationSurveyId { get; set; }

        /// <summary>
        /// Error messages (if any)
        /// </summary>
        public IEnumerable<string> ErrorMessages { get; set; } = new List<string>();
    }
}

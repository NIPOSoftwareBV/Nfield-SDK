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

namespace Nfield.SDK.Models
{
    /// <summary>
    /// Parameters to start creating a manual test survey.
    /// To specify uploaded files this model uses the file name and the file content.
    /// For the sample data we expect binary as an input since it could be csv or Excel data. 
    /// To upload data directly from files use <see cref="StartCreateManualTestSurveyFile"/>
    /// </summary>
    public class StartCreateManualTestSurvey
    {
        /// <summary>
        /// Enable reporting
        /// </summary>
        public bool EnableReporting { get; set; }

        /// <summary>
        /// Use sample data from the original survey
        /// </summary>
        public bool UseOriginalSample { get; set; }

        /// <summary>
        /// Sample data file name
        /// </summary>
        public string SampleDataFileName { get; set; }

        /// <summary>
        /// Sample data
        /// </summary>
        public byte[] SampleDataFile { get; set; }
    }
}

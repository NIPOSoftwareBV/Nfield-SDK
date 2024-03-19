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

namespace Nfield.Models
{
    /// <summary>
    /// Parameters to start interview simulation.
    /// To specify uploaded files this model uses the file name and the file content as a text string.
    /// To upload data directly from files use <see cref="InterviewSimulationFiles"/>
    /// </summary>
    public class InterviewSimulation
    {
        /// <summary>
        /// Number of interviews to be simulated
        /// </summary>
        public int InterviewsCount { get; set; }

        /// <summary>
        /// Enable reporting for the simulation
        /// </summary>
        public bool EnableReporting { get; set; }

        /// <summary>
        /// Use sample data from the original survey
        /// </summary>
        public bool UseOriginalSample { get; set; }

        /// <summary>
        /// Hints file name
        /// </summary>
        public string HintsFileName { get; set; }

        /// <summary>
        /// Hints data
        /// </summary>
        public string Hints { get; set; }

        /// <summary>
        /// Sample data file name
        /// </summary>
        public string SampleDataFileName { get; set;}

        /// <summary>
        /// Sample data
        /// </summary>
        public string SampleData { get; set; }
    }
}

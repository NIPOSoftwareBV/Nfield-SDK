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
    /// Base model for the interview settings (for a survey)
    /// It defines which actions are available during the interviews
    /// These settings are only used in the Online channel
    /// </summary>
    public class SurveyInterviewSettings
    {
        /// <summary>
        /// Allow to navigate backwards
        /// </summary>
        public bool? BackButtonAvailable { get; set; }

        /// <summary>
        /// Allow identified users to pause the interview
        /// </summary>
        public bool? PauseButtonAvailable { get; set; }

        /// <summary>
        /// Allow to clear all answers from the screen
        /// </summary>
        public bool? ClearButtonAvailable { get; set; }
    }
}

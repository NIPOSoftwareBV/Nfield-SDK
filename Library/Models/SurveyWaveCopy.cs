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
    /// Survey wave copy properties. Inherits from Survey to extend it with the SourceWaveId.
    /// This model is used to create a new wave survey by copying artifacts from an existing wave.
    /// </summary>
    public class SurveyWaveCopy : Survey
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="surveyType">Type of the survey</param>
        public SurveyWaveCopy(SurveyType surveyType) : base(surveyType) { }

        /// <summary>
        /// The id of the wave to copy artifacts from.
        /// </summary>
        public string SourceWaveId { get; set; }
    }
}

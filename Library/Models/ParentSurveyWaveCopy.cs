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
    public class ParentSurveyWaveCopy
    {

        /// <summary>
        /// The name of the new survey
        /// </summary>
        public string SurveyName { get; set; }
    }
}

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
    /// Holds the properties of a survey general setting
    /// </summary>
    public class SurveyGeneralSetting
    {
        /// <summary>
        /// The  Id of survey general settings        
        /// </summary>
        public string SurveyId { get; set; }

        /// <summary>
        /// The Description of survey general settings
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Client of survey general settings
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// The Name of survey general settings
        /// </summary>
        public string Name { get; set; }

    }
}

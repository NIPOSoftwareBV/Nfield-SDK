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
    /// Class to hold the current package state of a survey
    /// </summary>
    public class SurveyPackageStateModel
    {
        /// <summary>
        /// The state of the package for live interviewing
        /// </summary>
        public PackagePublishState Live { get; set; }

        /// <summary>
        /// The state of the package for test interviewing
        /// </summary>
        public PackagePublishState Test { get; set; }
    }
}

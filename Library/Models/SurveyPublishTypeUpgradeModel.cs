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
    /// A model for Survey Publish Type and force Upgrade
    /// </summary>
    public class SurveyPublishTypeUpgradeModel
    {
        /// <summary>
        /// The Interview Package Type
        /// </summary>
        public InterviewPackageType PackageType { get; set; }
        /// <summary>
        /// The state of published new package if upgrade for current interviews needed
        /// </summary>
        public PackageForceUpgrade ForceUpgrade { get; set; }
        /// <summary>
        /// Indicates if script with unfixed positions is allowed
        /// </summary>
        public bool UnfixedIsOk { get; set; }
    }
}

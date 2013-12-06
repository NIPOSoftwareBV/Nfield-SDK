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

using System.Linq;
using Nfield.Models;
using Nfield.Services;
using Nfield.Services.Implementation;

namespace Nfield.Extensions
{
    /// <summary>
    /// Extensions to make the asynchronous methods of <see cref="NfieldSurveySettingsService"/> synchronous
    /// </summary>
    public static class NfieldSurveySettingsServiceExtensions
    {
        /// <summary>
        /// A synchronous version of <see cref="INfieldSurveySettingsService.AddOrUpdateAsync"/>
        /// </summary>
        public static SurveySetting AddOrUpdate(this INfieldSurveySettingsService settingsService, string surveyId, SurveySetting setting)
        {
            return settingsService.AddOrUpdateAsync(surveyId, setting).Result;
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldSurveySettingsService.AddOrUpdateAsync"/>
        /// </summary>
        public static IQueryable<SurveySetting> Query(this INfieldSurveySettingsService settingsService, string surveyId)
        {
            return settingsService.QueryAsync(surveyId).Result;
        }
    }
}

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

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nfield.Models
{
    /// <summary>
    /// Survey types
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))] // Serialize as string value, not underlying int value
    public enum SurveyType
    {
        /// <summary>
        /// Basic survey type, this survey type has no features
        /// </summary>
        Basic,

        /// <summary>
        /// Survey that sampling points assigned
        /// </summary>
        Advanced,

        /// <summary>
        /// Survey that has sampling points with targets
        /// </summary>
        EuroBarometer,

        /// <summary>
        /// Basic online survey type, this survey type has no features
        /// </summary>
        OnlineBasic,
    }
}
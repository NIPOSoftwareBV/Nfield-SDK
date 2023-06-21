﻿//    This file is part of Nfield.SDK.
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

using Newtonsoft.Json;
using System;

namespace Nfield.Models
{
    /// <summary>
    /// Model for the survey version
    /// </summary>
    public class SurveyVersion
    {
        /// <summary>
        /// The Etag
        /// </summary>
        [JsonProperty(PropertyName = "eTag")]
        public string Etag { get; set; }

        /// <summary>
        /// The date when the survey package was published
        /// </summary>
        ///<remarks>Time format is in UTC</remarks>
        public DateTime PublishDateUtc { get; set; }
    }
}
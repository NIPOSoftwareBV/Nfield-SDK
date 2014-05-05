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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nfield.Models;
using Nfield.Services;
using Nfield.Services.Implementation;

namespace Nfield.Extensions
{
    /// <summary>
    /// Extensions to make the asynchronous methods of <see cref="NfieldSurveysService"/> synchronous
    /// </summary>
    public static class NfieldSurveysServiceExtensions
    {
        /// <summary>
        /// A synchronous version of <see cref="INfieldSurveysService.QueryAsync"/>
        /// </summary>
        public static IQueryable<Survey> Query(this INfieldSurveysService surveysService)
        {
            return surveysService.QueryAsync().Result;
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldSurveysService.SamplingPointsQueryAsync"/>
        /// </summary>
        public static IQueryable<SamplingPoint> SamplingPointsQuery(this INfieldSurveysService surveysService, string surveyId)
        {
            return surveysService.SamplingPointsQueryAsync(surveyId).Result;
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldSurveysService.SamplingPointQueryAsync"/>
        /// </summary>
        /// <param name="surveysService"></param>
        /// <param name="surveyId"></param>
        /// <param name="samplingPointId"></param>
        /// <returns></returns>
        public static SamplingPoint SamplingPointQuery(this INfieldSurveysService surveysService, string surveyId,
            string samplingPointId)
        {
            return surveysService.SamplingPointQueryAsync(surveyId, samplingPointId).Result;
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldSurveysService.SamplingPointDeleteAsync"/>
        /// </summary>
        public static void SamplingPointDelete(this INfieldSurveysService surveysService, string surveyId, SamplingPoint samplingPoint)
        {
            surveysService.SamplingPointDeleteAsync(surveyId, samplingPoint).Wait();
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldSurveysService.SamplingPointAddAsync"/>
        /// </summary>
        public static void SamplingPointAdd(this INfieldSurveysService surveysService, string surveyId, SamplingPoint samplingPoint)
        {
            surveysService.SamplingPointAddAsync(surveyId, samplingPoint).Wait();
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldSurveysService.SamplingPointUpdateAsync"/>
        /// </summary>
        public static void SamplingPointUpdate(this INfieldSurveysService surveysService, string surveyId, SamplingPoint samplingPoint)
        {
            surveysService.SamplingPointUpdateAsync(surveyId, samplingPoint).Wait();
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldSurveysService.SamplingPointImageAddAsync(string, string, string)"/>
        /// </summary>
        public static void SamplingPointImageAdd(this INfieldSurveysService surveysService, string surveyId, string samplingPointId, string filePath)
        {
            surveysService.SamplingPointImageAddAsync(surveyId, samplingPointId, filePath).Wait();
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldSurveysService.SamplingPointImageAddAsync(string, string, string, byte[])"/>
        /// </summary>
        public static void SamplingPointImageAdd(this INfieldSurveysService surveysService, string surveyId, string samplingPointId, string filename, byte[] content)
        {
            surveysService.SamplingPointImageAddAsync(surveyId, samplingPointId, filename, content).Wait();
        }
    }
}

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
using Nfield.Services;

namespace Nfield.SDK.Samples
{
    public class NfieldSampleManagement
    {
        private readonly INfieldSurveySampleService _surveySampleService;

        /// <summary>
        /// Constructor.
        /// </summary>
        public NfieldSampleManagement(INfieldSurveySampleService surveySampleServiceService)
        {
            _surveySampleService = surveySampleServiceService;
        }

        /// <summary>
        /// Get all sample records
        /// Note that this example does not return the result, allthough your real class will do so.
        /// </summary>
        /// <param name="surveyId">id of the survey for which we want to get the sample records</param>
        public void DownloadSample(string surveyId)
        {
            var sample = _surveySampleService.GetAsync(surveyId).Result;
        }

        /// <summary>
        /// upload sample
        /// Note that this example does not return the result, allthough your real class will do so.
        /// </summary>
        /// <param name="surveyId">id of the survey where we want to upload the sample records to</param>
        /// <param name="sample">the sample to upload</param>
        public void UploadSample(string surveyId, string sample)
        {
            var result = _surveySampleService.PostAsync(surveyId, sample).Result;
        }

        /// <summary>
        /// delete a sample record
        /// Note that this example does not return the result, allthough your real class will do so.
        /// </summary>
        /// <param name="surveyId">id of the survey the wample record belongs to</param>
        /// <param name="respondentKey">respondentKey used to identify which samplerecord to delete</param>
        public void DeleteSample(string surveyId, string respondentKey)
        {
            var result = _surveySampleService.DeleteAsync(surveyId, respondentKey).Result;
        }
    }
}

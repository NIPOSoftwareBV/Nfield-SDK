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

using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Service for making a download data request.
    /// </summary>
    public interface INfieldSurveyDataService
    {
        /// <summary>
        /// Gets the download data url.
        /// </summary>
        Task<BackgroundTask> PostAsync(SurveyDownloadDataRequest surveyDownloadDataRequest);

        /// <summary>
        /// Gets the download data url.
        /// </summary>
        Task<string> PrepareDownload(string surveyId, SurveyDownloadDataRequest surveyDownloadDataRequest);

        /// <summary>
        /// Gets the download data url.
        /// </summary>
        Task<string> PrepareInterviewDownload(string surveyId, int interviewId);

    }
}

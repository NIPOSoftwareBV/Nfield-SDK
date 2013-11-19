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

namespace Nfield.Models
{
    /// <summary>
    /// All possible actions that an user can do that activates a background task.
    /// For the user those actions are visible on the 'Jobs' page.
    /// </summary>
    public enum BackgroundTaskType
    {
        /// <summary>
        /// This is not an actual task.
        /// </summary>
        None = 0,

        /// <summary>
        /// The task to prepare a download of survey data
        /// </summary>
        DownloadSurveyData = 1,


    }
}

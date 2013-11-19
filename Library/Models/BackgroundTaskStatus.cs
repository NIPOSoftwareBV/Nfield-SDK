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
    /// BackgroundTaskStatus.
    /// Represents to current stage in the life cycle of BackgroundTask.
    /// </summary>
    public enum BackgroundTaskStatus
    {
        /// <summary>
        /// The task has been initialized
        /// </summary>
        Created = 0,
        /// <summary>
        /// The task is runing but has not completed yet
        /// </summary>
        Running = 1,
        /// <summary>
        /// The task has been canceled
        /// </summary>
        Canceled = 2,
        /// <summary>
        /// The task completed due to an exception
        /// </summary>
        Faulted = 3,
        /// <summary>
        /// The task completed execution successfully
        /// </summary>
        SuccessfullyCompleted = 4

    }
}

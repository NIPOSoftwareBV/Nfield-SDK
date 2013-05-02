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
using System.Threading.Tasks;

namespace Nfield.Extensions
{
    /// <summary>
    /// Extensions to the <see cref="Task"/> class.
    /// </summary>
    internal static class TaskExtensions
    {
        /// <summary>
        /// Flattens exception structure of AggregateExceptions without the need of catching/rethrowing
        /// </summary>
        public static Task<T> FlattenExceptions<T>(this Task<T> task)
        {
            return task.ContinueWith(previousTask =>
                {
                    var tcs = new TaskCompletionSource<T>();

                    switch (previousTask.Status)
                    {
                        case TaskStatus.Faulted:
                            // Exceptions occured in (one of the) previous tasks
                            if (previousTask.Exception == null)
                            {
                                throw new InvalidOperationException("Faulted Task should have Exception");
                            }
                            tcs.SetException(previousTask.Exception.Flatten().InnerExceptions);
                            break;

                        case TaskStatus.Canceled:
                            tcs.SetCanceled();
                            break;

                        case TaskStatus.RanToCompletion:
                            tcs.SetResult(previousTask.Result);
                            break;

                        default:
                            throw new InvalidOperationException(
                                string.Format("Invalid task status: {0}", previousTask.Status));
                    }

                    return tcs.Task;
                }).Unwrap();
        }
    }
}
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

using Nfield.Models;
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read and update the interviewer data.
    /// </summary>
    public interface INfieldInterviewersService
    {
        /// <summary>
        /// Asks for interviewers work logs and gets the URL to perform the download (UTC datetime)
        /// </summary>
        /// <param name="query">Query model with UTC datetime</param>
        /// <returns>URL to download the file</returns>
        Task<string> QueryLogsAsync(LogQueryModel query);
    }
}
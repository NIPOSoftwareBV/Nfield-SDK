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

using Nfield.Models;
using System;
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Service for managing the blacklist of the domain
    /// </summary>
    public interface INfieldBlacklistService
    {
        /// <summary>
        /// Uploads the blacklist as a csv file
        /// </summary>
        /// <param name="blacklist">blacklist</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns><see cref="BlacklistUploadStatus"/></returns>
        Task<BlacklistUploadStatus> PostAsync(string blacklist);

        /// <summary>
        /// Downloads the blacklist as a tab delimited string, that can be stored as csv file.
        /// </summary>
        Task<string> GetAsync();
    }
}

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
using Nfield.Services;
using System.Linq;

namespace Nfield.Extensions
{
    public static class NfieldCatiInterviewersServiceExtensions
    {
        /// <summary>
        /// A synchronous version of <see cref="INfieldCatiInterviewersService.AddAsync"/>
        /// </summary>
        public static CatiInterviewer Add(this INfieldCatiInterviewersService catiInterviewersService, CatiInterviewer catiInterviewer)
        {
            return catiInterviewersService.AddAsync(catiInterviewer).Result;
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldCatiInterviewersService.RemoveAsync"/>
        /// </summary>
        public static void Remove(this INfieldCatiInterviewersService catiInterviewersService, CatiInterviewer catiInterviewer)
        {
            catiInterviewersService.RemoveAsync(catiInterviewer).Wait();
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldCatiInterviewersService.QueryAsync"/>
        /// </summary>
        public static IQueryable<CatiInterviewer> Query(this INfieldCatiInterviewersService catiInterviewersService)
        {
            return catiInterviewersService.QueryAsync().Result;
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldCatiInterviewersService.ChangePasswordAsync"/>
        /// </summary>
        public static CatiInterviewer ChangePassword(this INfieldCatiInterviewersService catiInterviewersService, CatiInterviewer catiInterviewer, string password)
        {
            return catiInterviewersService.ChangePasswordAsync(catiInterviewer, password).Result;
        }
    }
}

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
using System.Linq;
using Nfield.Models;
using Nfield.Services;
using Nfield.Services.Implementation;

namespace Nfield.Extensions
{
    /// <summary>
    /// Extensions to make the asynchronous methods of <see cref="NfieldInterviewersService"/> synchronous
    /// </summary>
    public static class NfieldInterviewersServiceExtensions
    {
        /// <summary>
        /// A synchronous version of <see cref="INfieldInterviewersService.AddAsync"/>
        /// </summary>
        public static Interviewer Add(this INfieldInterviewersService interviewersService, Interviewer interviewer)
        {
            return interviewersService.AddAsync(interviewer).Result;
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldInterviewersService.RemoveAsync"/>
        /// </summary>
        public static void Remove(this INfieldInterviewersService interviewersService, Interviewer interviewer)
        {
            interviewersService.RemoveAsync(interviewer).Wait();
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldInterviewersService.UpdateAsync"/>
        /// </summary>
        public static Interviewer Update(this INfieldInterviewersService interviewersService, Interviewer interviewer)
        {
            return interviewersService.UpdateAsync(interviewer).Result;
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldInterviewersService.QueryAsync"/>
        /// </summary>
        public static IQueryable<Interviewer> Query(this INfieldInterviewersService interviewersService)
        {
            return interviewersService.QueryAsync().Result;
        }

        /// <summary>
        /// A synchronous version of <see cref="INfieldInterviewersService.ChangePasswordAsync"/>
        /// </summary>
        public static Interviewer ChangePassword(this INfieldInterviewersService interviewersService, Interviewer interviewer, string password)
        {
            return interviewersService.ChangePasswordAsync(interviewer, password).Result;
        }
    }
}

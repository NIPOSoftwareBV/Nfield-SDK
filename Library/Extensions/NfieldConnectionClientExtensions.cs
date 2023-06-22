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
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Nfield.Exceptions;
using Nfield.Infrastructure;
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;

namespace Nfield.Extensions
{
    /// <summary>
    /// Extensions to <see cref="NfieldConnection"/> class to make the asynchronous methods synchronous
    /// </summary>
    public static class NfieldConnectionClientExtensions
    {
        private static Uri BackgroundActivityUrl(this INfieldConnectionClient client, string activityId)
        {
            return new Uri(client.NfieldServerUri, $"BackgroundActivities/{activityId}");
        }

        /// <summary>
        /// Method that polls the activity status until it completes.
        /// </summary>
        /// <param name="connectionClient">NField Connection Client (this)</param>
        /// <param name="activityId">The id of the activity to wait for.</param>
        /// <param name="fieldNameResult">The name of the result field</param>
        /// <returns>The <see cref="BackgroundActivityStatus" /> id.</returns>
        internal static async Task<T> GetActivityResultAsync<T>(this INfieldConnectionClient connectionClient, string activityId, string fieldNameResult)
        {
            while (true)
            {
                var response = await connectionClient.Client.GetAsync(connectionClient.BackgroundActivityUrl(activityId));

                var content = await response.Content.ReadAsStringAsync();

                var obj = JObject.Parse(content);
                var status = obj["Status"].Value<int>();

                switch (status)
                {
                    case 0: // pending
                    case 1: // started
                        Thread.Sleep(millisecondsTimeout: 200);
                        break;
                    case 2: // succeeded
                        return obj[fieldNameResult].Value<T>();
                    case 3: // failed
                    default:
                        throw new NfieldErrorException("Action did not complete successfully");
                }
            }
        }
    }
}

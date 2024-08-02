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

using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;
using Nfield.Quota.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldInterviewersService"/>
    /// </summary>

    internal class NfieldInterviewersService : INfieldInterviewersService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldInterviewersService


        /// <summary>
        /// <see cref="INfieldInterviewersService.QueryLogsAsync"/>
        /// </summary>
        public async Task<string> QueryLogsAsync(LogQueryModel query)
        {
            Ensure.ArgumentNotNull(query, nameof(query));

            var uri = new Uri(ConnectionClient.NfieldServerUri, "InterviewersWorklog");

            return await Client.PostAsJsonAsync(uri, query)
                          .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                          .ContinueWith(task => JsonConvert.DeserializeObject<BackgroundActivityStatus>(task.Result))
                          .ContinueWith(task => ConnectionClient.GetActivityResultAsync<string>(task.Result.ActivityId, "DownloadDataUrl").Result)
                          .FlattenExceptions().ConfigureAwait(false);
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion

        private INfieldHttpClient Client
        {
            get { return ConnectionClient.Client; }
        }
    }

    internal class UpdateInterviewer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public bool IsSupervisor { get; set; }
    }
}

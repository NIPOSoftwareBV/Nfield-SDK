using Newtonsoft.Json;
using Nfield.Extensions;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.SDK.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nfield.SDK.Services.Implementation
{
    internal class NFieldMeService : INFieldMeService, INfieldConnectionClientObject
    {

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        public Task<UserRoleModel> GetUserRoles()
        {
            return ConnectionClient.Client.GetAsync(MeApi)
             .ContinueWith(
                 responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
             .ContinueWith(
                 stringTask =>
                 JsonConvert.DeserializeObject<UserRoleModel>(stringTask.Result))
             .FlattenExceptions();
        }

        private Uri MeApi
        {
            get { return new Uri(ConnectionClient.NfieldServerUri, "me/role/"); }
        }

    }
}

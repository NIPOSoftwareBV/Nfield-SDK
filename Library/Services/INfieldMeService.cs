using Nfield.SDK.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nfield.SDK.Services
{
    public interface INFieldMeService
    {
        Task<UserRoleModel> GetUserRoles();
    }
}

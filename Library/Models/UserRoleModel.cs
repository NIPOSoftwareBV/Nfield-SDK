using System;
using System.Collections.Generic;
using System.Text;

namespace Nfield.SDK.Models
{
    /// <summary>
    /// Contains the user role for the currently authenticated user.
    /// </summary>
    public class UserRoleModel
    {
        /// <summary>
        /// The user roles for the currently authenticated user. If multiple roles are assigned, the roles are ordered from 'most powerful' to 'least powerful'
        /// </summary>
        public IEnumerable<string> UserRoles { get; set; }

        /// <summary>
        /// The permissions for the currently authenticated session.
        /// </summary>
        public IEnumerable<string> Permissions { get; set; }
    }
}

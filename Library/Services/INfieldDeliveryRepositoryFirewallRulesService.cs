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

using System.Linq;
using System.Threading.Tasks;
using Nfield.SDK.Models.Delivery;

namespace Nfield.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface INfieldDeliveryRepositoryFirewallRulesService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <returns></returns>
        Task<IQueryable<FirewallRuleModel>> QueryAsync(long repositoryId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <param name="firewallRuleId"></param>
        /// <returns></returns>
        Task<FirewallRuleModel> GetAsync(long repositoryId, int firewallRuleId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task PostAsync(long repositoryId, FirewallRuleModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repositoryId"></param>
        /// <param name="firewallRuleId"></param>
        /// <returns></returns>
        Task DeleteAsync(long repositoryId, int firewallRuleId);
    }
}

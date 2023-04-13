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
    /// Set of methods to manage the respository fiewall rules
    /// </summary>
    public interface INfieldDeliveryRepositoryFirewallRulesService
    {
        /// <summary>
        /// Returns the firewall rules configured for the repository database.
        /// </summary>
        /// <param name="repositoryId">The repository id.</param>
        /// <returns>A list of firewall rules</returns>
        Task<IQueryable<FirewallRuleModel>> QueryAsync(long repositoryId);

        /// <summary>
        /// Returns the firewall rule for the requested repository based on the given identifier.
        /// </summary>
        /// <param name="repositoryId">The repository id.</param>
        /// <param name="firewallRuleId">The firewall rule id.</param>
        /// <returns>The firewall rule associated with the requested id.</returns>
        Task<FirewallRuleModel> GetAsync(long repositoryId, int firewallRuleId);

        /// <summary>
        /// Adds a firewall rule to allow access to repository database for specific IP addresses.
        /// </summary>
        /// <param name="repositoryId">The repository id.</param>
        /// <returns>The firewall rule id, if succeeded. The appropriate exception in case of failure.</returns>
        Task<FirewallRuleModel> PostAsync(long repositoryId, FirewallRuleModel model);

        /// <summary>
        /// Deletes the specified firewall rule from the Repository database.
        /// </summary>
        /// <param name="repositoryId">The repository id.</param>
        /// <param name="firewallRuleId">The id of the firewall rule to delete.</param>
        /// <returns><c>NoContentResult</c>, if succeeded. The appropriate exception in case of failure.</returns>
        Task DeleteAsync(long repositoryId, int firewallRuleId);
    }
}

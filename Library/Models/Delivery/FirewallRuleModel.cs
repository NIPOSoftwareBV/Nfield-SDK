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

namespace Nfield.SDK.Models.Delivery
{
    /// <summary>
    /// Describes the firewall rule for an Azure Sql Database.
    /// </summary>
    public class FirewallRuleModel
    {
        /// <summary>
        /// The identifier for the rule
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the rule. Unique per database.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The starting IP Address of the allowed range.
        /// </summary>
        public string StartIpAddress { get; set; }

        /// <summary>
        /// The last IP address that will be allowed (in the specified range).
        /// </summary>
        public string EndIpAddress { get; set; }
    }
}
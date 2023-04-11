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
    public class RepositoryModel
    {
        /// <summary>
        /// The id of the repository.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The repository name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The status id of the repository.
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// The plan id of the repository.
        /// </summary>
        public int? PlanId { get; set; }

        /// <summary>
        /// The name of the database that has been provisioned for this repository. 
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// The user that created the repository.
        /// </summary>
        public string CreatedBy { get; set; }
    }
}

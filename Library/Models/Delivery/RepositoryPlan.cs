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

using System.Collections.Generic;

namespace Nfield.SDK.Models.Delivery
{
    /// <summary>
    /// Represents a subscription plan for Data Repositories.
    /// </summary>
    public class RepositoryPlan
    {
        /// <summary>
        /// The Id for the plan.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// The name of the plan.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A short description explaining what's the plan is about.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The monthly price of the plan.
        /// </summary>
        public Amount Price { get; set; }

        /// <summary>
        /// Returns the plans for Data Repositories. This is a temporarily solution and the data are hard-coded in Reporting. Especially the pricing info should be taken dynamically from another system/api.
        /// </summary>
        /// <remarks>
        ///	Plan 1: Tests and small surveys
        /// 5 DTU, 2GB MS price 4.21 per month, Nfield price 50.00 per month
        /// Good enough for running PoC's and small surveys < 2.000 interviews, with Answers < 30 per interview
        /// Plan 2: Light
        /// 10 DTU, 100 GB MS price 12.65 per month.Nfield price 100.00 per month
        /// Interview up to 10.000 < 50 answers for good performance. Lots of storage and good enough for daily reloads
        /// Plan 3: Medium
        /// 100 DTU, 250 GB MS price 63.26, Nfield price 500.00 per month
        /// Up to 100.000 interviews< 100 answers for good performance, daily refreshes
        /// Plan 4: Heavy duty
        /// 200 DTU, 250GB MS price 253.01 Nfield price 1000.00 per month
        /// Half of the total Nfield reporting performance for one deployment, just for one repo! Multiple 100k+ surveys for daily refreshes, or 10's of thousand of interviews with direct query and still have good performance!
        ///</remarks>
        public static IList<RepositoryPlan> GetPlans()
        {
            return new List<RepositoryPlan>
        {
            new RepositoryPlan
            {
                Id = 1,
                Name = "Tests and small surveys",
                Description ="Good enough for running PoC's and small surveys < 2.000 interviews, with Answers < 30 per interview.",
                Price = new Amount
                {
                    Currency = "EUR",
                    Value =50
                }
            },
            new RepositoryPlan
            {
                Id = 2,
                Name = "Light",
                Description ="Interview up to 10.000 < 50 answers for good performance. Lots of storage and good enough for daily reloads.",
                Price = new Amount
                {
                    Currency = "EUR",
                    Value =100
                }
            },
            new RepositoryPlan
            {
                Id = 3,
                Name = "Medium",
                Description ="Up to 100.000 interviews < 100 answers for good performance, daily refreshes.",
                Price = new Amount
                {
                    Currency = "EUR",
                    Value =500
                }
            },
            new RepositoryPlan
            {
                Id = 4,
                Name = "Heavy duty",
                Description ="Half of the total Nfield reporting performance for one deployment, just for one repo! Multiple 100k+ surveys for daily refreshes, or 10's of thousand of interviews with direct query and still have good performance!",
                Price = new Amount
                {
                    Currency = "EUR",
                    Value =1000
                }
            }
        };
        }
    }
}
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

namespace Nfield.Models
{
    public class SurveyMetricCounts
    {
        /// <summary>
        /// The name of the metric.
        /// 
        /// The possible metric names are:
        /// - Page Complexity
        /// - Expression Complexity
        /// - Interview State Size
        /// - Expensive Command Count
        /// </summary>
        public string MetricName { get; set; }

        /// <summary>
        /// The number of times an interview reached the warn or kill threshold for the current published version of the survey, for the specified metric
        /// </summary>
        public MetricCounts All { get; set; }

        /// <summary>
        /// The number of times an interview reached the warn or kill threshold for the current published version of the survey, for the specified metric
        /// </summary>
        public MetricCounts Published { get; set; }
    }
}

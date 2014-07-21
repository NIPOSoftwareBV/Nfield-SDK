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
using System;

namespace Nfield.Models
{
   /// <summary>
   /// 
   /// </summary>
   public class InterviewDetailsModel
    {
        /// <summary>
        /// Interview Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Quality Control state of an interview
        /// </summary>
        public InterviewQuality InterviewQuality { get; set; }

        /// <summary>
        /// Date and Time the interview was started
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Id of the interviewer
        /// </summary>
        public string InterviewerId { get; set; }

        /// <summary>
        /// Id of the sampling point, null for surveys without sampling points
        /// </summary>
        public string SamplingPointId { get; set; }

        /// <summary>
        /// Id of the Fieldwork Office
        /// </summary>
        public string OfficeId { get; set; }
    }
}

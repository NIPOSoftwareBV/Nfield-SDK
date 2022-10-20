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
    /// <summary>
    /// Status of the blacklist upload
    /// </summary>
    public class BlacklistUploadStatus
    {
        /// <summary>
        /// Description of the status of the upload operation
        /// </summary>
        public string ProcessingStatus { get; set; }

        /// <summary>
        /// Total number of records contained in the request
        /// </summary>
        public int TotalRecordCount { get; set; }

        /// <summary>
        /// Number of records inserted as result of the operation
        /// </summary>
        public int InsertedCount { get; set; }

        /// <summary>
        /// Number of records updated as result of the operation
        /// </summary>
        public int UpdatedCount { get; set; }

        /// <summary>
        /// Number of records contained in the request that result in a duplicate key
        /// </summary>
        public int DuplicateKeyCount { get; set; }

        /// <summary>
        /// Number of records contained in the request with no key
        /// </summary>
        public int EmptyKeyCount { get; set; }

        /// <summary>
        /// Number of records contained in the request with invalid key
        /// </summary>
        public int InvalidKeyCount { get; set; }

        /// <summary>
        /// Number of invalid records contained in the request
        /// </summary>
        public int InvalidDataCount { get; set; }

        /// <summary>
        ///  Number of records skipped during the operation
        /// </summary>
        public int SkippedCount { get; set; }

        /// <summary>
        /// True if the data posted with the request has an invalid header
        /// </summary>
        public bool HeaderInvalid { get; set; }

        /// <summary>
        /// Number of header columns invalid in the request's data
        /// </summary>
        public int HeaderInvalidColumnsCount { get; set; }

        /// <summary>
        /// True if the data in the request does not match the header
        /// </summary>
        public bool HeaderDataMismatch { get; set; }
    }
}

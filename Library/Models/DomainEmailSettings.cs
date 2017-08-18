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
    /// Dto for the email settings of a domain
    /// </summary>
    public class DomainEmailSettings
    {
        /// <summary>
        /// The 'from' email address
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// Name of the sender
        /// </summary>
        public string FromName { get; set; }

        /// <summary>
        /// The 'reply to' address
        /// </summary>
        public string ReplyToAddress { get; set; }

        /// <summary>
        /// The physical address
        /// </summary>
        public string PostalAddress { get; set; }

        /// <summary>
        /// The default 'from' email address
        /// </summary>
        public string DefaultFromAddress { get; set; }

        /// <summary>
        /// The default 'reply to' address
        /// </summary>
        public string DefaultReplyToAddress { get; set; }
    }
}

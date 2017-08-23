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

using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    /// <summary>
    /// Service for getting and changing the email settings at the domain level
    /// </summary>
    public interface INfieldDomainEmailSettingsService
    {
        /// <summary>
        /// Gets the email settings at the domain level
        /// </summary>
        Task<DomainEmailSettings> GetAsync();

        /// <summary>
        /// Changes the email settings at the domain level (dns validation on from email address)
        /// </summary>
        Task<EmailSettingsEditable> PutAsync(EmailSettingsEditable settings);
    }
}

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
    public class SurveyOtherSettingsRequest
    {
        public string Owner { get; set; }

        public bool? BackButtonAvailable { get; set; }

        public bool? PauseButtonAvailable { get; set; }

        public bool? ClearButtonAvailable { get; set; }

        public bool? AllowOnlyKnownRespondents { get; set; }

    }

    public class SurveyOtherSettingsResponse
    {
        public string SurveyId { get; set; }

        public User Owner { get; set; }

        public string EncryptionKey { get; set; }

        public bool BackButtonAvailable { get; set; }

        public bool PauseButtonAvailable { get; set; }

        public bool ClearButtonAvailable { get; set; }

        public bool AllowOnlyKnownRespondents { get; set; }

    }

    public class User
    {
        /// <summary>
        /// Unique Id of user
        /// </summary>
        public string Id { get; set; }

        public string UserName { get; set; }
    }
}

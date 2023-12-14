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

namespace Nfield.SDK.Models
{
    /// <summary>
    /// A warning from a language translation
    /// </summary>
    public class TranslationWarning
    {
        /// <summary>
        /// Type of warning. For now we only have 'unknown-variable'
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Variable which is causing the warning
        /// </summary>
        public string Variable { get; set; }

        /// <summary>
        /// Translation to which the warning belongs to
        /// </summary>
        public string Translation { get; set; }

        /// <summary>
        /// Language of the translation
        /// </summary>
        public string Language { get; set; }
    }
}

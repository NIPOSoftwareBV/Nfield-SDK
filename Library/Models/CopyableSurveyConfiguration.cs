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
    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4070:Non-flags enums should not be marked with \"FlagsAttribute\"",
        Justification = "It is a flags enum")]
    public enum CopyableSurveyConfiguration
    {
        None = 0,
        All = ~None,

        QuotaFrame = 1,
        QuestionnaireScript = 2,
        TranslationLanguages = 4, // this is called "buttons and messages" in the UI
        MediaFiles = 8,
        ResponseCodes = 16,
        InterviewButtons = 32, // this is called "interview interactions" in the UI
        LandingPage = 64,
    }
}

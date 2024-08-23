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

using System.Linq;
using System.Threading.Tasks;
using Nfield.Models;

namespace Nfield.Services
{
    public interface INfieldParentSurveyWavesService
    {
        /// <summary>
        /// Gets survey queryable object.
        /// </summary>
        Task<IQueryable<Survey>> GetParentSurveyWavesAsync(string parentSurveyId);

        /// <summary>
        /// Creates a new wave.
        /// </summary>
        Task<Survey> AddWaveAsync(string parentSurveyId, ParentSurveyWave survey);

        /// <summary>
        /// Creates a new wave from an existing wave.
        /// The unique id of the survey (Guid) to be copy
        /// The Survey Id must be a wave (Has a parent Survey)
        /// </summary>
        Task<Survey> CopyWaveAsync(string parentSurveyId, string waveId, ParentSurveyWaveCopy survey);
    }
}

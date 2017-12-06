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
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    internal class NfieldSurveyScriptFragmentService : INfieldSurveyScriptFragmentService, INfieldConnectionClientObject
    {
        #region Implementation of INfieldSurveyScriptFragmentService

        public Task<IQueryable<SurveyScript>> QueryAsync(string surveyId)
        {
            return Task.Factory.StartNew(() => { return Enumerable.Empty<SurveyScript>().AsQueryable(); });
        }

        public Task PostAsync(string surveyId, SurveyScript surveyScript)
        {
            return Task.Factory.StartNew(() => { });
        }

        public Task RemoveAsync(string surveyId, string fileName)
        {
            return Task.Factory.StartNew(() => { });
        }

        #endregion

        #region Implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
        }

        #endregion
    }
}
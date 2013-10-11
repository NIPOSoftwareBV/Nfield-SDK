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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyScriptService"/>
    /// </summary>
    public class NfieldSurveyScriptServiceTests : NfieldServiceTestsBase
    {
        #region GetAsync

        [Fact]
        public void TestGetAsync_WhenScriptExits_ReturnsCorrectScript()
        {
            const string surveyId = "SurveyId";
            const string script = "this is the script";
            var expected = new SurveyScript{Script = script};

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "SurveyScript/" + surveyId))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expected))));

            var target = new NfieldSurveyScriptService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.GetAsync(surveyId).Result;

            Assert.Equal(script, actual.Script);
        }


        #endregion
    }
}

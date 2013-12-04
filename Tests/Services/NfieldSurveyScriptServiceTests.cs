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
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
            const string fileName = "fileq.odin";

            var expected = new SurveyScript{Script = script,FileName = fileName};

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "surveyscript/" + surveyId))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expected))));

            var target = new NfieldSurveyScriptService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.GetAsync(surveyId).Result;

            Assert.Equal(script, actual.Script);
            Assert.Equal(fileName, actual.FileName);
        }

        #endregion

        #region PostAsync

        [Fact]
        public void TestPostAsync_FileDoesNotExist_ThrowsFileNotFoundException()
        {
            var target = new NfieldSurveyScriptService();
            Assert.Throws<FileNotFoundException>(
                () =>
                    UnwrapAggregateException(target.PostAsync("surveyId", "NotExistingFile")));
        }

        [Fact]
        public void TestPostAsync_SurveyScriptModelIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveyScriptService();
            Assert.Throws<ArgumentNullException>(
                () =>
                    UnwrapAggregateException(target.PostAsync("surveyId", surveyScript:null)));
        }

        [Fact]
        public void TestPostAsync_ServerAccepts_ReturnsSurveyScript()
        {
            const string surveyId = "SurveyId";
            const string script = "this is the script";
            const string fileName = "fileq.odin";

            var surveyScript = new SurveyScript { Script = script, FileName = fileName };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(surveyScript));

            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(ServiceAddress + "surveyscript/" + surveyId, surveyScript))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldSurveyScriptService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.PostAsync(surveyId,surveyScript).Result;

            Assert.Equal(surveyScript.FileName, actual.FileName);
            Assert.Equal(surveyScript.Script, actual.Script);
        }

        #endregion
    }
}

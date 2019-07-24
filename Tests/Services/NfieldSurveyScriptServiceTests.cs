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

using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services.Implementation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyScriptService"/>
    /// </summary>
    public class NfieldSurveyScriptServiceTests : NfieldServiceTestsBase
    {
        private readonly NfieldSurveyScriptService _target;
        private readonly Mock<IFileSystem> _mockedFileSystem;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;

        public NfieldSurveyScriptServiceTests()
        {
            _mockedFileSystem = new Mock<IFileSystem>();

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldSurveyScriptService(_mockedFileSystem.Object);
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

        }
        #region GetAsync

        [Fact]
        public void TestGetAsync_WhenScriptExits_ReturnsCorrectScript()
        {
            const string surveyId = "SurveyId";
            const string script = "this is the script";
            const string fileName = "fileq.odin";

            var expected = new SurveyScript { Script = script, FileName = fileName };

            _mockedHttpClient
                 .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"Surveys/{surveyId}/Script/")))
                 .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expected))));

            var actual = _target.GetAsync(surveyId).Result;

            Assert.Equal(script, actual.Script);
            Assert.Equal(fileName, actual.FileName);
        }

        [Fact]
        public void TestGetAsync_WhenSpecificVersionOfScriptExits_ReturnsCorrectScript()
        {
            const string surveyId = "SurveyId";
            const string script = "this is the script";
            const string fileName = "fileq.odin";
            const string eTag = "etag";

            var expected = new SurveyScript { Script = script, FileName = fileName };

            _mockedHttpClient
                 .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"Surveys/{surveyId}/Script/{eTag}")))
                 .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expected))));

            var actual = _target.GetAsync(surveyId, eTag).Result;

            Assert.Equal(script, actual.Script);
            Assert.Equal(fileName, actual.FileName);
        }

        #endregion

        #region PostAsync

        [Fact]
        public void TestPostAsync_FileDoesNotExist_ThrowsFileNotFoundException()
        {
            _mockedFileSystem.Setup(fs => fs.Path.GetFileName(It.IsAny<string>())).Returns(string.Empty);
            _mockedFileSystem.Setup(fs => fs.File.Exists(It.IsAny<string>())).Returns(false);

            Assert.Throws<FileNotFoundException>(
                () =>
                    UnwrapAggregateException(_target.PostAsync("surveyId", "NotExistingFile")));
        }

        [Fact]
        public void TestPostAsync_SurveyScriptModelIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                    UnwrapAggregateException(_target.PostAsync("surveyId", surveyScript: null)));
        }

        [Fact]
        public void TestPostAsync_ServerAccepts_ReturnsSurveyScript()
        {
            const string surveyId = "SurveyId";
            const string script = "this is the script";
            const string fileName = "fileq.odin";

            var surveyScript = new SurveyScript { Script = script, FileName = fileName };

            var content = new StringContent(JsonConvert.SerializeObject(surveyScript));

            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{surveyId}/Script/"), surveyScript))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var actual = _target.PostAsync(surveyId, surveyScript).Result;

            Assert.Equal(surveyScript.FileName, actual.FileName);
            Assert.Equal(surveyScript.Script, actual.Script);
            Assert.Null(actual.WarningMessages);
        }

        [Fact]
        public void TestPostAsync_ServerAccepts_WarningMessages_ReturnsSurveyScriptAndMessages()
        {
            const string surveyId = "SurveyId";
            const string script = "this is the script";
            const string fileName = "fileq.odin";
            var warningMessages = new List<string>
            {
                "Warning1",
                "warning2"
            };

            var surveyScript = new SurveyScript { Script = script, FileName = fileName, WarningMessages = warningMessages };

            var content = new StringContent(JsonConvert.SerializeObject(surveyScript));

            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{surveyId}/Script/"), surveyScript))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var actual = _target.PostAsync(surveyId, surveyScript).Result;

            Assert.Equal(surveyScript.FileName, actual.FileName);
            Assert.Equal(surveyScript.Script, actual.Script);
            Assert.Equal(surveyScript.WarningMessages, actual.WarningMessages);
        }

        [Fact]
        public void TestPostAsync_FileDoesExists_FileUploaded()
        {
            const string surveyId = "SurveyId";
            const string script = "this is the script";
            const string fileName = "fileq.odin";

            _mockedFileSystem.Setup(fs => fs.Path.GetFileName(It.IsAny<string>())).Returns(fileName);
            _mockedFileSystem.Setup(fs => fs.File.Exists(It.IsAny<string>())).Returns(true);
            _mockedFileSystem.Setup(fs => fs.File.ReadAllText(It.IsAny<string>())).Returns(script);

            var surveyScript = new SurveyScript { Script = script, FileName = fileName };

            var stringContent = new StringContent(JsonConvert.SerializeObject(surveyScript));


            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(It.IsAny<Uri>(), It.IsAny<SurveyScript>()))
                .Returns(CreateTask(HttpStatusCode.OK, stringContent));

            _target.PostAsync(surveyId, fileName);

            _mockedHttpClient.Verify(hc => hc.PostAsJsonAsync(It.Is<Uri>(uri => uri.AbsolutePath.Contains(surveyId)),
                        It.Is<SurveyScript>(scripts => scripts.FileName == fileName && scripts.Script == script)), Times.Once());
        }

        #endregion
    }
}

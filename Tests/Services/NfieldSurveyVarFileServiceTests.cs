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
using System.Net;
using System.Net.Http;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyVarFileService"/>
    /// </summary>
    public class NfieldSurveyVarFileServiceTests : NfieldServiceTestsBase
    {
        private readonly NfieldSurveyVarFileService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;

        public NfieldSurveyVarFileServiceTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldSurveyVarFileService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

        }
        #region GetAsync

        [Fact]
        public void TestGetAsync_WhenScriptExists_ReturnsCorrectVarFile()
        {
            const string surveyId = "SurveyId";
            const string varFile = "this is the varFile";
            const string fileName = "file.var";

            var expected = new SurveyVarFile { FileContent = varFile, FileName = fileName };

            _mockedHttpClient
                 .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"Surveys/{surveyId}/VarFile/")))
                 .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expected))));

            var actual = _target.GetAsync(surveyId).Result;

            Assert.Equal(varFile, actual.FileContent);
            Assert.Equal(fileName, actual.FileName);
        }

        [Fact]
        public void TestGetAsync_WhenSpecificVersionOfScriptExits_ReturnsCorrectScript()
        {
            const string surveyId = "SurveyId";
            const string varFile = "this is the varFile";
            const string fileName = "file.var";
            const string eTag = "etag";

            var expected = new SurveyVarFile { FileContent = varFile, FileName = fileName };

            _mockedHttpClient
                 .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"Surveys/{surveyId}/VarFile/{eTag}")))
                 .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expected))));

            var actual = _target.GetAsync(surveyId, eTag).Result;

            Assert.Equal(varFile, actual.FileContent);
            Assert.Equal(fileName, actual.FileName);
        }

        #endregion
    }
}

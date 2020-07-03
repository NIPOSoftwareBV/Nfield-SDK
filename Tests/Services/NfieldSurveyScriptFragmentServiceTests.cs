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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    public class NfieldSurveyScriptFragmentServiceTests : NfieldServiceTestsBase
    {
        #region  QueryAsync

        [Fact]
        public void TestQueryAsync_WhenFilePresent_ReturnsCorrectName()
        {
            const string surveyId = "SurveyId";
            const string fileName = "MyFileName";
            var expected = new List<string> { fileName };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/" + surveyId + "/ScriptFragments/")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expected))));

            var target = new NfieldSurveyScriptFragmentService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.QueryAsync(surveyId).Result.ToList();

            Assert.Single(actual);
            Assert.Equal(fileName, actual[0]);
        }

        #endregion

        #region GetAsync

        [Fact]
        public void TestGetAsync_Always_ReturnsExpectedResult()
        {
            const string surveyId = "SurveyId";
            const string fileName = "MyFileName";
            const string script = "*END";

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/" + surveyId + "/ScriptFragments/" + fileName)))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(script)));

            var target = new NfieldSurveyScriptFragmentService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var task = target.GetAsync(surveyId, fileName);
            var actual = task.Result;

            Assert.Equal(script, actual);
        }

        [Fact]
        public void TestGetAsync_WhenFileNameContainsAmpersand_ReturnsExpectedResult()
        {
            const string surveyId = "SurveyId";
            const string fileName = "MyFileName&";
            const string script = "*END";

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/" + surveyId + "/ScriptFragments/MyFileName%26")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(script)));

            var target = new NfieldSurveyScriptFragmentService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var task = target.GetAsync(surveyId, fileName);
            var actual = task.Result;

            Assert.Equal(script, actual);
        }

        #endregion

        #region  AddOrUpdateAsync

        [Fact]
        public void TestAddOrUpdateAsync_Always_DoesNotThrow()
        {
            const string surveyId = "SurveyId";
            const string fileName = "MyFileName";
            const string script = "*END";

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsync(new Uri(ServiceAddress, "Surveys/" + surveyId + "/ScriptFragments/" + fileName),
                    It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldSurveyScriptFragmentService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.AddOrUpdateAsync(surveyId, fileName, script).Wait();
        }

        #endregion

        #region  RemoveAsync

        [Fact]
        public void TestRemoveAsync_Always_DoesNotThrow()
        {
            const string surveyId = "SurveyId";
            const string fileName = "MyFileName";

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, "Surveys/" + surveyId + "/ScriptFragments/" + fileName)))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldSurveyScriptFragmentService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.RemoveAsync(surveyId, fileName).Wait();
        }

        #endregion
    }
}
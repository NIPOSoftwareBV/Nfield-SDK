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
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyInterviewInteractionsSettingsService"/>
    /// </summary>
    public class NfieldSurveyInterviewInteractionsSettingsServiceTests : NfieldServiceTestsBase
    {
        private const string SurveyId = "TestSurveyId";

        #region "Get"

        [Fact]
        public void TestGetAsync_SurveyIdIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveyInterviewInteractionsSettingsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.GetAsync(null)));
        }

        [Fact]
        public void TestGetAsync_SurveyIdIsEmpty_ThrowsArgumentException()
        {
            var target = new NfieldSurveyInterviewInteractionsSettingsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.GetAsync("")));
        }

        [Fact]
        public void TestGetAsync_ServerReturnsQuery_ReturnsListWithSettings()
        {
            var expectedSettings = new SurveyInterviewInteractionsSettings
            {
                BackButtonAvailable = true,
                PauseButtonAvailable = false,
                ClearButtonAvailable = true
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/" + SurveyId + "/InterviewInteractionsSettings")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSettings))));

            var target = new NfieldSurveyInterviewInteractionsSettingsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSettings = target.GetAsync(SurveyId).Result; ;
            Assert.Equal(expectedSettings.BackButtonAvailable, actualSettings.BackButtonAvailable);
            Assert.Equal(expectedSettings.PauseButtonAvailable, actualSettings.PauseButtonAvailable);
            Assert.Equal(expectedSettings.ClearButtonAvailable, actualSettings.ClearButtonAvailable);
        }

        #endregion

        #region "Update"

        [Fact]
        public void TestUpdateAsync_SurveyIdIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveyInterviewInteractionsSettingsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync(null, new SurveyInterviewInteractionsSettings())));
        }

        [Fact]
        public void TestUpdateAsync_SurveyIdIsEmpty_ThrowsArgumentException()
        {
            var target = new NfieldSurveyInterviewInteractionsSettingsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.UpdateAsync("", new SurveyInterviewInteractionsSettings())));
        }

        [Fact]
        public void TestUpdateAsync_SettingsIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveyInterviewInteractionsSettingsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync("SurveyId", null)));
        }

        [Fact]
        public void TestUpdateAsync_ServerAcceptsSettings_ReturnsSettings()
        {
            var expectedSettings = new SurveyInterviewInteractionsSettings
            {
                BackButtonAvailable = true,
                PauseButtonAvailable = false,
                ClearButtonAvailable = true
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(expectedSettings));
            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(new Uri(ServiceAddress, "Surveys/" + SurveyId + "/InterviewInteractionsSettings"), expectedSettings))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldSurveyInterviewInteractionsSettingsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSettings = target.UpdateAsync(SurveyId, expectedSettings).Result;

            Assert.Equal(expectedSettings.BackButtonAvailable, actualSettings.BackButtonAvailable);
            Assert.Equal(expectedSettings.PauseButtonAvailable, actualSettings.PauseButtonAvailable);
            Assert.Equal(expectedSettings.ClearButtonAvailable, actualSettings.ClearButtonAvailable);
        }

        #endregion
    }
}

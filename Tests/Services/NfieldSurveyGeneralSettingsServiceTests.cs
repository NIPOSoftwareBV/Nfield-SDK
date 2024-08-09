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
    /// Tests for <see cref="NfieldSurveyGeneralSettingsService"/>
    /// </summary>
    public class NfieldSurveyGeneralSettingsServiceTests : NfieldServiceTestsBase
    {
        private readonly NfieldSurveyGeneralSettingsService _target;
        private readonly Mock<INfieldHttpClient> _mockedHttpClient;

        const string SurveyId = "MySurvey";

        public NfieldSurveyGeneralSettingsServiceTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldSurveyGeneralSettingsService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);
        }

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_SurveyGeneralSettingsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.QueryAsync(null)));
        }     

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithSurveyGeneralSettings()
        {
            var expectedSurveyGeneralSettings = new SurveyGeneralSettingsResponse
            {
                Description = "X Type",
                Client = "client1",
                Name = "X name",
                ExcludeFromAutomaticCleanup = true,
                Owner = new User { Id = "userid", UserName = "user name"}
            };

            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/GeneralSettings")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSurveyGeneralSettings))));


            var actualSurveyGeneralSettings = _target.QueryAsync(SurveyId).Result;
            Assert.Equal(expectedSurveyGeneralSettings.Client, actualSurveyGeneralSettings.Client);
            Assert.Equal(expectedSurveyGeneralSettings.Description, actualSurveyGeneralSettings.Description);
            Assert.Equal(expectedSurveyGeneralSettings.Name, actualSurveyGeneralSettings.Name);
            Assert.Equal(expectedSurveyGeneralSettings.ExcludeFromAutomaticCleanup, actualSurveyGeneralSettings.ExcludeFromAutomaticCleanup);
            Assert.Equal(expectedSurveyGeneralSettings.Owner.Id, actualSurveyGeneralSettings.Owner.Id);
            Assert.Equal(expectedSurveyGeneralSettings.Owner.UserName, actualSurveyGeneralSettings.Owner.UserName);
        }

        #endregion

        #region PatchAsync
        [Fact]
        public void TestPatchAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.UpdateAsync(null, new SurveyGeneralSettings())));
        }

        [Fact]
        public void TestPatchAsync_Always_CallsCorrectURI()
        {
            var expectedUrl = new Uri(ServiceAddress, $"Surveys/{SurveyId}/GeneralSettings");

            _mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(expectedUrl, It.IsAny<SurveyGeneralSettings>()))
                .Returns(CreateTask(HttpStatusCode.OK));

            _target.UpdateAsync(SurveyId, null);

            _mockedHttpClient
                .Verify(client => client.PatchAsJsonAsync(expectedUrl, It.IsAny<SurveyGeneralSettings>()), Times.Once());
        }

        #endregion

    }
}

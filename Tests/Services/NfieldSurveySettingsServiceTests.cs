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
using System.Linq;
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
    /// Tests for <see cref="NfieldSurveySettingsService"/>
    /// </summary>
    public class NfieldSurveySettingsServiceTests : NfieldServiceTestsBase
    {
        const string SurveyId = "MySurvey";

        #region AddAsync

        [Fact]
        public void TestAddOrUpdateAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveySettingsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.AddOrUpdateAsync(null, new SurveySetting())));
        }

        [Fact]
        public void TestAddOrUpdateAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySettingsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.AddOrUpdateAsync("", new SurveySetting())));
        }

        [Fact]
        public void TestAddOrUpdateAsync_ServerAcceptsSetting_ReturnsSetting()
        {
            var setting = new SurveySetting { Name = "Setting X" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(setting));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(ServiceAddress + "Surveys/" + SurveyId + "/Settings", setting))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldSurveySettingsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddOrUpdateAsync(SurveyId, setting).Result;

            Assert.Equal(setting.Name, actual.Name);
        }

        #endregion

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveySettingsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.QueryAsync(null)));
        }

        [Fact]
        public void TestQueryAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySettingsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.QueryAsync("")));
        }

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithSettings()
        {
            var expectedSettings = new SurveySetting[]
            { new SurveySetting{ Name = "X", Value = "X Value" },
              new SurveySetting{ Name = "Y", Value = "Y Value" }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "Surveys/" + SurveyId + "/Settings"))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSettings))));

            var target = new NfieldSurveySettingsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSettings = target.QueryAsync(SurveyId).Result.ToArray(); ;
            Assert.Equal(expectedSettings[0].Name, actualSettings[0].Name);
            Assert.Equal(expectedSettings[1].Name, actualSettings[1].Name);
            Assert.Equal(2, actualSettings.Length);
        }

        #endregion
    }
}

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
using Nfield.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyGeneralSettingsService"/>
    /// </summary>
    public class NfieldSurveyGeneralSettingsServiceTests : NfieldServiceTestsBase
    {
        private readonly NfieldSurveyGeneralSettingsService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;

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
        public void TestQueryAsync_SurveyGeneralSettingsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.QueryAsync("")));
        }

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithSurveyGeneralSettings()
        {
            var expectedSurveyGeneralSettings = new[]
            { new SurveyGeneralSetting {  Description = "X Type", Client = "client1", Name = "X name" },
              new SurveyGeneralSetting {  Description = "Y Type", Client = "client2", Name = "Y name" }
            };
            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "SurveyGeneralSettings/" + SurveyId + "")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSurveyGeneralSettings))));


            var actualSurveyGeneralSettings = _target.QueryAsync(SurveyId).Result.ToArray(); ;
            Assert.Equal(expectedSurveyGeneralSettings[0].Client, actualSurveyGeneralSettings[0].Client);
            Assert.Equal(expectedSurveyGeneralSettings[0].Description, actualSurveyGeneralSettings[0].Description);
            Assert.Equal(expectedSurveyGeneralSettings[0].Name, actualSurveyGeneralSettings[0].Name);
            Assert.Equal(expectedSurveyGeneralSettings[1].Client, actualSurveyGeneralSettings[1].Client);
            Assert.Equal(expectedSurveyGeneralSettings[1].Description, actualSurveyGeneralSettings[1].Description);
            Assert.Equal(expectedSurveyGeneralSettings[1].Name, actualSurveyGeneralSettings[1].Name);
            Assert.Equal(2, actualSurveyGeneralSettings.Length);
        }

        #endregion

        #region PatchAsync
        [Fact]
        public void TestPatchAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.UpdateAsync(null, new List<SurveyGeneralSetting>())));
        }

        [Fact]
        public void TestPatchAsync_Always_CallsCorrectURI()
        {
            var expectedUrl = new Uri(ServiceAddress, $"SurveyGeneralSettings/{SurveyId}");

            _mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(expectedUrl, It.IsAny<IEnumerable<SurveyGeneralSetting>>()))
                .Returns(CreateTask(HttpStatusCode.OK));

            _target.UpdateAsync(SurveyId, null);

            _mockedHttpClient
                .Verify(client => client.PatchAsJsonAsync(expectedUrl, It.IsAny<IEnumerable<SurveyGeneralSetting>>()), Times.Once());
        }

        #endregion
    }
}

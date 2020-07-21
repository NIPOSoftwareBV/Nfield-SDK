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
using Nfield.SDK.Models;
using Nfield.SDK.Services.Implementation;
using Nfield.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyVersionsService"/>
    /// </summary>
    public class NfieldQuotaServiceTests : NfieldServiceTestsBase
    {
        private const string SurveyId = "SurveyId";
        private List<QuotaFrameLevelTarget> quotaFrameTargets = new List<QuotaFrameLevelTarget>();
        private string quotaETag = "637235755520645297";

        #region GetQuotaFrameVersionsAsync

        [Fact]
        public void Test_GetQuotaFrameVersionsAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldQuotaService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.GetQuotaFrameVersionsAsync(null)));
        }

        [Fact]
        public void Test_GetQuotaFrameVersionsAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldQuotaService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.GetQuotaFrameVersionsAsync(string.Empty)));
        }

        [Fact]
        public void Test_GetQuotaFrameVersionsAsync_ReturnsQuotaVersions()
        {
            var expectedVersions = new List<QuotaFrameVersion>
            {
                new QuotaFrameVersion
                {
                    Etag = "637235755520645294"
                },
                new QuotaFrameVersion
                {
                    Etag = "637244319282906584"
                }
            }.ToArray();

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/" + SurveyId + "/QuotaVersions")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedVersions))));

            var target = new NfieldQuotaService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.GetQuotaFrameVersionsAsync(SurveyId).Result.ToArray();

            Assert.Equal(expectedVersions[0].Etag, actual[0].Etag);
            Assert.Equal(expectedVersions[1].Etag, actual[1].Etag);
        }

        #endregion

        #region UpdateQuotaTargetsAsync

        [Fact]
        public void Test_UpdateQuotaTargetsAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldQuotaService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateQuotaTargetsAsync(null, quotaETag, quotaFrameTargets)));
        }

        [Fact]
        public void Test_UpdateQuotaTargetsAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldQuotaService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.UpdateQuotaTargetsAsync(string.Empty, quotaETag, quotaFrameTargets)));
        }

        #endregion

        #region GetQuotaFrameAsync

        [Fact]
        public void Test_GetQuotaFrameAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldQuotaService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.GetQuotaFrameAsync(null, 1)));
        }

        [Fact]
        public void Test_GetQuotaFrameAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldQuotaService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.GetQuotaFrameAsync(string.Empty, 1)));
        }

        [Fact]
        public void Test_GetQuotaFrameAsync_ReturnsQuotaFrame()
        {
            const long quotaVersion = 3;
            var expecteQuotaFrame = new QuotaFrame
            {
                Id = "frameId",
                Target = 100
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/" + SurveyId + "/QuotaVersions/" + quotaVersion)))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expecteQuotaFrame))));

            var target = new NfieldQuotaService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.GetQuotaFrameAsync(SurveyId, quotaVersion).Result;

            Assert.Equal(expecteQuotaFrame.Id, actual.Id);
            Assert.Equal(expecteQuotaFrame.Target, actual.Target);
        }

        #endregion
    }
}

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
using Nfield.Models.NipoSoftware.Nfield.Manager.Api.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveySampleService"/>
    /// </summary>
    public class NfieldSurveySampleTests: NfieldServiceTestsBase
    {
        private const string SurveyId = "MySurvey";

        #region GetAsync

        [Fact]
        public void TestGetAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.GetAsync(null)));
        }

        [Fact]
        public void TestGetAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.GetAsync("  ")));
        }

        [Fact]
        public void TestGetAsync_SurveyHasSample_ReturnsSample()
        {
            const string sample = "a sample";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(sample);
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "Surveys/" + SurveyId + "/Sample"))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldSurveySampleService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.GetAsync(SurveyId).Result;

            Assert.Equal(sample, actual);
        }

        #endregion

        #region SendInvitationsAsync

        [Fact]
        public void TestPostAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.PostAsync(null, null)));
        }

        [Fact]
        public void TestPostAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.PostAsync("  ", null)));
        }

        [Fact]
        public void TestPostAsync_SampleIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.PostAsync(SurveyId, null)));
        }

        [Fact]
        public void TestPostAsync_SampleIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.PostAsync(SurveyId, "  ")));
        }

        [Fact]
        public void TestPostAsync_ServerAccepts_ReturnsStatusMessage()
        {
            const string sample = "a sample";
            var uploadStatus = new SampleUploadStatus
            {
                DuplicateKeyCount = 1,
                EmptyKeyCount = 2,
                HeaderDataMismatch = true,
                HeaderInvalid = true,
                HeaderInvalidColumnsCount = 3,
                InsertedCount = 4,
                InvalidDataCount = 5,
                InvalidKeyCount = 6,
                ProcessingStatus = "good status",
                SkippedCount = 7,
                TotalRecordCount = 8,
                UpdatedCount = 9
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient
                .Setup(client => client.PostAsync($"{ServiceAddress}Surveys/{SurveyId}/Sample", It.Is<StringContent>(stringContent => stringContent.ReadAsStringAsync().Result.Equals(sample))))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(uploadStatus))));

            var target = new NfieldSurveySampleService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            var actual = target.PostAsync(SurveyId, sample).Result;

            Assert.Equal(uploadStatus.DuplicateKeyCount, actual.DuplicateKeyCount);
            Assert.Equal(uploadStatus.EmptyKeyCount, actual.EmptyKeyCount);
            Assert.Equal(uploadStatus.HeaderDataMismatch, actual.HeaderDataMismatch);
            Assert.Equal(uploadStatus.HeaderInvalid, actual.HeaderInvalid);
            Assert.Equal(uploadStatus.HeaderInvalidColumnsCount, actual.HeaderInvalidColumnsCount);
            Assert.Equal(uploadStatus.InsertedCount, actual.InsertedCount);
            Assert.Equal(uploadStatus.InvalidDataCount, actual.InvalidDataCount);
            Assert.Equal(uploadStatus.InvalidKeyCount, actual.InvalidKeyCount);
            Assert.Equal(uploadStatus.ProcessingStatus, actual.ProcessingStatus);
            Assert.Equal(uploadStatus.SkippedCount, actual.SkippedCount);
            Assert.Equal(uploadStatus.TotalRecordCount, actual.TotalRecordCount);
            Assert.Equal(uploadStatus.UpdatedCount, actual.UpdatedCount);
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public void TestDeleteAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.PostAsync(null, null)));
        }

        [Fact]
        public void TestDeleteAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.PostAsync("  ", null)));
        }

        [Fact]
        public void TestDeleteAsync_SampleRecordIdIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.PostAsync(SurveyId, null)));
        }

        [Fact]
        public void TestDeleteAsync_SampleRecordIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.PostAsync(SurveyId, "  ")));
        }

        [Fact]
        public void TestDeleteAsync_SampleExists_ReturnsDeletedCount()
        {
            const string respondentKey = "a sample record id";
            const int deletedCount = 887;

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient
                .Setup(client => client.DeleteAsJsonAsync($"{ServiceAddress}Surveys/{SurveyId}/Sample",
                    It.Is<IEnumerable<SampleFilter>>(
                        filters => FilterEquals(filters.Single(), "RespondentKey", "eq", respondentKey))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new SampleDeleteStatus {DeletedCount = deletedCount}))));

            var target = new NfieldSurveySampleService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            var actual = target.DeleteAsync(SurveyId, respondentKey).Result;

            Assert.Equal(deletedCount, actual);
        }

        #endregion

        #region BlockAsync

        [Fact]
        public void TestBlockAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.BlockAsync(null, "anything")));
        }

        [Fact]
        public void TestBlockAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.BlockAsync(string.Empty, "anything")));
        }

        [Fact]
        public void TestBlockAsync_SurveyIdIsWhitespace_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.BlockAsync("  ", "anything")));
        }

        [Fact]
        public void TestBlockAsync_RespondentKeyIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.BlockAsync(SurveyId, null)));
        }

        [Fact]
        public void TestBlockAsync_RespondentKeyIsWhiteSpace_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.BlockAsync(SurveyId, "   ")));
        }

        [Fact]
        public void TestBlockAsync_RespondentKeyIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.BlockAsync(SurveyId, string.Empty)));
        }

        [Fact]
        public void TestBlockAsync_ParamsAreOk_Successful()
        {
            const string respondentKey = "testRespondent123";

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.PutAsJsonAsync($"{ServiceAddress}Surveys/{SurveyId}/Sample",
                                It.Is<IEnumerable<SampleFilter>>(filters => 
                                    FilterEquals(filters.Single(), "RespondentKey", "eq", respondentKey))))
                            .Returns(CreateTask(HttpStatusCode.OK,
                                new StringContent(
                                    JsonConvert.SerializeObject(new SampleBlockStatus { BlockedCount = 1 }))));

            var target = new NfieldSurveySampleService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            var result = target.BlockAsync(SurveyId, respondentKey).Result;

            Assert.Equal(1, result);
        }

        [Fact]
        public void TestBlockAsync_NotExistingRespondent_Successful()
        {
            const string respondentKey = "not-a-respondent";

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.PutAsJsonAsync($"{ServiceAddress}Surveys/{SurveyId}/Sample",
                    It.Is<IEnumerable<SampleFilter>>(filters =>
                        FilterEquals(filters.Single(), "RespondentKey", "eq", respondentKey))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new SampleBlockStatus { BlockedCount = 0 }))));

            var target = new NfieldSurveySampleService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            var result = target.BlockAsync(SurveyId, respondentKey).Result;

            Assert.Equal(0, result);
        }

        #endregion

        private static bool FilterEquals(SampleFilter filter, string name, string op, string value)
        {
            return filter.Name.Equals(name)
                   && filter.Op.Equals(op)
                   && filter.Value.Equals(value);
        }
    }
}

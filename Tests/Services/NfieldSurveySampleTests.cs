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
    public class NfieldSurveySampleTests : NfieldServiceTestsBase
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
                UnwrapAggregateException(target.DeleteAsync(null, null)));
        }

        [Fact]
        public void TestDeleteAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.DeleteAsync("  ", null)));
        }

        [Fact]
        public void TestDeleteAsync_RespondentKeyIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.DeleteAsync(SurveyId, null)));
        }

        [Fact]
        public void TestDeleteAsync_RespondentKeyIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.DeleteAsync(SurveyId, "  ")));
        }

        [Fact]
        public void TestDeleteAsync_ParamsAreOk_Successful()
        {
            const string respondentKey = "a sample record id";

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.DeleteAsJsonAsync($"{ServiceAddress}Surveys/{SurveyId}/Sample",
                    It.Is<IEnumerable<SampleFilter>>(filters =>
                        FilterEquals(filters.Single(), "RespondentKey", "eq", respondentKey))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));
            mockedHttpClient.Setup(client => client.GetAsync($"{ServiceAddress}BackgroundActivities/activity1"))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, DeletedTotal = 1 }))));

            var target = new NfieldSurveySampleService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            var result = target.DeleteAsync(SurveyId, respondentKey).Result;

            Assert.Equal(1, result);
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

            mockedHttpClient.Setup(client => client.PutAsJsonAsync($"{ServiceAddress}Surveys/{SurveyId}/Sample/Block",
                                It.Is<IEnumerable<SampleFilter>>(filters =>
                                    FilterEquals(filters.Single(), "RespondentKey", "eq", respondentKey))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));
            mockedHttpClient.Setup(client => client.GetAsync($"{ServiceAddress}BackgroundActivities/activity1"))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, BlockedTotal = 1 }))));

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

            mockedHttpClient.Setup(client => client.PutAsJsonAsync($"{ServiceAddress}Surveys/{SurveyId}/Sample/Block",
                    It.Is<IEnumerable<SampleFilter>>(filters =>
                        FilterEquals(filters.Single(), "RespondentKey", "eq", respondentKey))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));
            mockedHttpClient.Setup(client => client.GetAsync($"{ServiceAddress}BackgroundActivities/activity1"))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, BlockedTotal = 0 }))));

            var target = new NfieldSurveySampleService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            var result = target.BlockAsync(SurveyId, respondentKey).Result;

            Assert.Equal(0, result);
        }

        #endregion

        #region ResetAsync

        [Fact]
        public void TestResetAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.ResetAsync(null, "anything")));
        }

        [Fact]
        public void TestResetAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.ResetAsync(string.Empty, "anything")));
        }

        [Fact]
        public void TestResetAsync_SurveyIdIsWhitespace_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.ResetAsync("  ", "anything")));
        }

        [Fact]
        public void TestResetAsync_RespondentKeyIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.ResetAsync(SurveyId, null)));
        }

        [Fact]
        public void TestResetAsync_RespondentKeyIsWhiteSpace_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.ResetAsync(SurveyId, "   ")));
        }

        [Fact]
        public void TestResetAsync_RespondentKeyIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.ResetAsync(SurveyId, string.Empty)));
        }

        [Fact]
        public void TestResetAsync_ParamsAreOk_Successful()
        {
            const string respondentKey = "testRespondent123";

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.PutAsJsonAsync($"{ServiceAddress}Surveys/{SurveyId}/Sample/Reset",
                    It.Is<IEnumerable<SampleFilter>>(filters =>
                        FilterEquals(filters.Single(), "RespondentKey", "eq", respondentKey))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));
            mockedHttpClient.Setup(client => client.GetAsync($"{ServiceAddress}BackgroundActivities/activity1"))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, ResetTotal = 1 }))));

            var target = new NfieldSurveySampleService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            var result = target.ResetAsync(SurveyId, respondentKey).Result;

            Assert.Equal(1, result);
        }

        #endregion

        #region ClearAsync

        [Fact]
        public void TestClearAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.ClearAsync(null, "anything", null)));
        }

        [Fact]
        public void TestClearAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.ClearAsync(string.Empty, "anything", null)));
        }

        [Fact]
        public void TestClearAsync_SurveyIdIsWhitespace_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.ClearAsync("  ", "anything", null)));
        }

        [Fact]
        public void TestClearAsync_RespondentKeyIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.ClearAsync(SurveyId, null, null)));
        }

        [Fact]
        public void TestClearAsync_RespondentKeyIsWhiteSpace_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.ClearAsync(SurveyId, "   ", null)));
        }

        [Fact]
        public void TestClearAsync_RespondentKeyIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.ClearAsync(SurveyId, string.Empty, null)));
        }

        [Fact]
        public void TestClearAsync_ColumnsAreNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.ClearAsync(SurveyId, "anything", null)));
        }

        [Fact]
        public void TestClearAsync_ColumnsAreEmptyList_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.ClearAsync(SurveyId, "anything", new List<string>())));
        }

        [Fact]
        public void TestClearAsync_ParamsAreOk_Successful()
        {
            const string respondentKey = "testRespondent123";
            var columns = new List<string> { "ColumnName1", "ColumnName2" };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.PutAsJsonAsync($"{ServiceAddress}Surveys/{SurveyId}/Sample/Clear",
                    It.Is<ClearSurveySampleModel>(c =>
                        FilterEquals(c.Filters.Single(), "RespondentKey", "eq", respondentKey) && c.Columns.Any(n => n == "ColumnName1") && c.Columns.Any(n => n == "ColumnName2"))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));

            mockedHttpClient.Setup(client => client.GetAsync($"{ServiceAddress}BackgroundActivities/activity1"))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, ClearTotal = 1 }))));

            var target = new NfieldSurveySampleService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            var result = target.ClearAsync(SurveyId, respondentKey, columns).Result;

            Assert.Equal(1, result);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.UpdateAsync(null, 0, null)));
        }

        [Fact]
        public void TestUpdateAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.UpdateAsync(string.Empty, 0, null)));
        }

        [Fact]
        public void TestUpdateAsync_SurveyIdIsWhitespace_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.UpdateAsync("  ", 0, null)));
        }

        [Fact]
        public void TestUpdateAsync_ColumnsAreNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.UpdateAsync(SurveyId, 0, null)));
        }

        [Fact]
        public void TestUpdateAsync_ColumnsAreEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.UpdateAsync(SurveyId, 0, new List<SampleColumnUpdate>())));
        }

        [Fact]
        public void TestUpdateAsync_ParamsAreOk_Successful()
        {
            int sampleRecordId = 1;
            var columns = new List<SampleColumnUpdate>();
            columns.Add(new SampleColumnUpdate { ColumnName = "dummyCol", Value = "dummyVal" });

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.PutAsJsonAsync($"{ServiceAddress}Surveys/{SurveyId}/Sample/Update",
                It.Is<SurveyUpdateSampleRecordModel>(c => c.SampleRecordId == sampleRecordId && c.ColumnUpdates.Any(n => n.ColumnName == "dummyCol"))))
                    .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(new SampleUpdateStatus { ResultStatus = true }))));

            var target = new NfieldSurveySampleService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            var result = target.UpdateAsync(SurveyId, sampleRecordId, columns).Result;

            Assert.True(result);
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

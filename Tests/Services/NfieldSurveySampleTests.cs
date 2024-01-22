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
using Nfield.SDK.Models;
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
        private const string RespondentKey = "testRespondent123";
        private const int InterviewId = 1;
        private readonly List<string> _columnsToClear = new List<string> { "ColumnName1", "ColumnName2" };

        private readonly NfieldSurveySampleService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;

        public NfieldSurveySampleTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldSurveySampleService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);
        }

        #region GetAsync

        [Fact]
        public void TestGetAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.GetAsync(null)));
        }

        [Fact]
        public void TestGetAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.GetAsync("  ")));
        }

        [Fact]
        public void TestGetAsync_SurveyHasSample_ReturnsSample()
        {
            const string sample = "a sample";
            var content = new StringContent(sample);
            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/" + SurveyId + "/Sample/")))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var actual = _target.GetAsync(SurveyId).Result;

            Assert.Equal(sample, actual);
        }

        #endregion

        #region GetSingleSampleRecordAsync

        [Fact]
        public void TestGetSingleSampleRecordAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.GetSingleSampleRecordAsync(null, 0)));
        }

        [Fact]
        public void TestGetSingleSampleRecordAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.GetSingleSampleRecordAsync("  ", 0)));
        }

        [Fact]
        public void TestGetSingleSampleRecordAsync_SurveyHasSample_ReturnsSample()
        {
            const string sample = "a sample";
            var content = new StringContent(sample);
            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/" + SurveyId + "/Sample/1")))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var actual = _target.GetSingleSampleRecordAsync(SurveyId, 1).Result;

            Assert.Equal(sample, actual);
        }

        #endregion

        #region SendInvitationsAsync

        [Fact]
        public void TestPostAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.PostAsync(null, null)));
        }

        [Fact]
        public void TestPostAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.PostAsync("  ", null)));
        }

        [Fact]
        public void TestPostAsync_SampleIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.PostAsync(SurveyId, null)));
        }

        [Fact]
        public void TestPostAsync_SampleIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.PostAsync(SurveyId, "  ")));
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

            _mockedHttpClient
                .Setup(client => client.PostAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/Sample/"), It.Is<StringContent>(stringContent => stringContent.ReadAsStringAsync().Result.Equals(sample))))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(uploadStatus))));

            var actual = _target.PostAsync(SurveyId, sample).Result;

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
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.DeleteAsync(null, null)));
        }

        [Fact]
        public void TestDeleteAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.DeleteAsync("  ", null)));
        }

        [Fact]
        public void TestDeleteAsync_RespondentKeyIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.DeleteAsync(SurveyId, null)));
        }

        [Fact]
        public void TestDeleteAsync_RespondentKeyIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.DeleteAsync(SurveyId, "  ")));
        }

        [Fact]
        public void TestDeleteAsync_ParamsAreOk_Successful()
        {
            const string respondentKey = "a sample record id";

            _mockedHttpClient.Setup(client => client.DeleteAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/Sample/"),
                    It.Is<IEnumerable<SampleFilter>>(filters =>
                        FilterEquals(filters.Single(), "RespondentKey", "eq", respondentKey))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));
            _mockedHttpClient.Setup(client => client.GetAsync(new Uri(ServiceAddress, $"BackgroundActivities/activity1")))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, DeletedTotal = 1 }))));

            var result = _target.DeleteAsync(SurveyId, respondentKey).Result;

            Assert.Equal(1, result);
        }

        [Fact]
        public void TestDeleteWithFiltersAsync_ParamsAreOk_Successful()
        {
            const string respondentKey = "a sample record id";

            var filters = new List<SampleFilter>()
            {
                new SampleFilter()
                {
                    Name = "RespondentKey",
                    Op = "eq",
                    Value = respondentKey
                }
            };

            _mockedHttpClient.Setup(client => client.DeleteAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/Sample/"),
                    It.Is<IEnumerable<SampleFilter>>(fil =>
                        FilterEquals(fil.Single(), filters.Select(x => x.Name).FirstOrDefault(), filters.Select(x => x.Op).FirstOrDefault(), respondentKey))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));
            _mockedHttpClient.Setup(client => client.GetAsync(new Uri(ServiceAddress, $"BackgroundActivities/activity1")))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, DeletedTotal = 1 }))));

            var result = _target.DeleteWithFiltersAsync(SurveyId, filters).Result;

            Assert.Equal(1, result);
        }


        #endregion

        #region BlockAsync

        [Fact]
        public void TestBlockAsync_SurveyIdIsNull_Throws()
        {

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.BlockAsync(null, "anything")));
        }

        [Fact]
        public void TestBlockAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.BlockAsync(string.Empty, "anything")));
        }

        [Fact]
        public void TestBlockAsync_SurveyIdIsWhitespace_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.BlockAsync("  ", "anything")));
        }

        [Fact]
        public void TestBlockAsync_RespondentKeyIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.BlockAsync(SurveyId, null)));
        }

        [Fact]
        public void TestBlockAsync_RespondentKeyIsWhiteSpace_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.BlockAsync(SurveyId, "   ")));
        }

        [Fact]
        public void TestBlockAsync_RespondentKeyIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.BlockAsync(SurveyId, string.Empty)));
        }

        [Fact]
        public void TestBlockAsync_ParamsAreOk_Successful()
        {
            const string respondentKey = "testRespondent123";

            _mockedHttpClient.Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/Sample/Block"),
                                It.Is<IEnumerable<SampleFilter>>(filters =>
                                    FilterEquals(filters.Single(), "RespondentKey", "eq", respondentKey))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));
            _mockedHttpClient.Setup(client => client.GetAsync(new Uri(ServiceAddress, "BackgroundActivities/activity1")))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, BlockedTotal = 1 }))));

            var result = _target.BlockAsync(SurveyId, respondentKey).Result;

            Assert.Equal(1, result);
        }

        [Fact]
        public void TestBlockWithFiltersAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.BlockWithFiltersAsync(null, new List<SampleFilter>() { new SampleFilter() { Name = "" } })));
        }

        [Fact]
        public void TestBlockWithFiltersAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.BlockWithFiltersAsync(string.Empty, new List<SampleFilter>() { new SampleFilter() { Name = "" } })));
        }

        [Fact]
        public void TestBlockWithFiltersAsync_SurveyIdIsWhitespace_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.BlockWithFiltersAsync("  ", new List<SampleFilter>() { new SampleFilter() { Name = "" } })));
        }

        [Fact]
        public void TestBlockWithFiltersAsync_FiltersAreNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.BlockWithFiltersAsync(SurveyId, null)));
        }

        [Fact]
        public void TestBlockWithFiltersAsync_FiltersAreEmpty_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.BlockWithFiltersAsync(SurveyId, new List<SampleFilter>())));
        }

        [Fact]
        public void TestBlockWithFiltersAsync_ParamsAreOk_Successful()
        {
            const string respondentKey = "testRespondent123";

            var filters = new List<SampleFilter>()
              {
                  new SampleFilter()
                  {
                      Name = "RespondentKey",
                      Op = "eq",
                      Value = respondentKey
                  }
              };

            _mockedHttpClient.Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/Sample/Block"),
                                It.Is<IEnumerable<SampleFilter>>((fil =>
              FilterEquals(fil.Single(), filters.Select(x => x.Name).FirstOrDefault(), filters.Select(x => x.Op).FirstOrDefault(), respondentKey)))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));
            _mockedHttpClient.Setup(client => client.GetAsync(new Uri(ServiceAddress, "BackgroundActivities/activity1")))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, BlockedTotal = 1 }))));

            var result = _target.BlockWithFiltersAsync(SurveyId, filters).Result;

            Assert.Equal(1, result);
        }


        [Fact]
        public void TestBlockAsync_NotExistingRespondent_Successful()
        {
            const string respondentKey = "not-a-respondent";

            _mockedHttpClient.Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/Sample/Block"),
                    It.Is<IEnumerable<SampleFilter>>(filters =>
                        FilterEquals(filters.Single(), "RespondentKey", "eq", respondentKey))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));
            _mockedHttpClient.Setup(client => client.GetAsync(new Uri(ServiceAddress, $"BackgroundActivities/activity1")))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, BlockedTotal = 0 }))));

            var result = _target.BlockAsync(SurveyId, respondentKey).Result;

            Assert.Equal(0, result);
        }

        #endregion

        #region ResetAsync

        [Fact]
        public void TestResetAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ResetAsync(null, "anything")));
        }

        [Fact]
        public void TestResetAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.ResetAsync(string.Empty, "anything")));
        }

        [Fact]
        public void TestResetAsync_SurveyIdIsWhitespace_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.ResetAsync("  ", "anything")));
        }

        [Fact]
        public void TestResetAsync_RespondentKeyIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ResetAsync(SurveyId, null)));
        }

        [Fact]
        public void TestResetAsync_RespondentKeyIsWhiteSpace_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.ResetAsync(SurveyId, "   ")));
        }

        [Fact]
        public void TestResetAsync_RespondentKeyIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.ResetAsync(SurveyId, string.Empty)));
        }

        [Fact]
        public void TestResetAsync_ParamsAreOk_Successful()
        {
            const string respondentKey = "testRespondent123";

            _mockedHttpClient.Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/Sample/Reset"),
                    It.Is<IEnumerable<SampleFilter>>(filters =>
                        FilterEquals(filters.Single(), "RespondentKey", "eq", respondentKey))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));
            _mockedHttpClient.Setup(client => client.GetAsync(new Uri(ServiceAddress, $"BackgroundActivities/activity1")))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, ResetTotal = 1 }))));

            var result = _target.ResetAsync(SurveyId, respondentKey).Result;

            Assert.Equal(1, result);
        }

        [Fact]
        public void TestResetWithFiltersAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ResetWithFiltersAsync(null, new List<SampleFilter>() { new SampleFilter() { Name = "" } })));
        }

        [Fact]
        public void TestResetWithFiltersAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.ResetWithFiltersAsync(string.Empty, new List<SampleFilter>() { new SampleFilter() { Name = "" } })));
        }

        [Fact]
        public void TestResetWithFiltersAsync_SurveyIdIsWhitespace_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.ResetWithFiltersAsync("  ", new List<SampleFilter>() { new SampleFilter() { Name = "" } })));
        }

        [Fact]
        public void TestResetWithFiltersAsync_FiltersAreNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ResetWithFiltersAsync(SurveyId, null)));
        }

        [Fact]
        public void TestResetWithFiltersAsync_FiltersAreEmpty_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ResetWithFiltersAsync(SurveyId, new List<SampleFilter>())));
        }


        [Fact]
        public void TestResetWithFiltersAsync_ParamsAreOk_Successful()
        {
            const string respondentKey = "testRespondent123";

            var filters = new List<SampleFilter>()
              {
                  new SampleFilter()
                  {
                      Name = "RespondentKey",
                      Op = "eq",
                      Value = respondentKey
                  }
              };

            _mockedHttpClient.Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/Sample/Reset"),
                    It.Is<IEnumerable<SampleFilter>>(fil =>
              FilterEquals(fil.Single(), filters.Select(x => x.Name).FirstOrDefault(), filters.Select(x => x.Op).FirstOrDefault(), respondentKey))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));
            _mockedHttpClient.Setup(client => client.GetAsync(new Uri(ServiceAddress, $"BackgroundActivities/activity1")))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, ResetTotal = 1 }))));

            var result = _target.ResetWithFiltersAsync(SurveyId, filters).Result;

            Assert.Equal(1, result);
        }

        #endregion

        #region ClearByRespondentAsync

        [Fact]
        public void TestClearByRespondentAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ClearByRespondentAsync(null, RespondentKey, _columnsToClear)));
        }

        [Fact]
        public void TestClearByRespondentAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.ClearByRespondentAsync(string.Empty, RespondentKey, _columnsToClear)));
        }

        [Fact]
        public void TestClearByRespondentAsync_SurveyIdIsWhitespace_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.ClearByRespondentAsync("  ", RespondentKey, _columnsToClear)));
        }

        [Fact]
        public void TestClearByRespondentAsync_RespondentKeyIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ClearByRespondentAsync(SurveyId, null, _columnsToClear)));
        }

        [Fact]
        public void TestClearByRespondentAsync_RespondentKeyIsWhiteSpace_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.ClearByRespondentAsync(SurveyId, "   ", _columnsToClear)));
        }

        [Fact]
        public void TestClearByRespondentAsync_RespondentKeyIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.ClearByRespondentAsync(SurveyId, string.Empty, _columnsToClear)));
        }

        [Fact]
        public void TestClearByRespondentAsync_ColumnsAreNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ClearByRespondentAsync(SurveyId, RespondentKey, null)));
        }

        [Fact]
        public void TestClearByRespondentAsync_ColumnsAreEmptyList_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ClearByRespondentAsync(SurveyId, RespondentKey, new List<string>())));
        }

        [Fact]
        public void TestClearByRespondentAsync_ParamsAreOk_Successful()
        {
            _mockedHttpClient.Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/Sample/Clear"),
                    It.Is<ClearSurveySampleModel>(c =>
                        FilterEquals(c.Filters.Single(), "RespondentKey", "eq", RespondentKey) && c.Columns.Any(n => n == "ColumnName1") && c.Columns.Any(n => n == "ColumnName2"))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));

            _mockedHttpClient.Setup(client => client.GetAsync(new Uri(ServiceAddress, $"BackgroundActivities/activity1")))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, ClearTotal = 1 }))));

            var result = _target.ClearByRespondentAsync(SurveyId, RespondentKey, _columnsToClear).Result;

            Assert.Equal(1, result);
        }

        #endregion


        #region ClearByInterviewAsync

        [Fact]
        public void TestClearByInterviewAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ClearByInterviewAsync(null, InterviewId, _columnsToClear)));
        }

        [Fact]
        public void TestClearByInterviewAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.ClearByInterviewAsync(string.Empty, InterviewId, _columnsToClear)));
        }

        [Fact]
        public void TestClearByInterviewAsync_SurveyIdIsWhitespace_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.ClearByInterviewAsync("  ", InterviewId, _columnsToClear)));
        }

        [Fact]
        public void TestClearByInterviewAsync_ColumnsAreNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ClearByInterviewAsync(SurveyId, InterviewId, null)));
        }

        [Fact]
        public void TestClearByInterviewAsync_ColumnsAreEmptyList_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ClearByInterviewAsync(SurveyId, InterviewId, new List<string>())));
        }

        [Fact]
        public void TestClearByInterviewAsync_ParamsAreOk_Successful()
        {
            _mockedHttpClient.Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/Sample/Clear"),
                    It.Is<ClearSurveySampleModel>(c =>
                        FilterEquals(c.Filters.Single(), "InterviewId", "eq", InterviewId.ToString()) && c.Columns.Any(n => n == "ColumnName1") && c.Columns.Any(n => n == "ColumnName2"))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));

            _mockedHttpClient.Setup(client => client.GetAsync(new Uri(ServiceAddress, $"BackgroundActivities/activity1")))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, ClearTotal = 1 }))));

            var result = _target.ClearByInterviewAsync(SurveyId, InterviewId, _columnsToClear).Result;

            Assert.Equal(1, result);
        }

        [Fact]
        public void TestClearByFiltersAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ClearByFiltersAsync(null, new List<SampleFilter>() { new SampleFilter() { Name = "" } }, new List<string>() { string.Empty })));
        }

        [Fact]
        public void TestClearByFiltersAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.ClearByFiltersAsync(string.Empty, new List<SampleFilter>() { new SampleFilter() { Name = "" } }, new List<string>() { string.Empty })));
        }

        [Fact]
        public void TestClearByFiltersAsync_SurveyIdIsWhitespace_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.ClearByFiltersAsync(" ", new List<SampleFilter>() { new SampleFilter() { Name = "" } }, new List<string>() { string.Empty })));
        }

        [Fact]
        public void TestClearByFiltersAsync_FiltersAreNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ClearByFiltersAsync(SurveyId, null, new List<string>() { string.Empty })));
        }

        [Fact]
        public void TestClearByFiltersAsync_FiltersAreEmpty_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ClearByFiltersAsync(SurveyId, new List<SampleFilter>(), new List<string>() { string.Empty })));
        }

        [Fact]
        public void TestClearByFiltersAsync_ColumnsAreNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ClearByFiltersAsync(SurveyId, new List<SampleFilter>() { new SampleFilter() { Name = "" } }, null)));
        }

        [Fact]
        public void TestClearByFiltersAsync_ColumnsAreEmpty_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.ClearByFiltersAsync(SurveyId, new List<SampleFilter>(), new List<string>())));
        }


        [Fact]
        public void TestClearWithFiltersAsync_ParamsAreOk_Successful()
        {
            var filters = new List<SampleFilter>()
              {
                  new SampleFilter()
                  {
                      Name = "InterviewId",
                      Op = "eq",
                      Value = InterviewId.ToString()
                  }
              };

            _mockedHttpClient.Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/Sample/Clear"),
                    It.Is<ClearSurveySampleModel>(c =>
                        FilterEquals(c.Filters.Single(), filters.Select(x => x.Name).FirstOrDefault(), filters.Select(x => x.Op).FirstOrDefault(), InterviewId.ToString())
                        && c.Columns.Any(n => n == "ColumnName1") && c.Columns.Any(n => n == "ColumnName2"))))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new BackgroundActivityStatus { ActivityId = "activity1" }))));

            _mockedHttpClient.Setup(client => client.GetAsync(new Uri(ServiceAddress, $"BackgroundActivities/activity1")))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(
                        JsonConvert.SerializeObject(new { Status = 2, ClearTotal = 1 }))));

            var result = _target.ClearByFiltersAsync(SurveyId, filters, _columnsToClear).Result;

            Assert.Equal(1, result);
        }

        #endregion
        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.UpdateAsync(null, 0, null)));
        }

        [Fact]
        public void TestUpdateAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.UpdateAsync(string.Empty, 0, null)));
        }

        [Fact]
        public void TestUpdateAsync_SurveyIdIsWhitespace_Throws()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.UpdateAsync("  ", 0, null)));
        }

        [Fact]
        public void TestUpdateAsync_ColumnsAreNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.UpdateAsync(SurveyId, 0, null)));
        }

        [Fact]
        public void TestUpdateAsync_ColumnsAreEmpty_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.UpdateAsync(SurveyId, 0, new List<SampleColumnUpdate>())));
        }

        [Fact]
        public void TestUpdateAsync_ParamsAreOk_Successful()
        {
            int sampleRecordId = 1;
            var columns = new List<SampleColumnUpdate>();
            columns.Add(new SampleColumnUpdate { ColumnName = "dummyCol", Value = "dummyVal" });

            _mockedHttpClient.Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/Sample/Update"),
                It.Is<SurveyUpdateSampleRecordModel>(c => c.SampleRecordId == sampleRecordId && c.ColumnUpdates.Any(n => n.ColumnName == "dummyCol"))))
                    .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(new SampleUpdateStatus { ResultStatus = true }))));

            var result = _target.UpdateAsync(SurveyId, sampleRecordId, columns).Result;

            Assert.True(result);
        }
        #endregion


        #region CreateSampleRecord
        [Fact]
        public void TestCreateSample_Returns_Expected_Columns()
        {

            var sampleColumns = new List<SampleColumnCreate>()
            {
                new SampleColumnCreate()
                {
                    ColumnName = "TestingI",
                    Value = "ValueI"
                },
                 new SampleColumnCreate()
                {
                    ColumnName = "TestingII",
                    Value = "ValueII"
                }
            };

            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/Sample/Create"),
                It.Is<IEnumerable<SampleColumnCreate>>(x => x == sampleColumns)))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(sampleColumns))));

            var actual = _target.CreateAsync(SurveyId, sampleColumns).Result;

            foreach (var item in sampleColumns)
            {
                Assert.True(actual.Any(el => el.ColumnName == item.ColumnName));
                Assert.True(actual.Any(el => el.Value != null && el.Value.ToString() == item.Value.ToString()));
            }
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

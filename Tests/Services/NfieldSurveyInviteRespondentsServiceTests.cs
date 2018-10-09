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
    public class NfieldSurveyInviteRespondentsServiceTests : NfieldServiceTestsBase
    {
        private const string SurveyId = "MySurveyId";

        #region SendInvitationsAsync

        [Fact]
        public void TestSendInvitationsAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyInviteRespondentsService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.SendInvitationsAsync(null, null)));
        }

        [Fact]
        public void TestSendInvitationsAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveyInviteRespondentsService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.SendInvitationsAsync("  ", null)));
        }

        [Fact]
        public void TestSendInvitationsAsync_BatchDataIsNull_Throws()
        {
            var target = new NfieldSurveyInviteRespondentsService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.SendInvitationsAsync(SurveyId, null)));
        }

        [Fact]
        public void TestSendInvitationsAsync_ServerAccepts_ReturnsCorrectInviteRespondentsStatus()
        {
            var scheduledFor = DateTime.Now.AddDays(2);
            var batch = new InvitationBatch
            {
                RespondentKeys = new List<string> { "r1", "r2" },
                EmailColumnName = "email",
                InvitationTemplateId = 2,
                Name = "FirstBatch",
                ScheduledFor = scheduledFor
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var expectedResult = new InviteRespondentsStatus
            {
                Count = 2,
                Status = "Completed",
                ErrorMessage = ""
            };

            var url = new Uri(ServiceAddress, $"Surveys/{SurveyId}/InviteRespondents/");
            mockedHttpClient.Setup(client => client
                    .PostAsJsonAsync(url, It.Is<InvitationBatchWithFilter>(b =>
                        b.Name == batch.Name &&
                        b.ScheduledFor == batch.ScheduledFor &&
                        b.EmailColumnName == batch.EmailColumnName &&
                        b.InvitationTemplateId == batch.InvitationTemplateId
                        )))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedResult))));

            var target = new NfieldSurveyInviteRespondentsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            var result = target.SendInvitationsAsync(SurveyId, batch).Result;

            // Verify the filter send
            mockedHttpClient.Verify(s => s.PostAsJsonAsync(
                url, 
                It.Is<InvitationBatchWithFilter>(b => 
                    b.Filters.Count() == 1 &&
                    FilterEquals(b.Filters.First(), "RespondentKey", "in", string.Join(",", batch.RespondentKeys)) )), 
                Times.Once());

            Assert.Equal(expectedResult.Count, result.Count);
            Assert.Equal(expectedResult.Status, result.Status);
            Assert.Equal(expectedResult.ErrorMessage, result.ErrorMessage);
        }

        private static bool FilterEquals(SampleFilter filter, string name, string op, string value)
        {
            return filter.Name.Equals(name)
                   && filter.Op.Equals(op)
                   && filter.Value.Equals(value);
        }

        #endregion

        #region GetSurveysInvitationStatusAsync

        [Fact]
        public void TestGetSurveysInvitationStatusAsync_ProvideBatchName_ReturnsData()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var target = new NfieldSurveyInviteRespondentsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var expectedResult = new
            {
                SurveyId = SurveyId,
                SurveyName = "SurveyName",
                InvitationsBlocked = true,
                LastActivity = DateTime.UtcNow,
                TotalCount = 1,
                ScheduledCount = 2,
                PendingCount = 3,
                NotSentCount = 4,
                ErrorCount = 5,
                SentCount = 6,
                OpenedCount = 7,
                ClickedCount = 8,
                UnsubscribedCount = 9,
                AbuseReportCount = 10,
                UnknownCount = 11
            };

            var url = new Uri(ServiceAddress, $"Surveys/InviteRespondents/SurveysInvitationStatus/");
            mockedHttpClient.Setup(client => client.GetAsync(url))
                            .Returns(CreateTask(HttpStatusCode.OK,
                                                new StringContent(JsonConvert.SerializeObject(new[] { expectedResult }))));

            var result = target.GetSurveysInvitationStatusAsync().Result.ToArray();

            Assert.Equal(1, result.Length);
            Assert.Equal(expectedResult.SurveyId, result[0].SurveyId);
            Assert.Equal(expectedResult.SurveyName, result[0].SurveyName);
            Assert.Equal(expectedResult.InvitationsBlocked, result[0].InvitationsBlocked);
            Assert.Equal(expectedResult.LastActivity, result[0].LastActivity);
            Assert.Equal(expectedResult.TotalCount, result[0].TotalCount);
            Assert.Equal(expectedResult.ScheduledCount, result[0].ScheduledCount);
            Assert.Equal(expectedResult.PendingCount, result[0].PendingCount);
            Assert.Equal(expectedResult.NotSentCount, result[0].NotSentCount);
            Assert.Equal(expectedResult.ErrorCount, result[0].ErrorCount);
            Assert.Equal(expectedResult.SentCount, result[0].SentCount);
            Assert.Equal(expectedResult.OpenedCount, result[0].OpenedCount);
            Assert.Equal(expectedResult.ClickedCount, result[0].ClickedCount);
            Assert.Equal(expectedResult.UnsubscribedCount, result[0].UnsubscribedCount);
            Assert.Equal(expectedResult.AbuseReportCount, result[0].AbuseReportCount);
            Assert.Equal(expectedResult.UnknownCount, result[0].UnknownCount);
        }

        #endregion

        #region GetSurveyBatchesStatusAsync

        [Fact]
        public void TestGetSurveyBatchesStatusAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyInviteRespondentsService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.GetSurveyBatchesStatusAsync(null)));
        }

        [Fact]
        public void TestGetSurveyBatchesStatusAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveyInviteRespondentsService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.GetSurveyBatchesStatusAsync(string.Empty)));
        }

        [Fact]
        public void TestGetSurveyBatchesStatusAsync_SurveyIdIsWhitespace_Throws()
        {
            var target = new NfieldSurveyInviteRespondentsService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.GetSurveyBatchesStatusAsync("   ")));
        }

        [Fact]
        public void TestGetSurveyBatchesStatusAsync_ProvideBatchName_ReturnsData()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var target = new NfieldSurveyInviteRespondentsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var expectedResult = new
            {
                SurveyId = SurveyId,
                BatchName = "BatchName",
                Status = "Status",
                ScheduledFor = DateTime.UtcNow,
                TotalCount = 1,
                ScheduledCount = 2,
                PendingCount = 3,
                NotSentCount = 4,
                ErrorCount = 5,
                SentCount = 6,
                OpenedCount = 7,
                ClickedCount = 8,
                UnsubscribedCount = 9,
                AbuseReportCount = 10,
                UnknownCount = 11
            };

            var url = new Uri(ServiceAddress, $"Surveys/{SurveyId}/InviteRespondents/SurveyBatchesStatus/");
            mockedHttpClient.Setup(client => client.GetAsync(url))
                            .Returns(CreateTask(HttpStatusCode.OK,
                                                new StringContent(JsonConvert.SerializeObject(new[] { expectedResult }))));

            var result = target.GetSurveyBatchesStatusAsync(SurveyId).Result.ToArray();

            Assert.Equal(1, result.Length);
            Assert.Equal(expectedResult.SurveyId, result[0].SurveyId);
            Assert.Equal(expectedResult.BatchName, result[0].BatchName);
            Assert.Equal(expectedResult.Status, result[0].Status);
            Assert.Equal(expectedResult.ScheduledFor, result[0].ScheduledFor);
            Assert.Equal(expectedResult.TotalCount, result[0].TotalCount);
            Assert.Equal(expectedResult.ScheduledCount, result[0].ScheduledCount);
            Assert.Equal(expectedResult.PendingCount, result[0].PendingCount);
            Assert.Equal(expectedResult.NotSentCount, result[0].NotSentCount);
            Assert.Equal(expectedResult.ErrorCount, result[0].ErrorCount);
            Assert.Equal(expectedResult.SentCount, result[0].SentCount);
            Assert.Equal(expectedResult.OpenedCount, result[0].OpenedCount);
            Assert.Equal(expectedResult.ClickedCount, result[0].ClickedCount);
            Assert.Equal(expectedResult.UnsubscribedCount, result[0].UnsubscribedCount);
            Assert.Equal(expectedResult.AbuseReportCount, result[0].AbuseReportCount);
            Assert.Equal(expectedResult.UnknownCount, result[0].UnknownCount);
        }

        #endregion

        #region GetInvitationStatusAsync

        [Fact]
        public void TestGetInvitationStatusAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyInviteRespondentsService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.GetInvitationStatusAsync(null, null)));
        }

        [Fact]
        public void TestGetInvitationStatusAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveyInviteRespondentsService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.GetInvitationStatusAsync(string.Empty, null)));
        }

        [Fact]
        public void TestGetInvitationStatusAsync_SurveyIdIsWhitespace_Throws()
        {
            var target = new NfieldSurveyInviteRespondentsService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.GetInvitationStatusAsync("   ", null)));
        }

        [Fact]
        public void TestGetInvitationStatusAsync_BatchDataIsNull_Throws()
        {
            var target = new NfieldSurveyInviteRespondentsService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.GetInvitationStatusAsync(SurveyId, null)));
        }

        [Fact]
        public void TestGetInvitationStatusAsync_ProvideBatchName_ReturnsData()
        {
            const string batchName = "TestBatch";
            const string respondentKey = "TestRespondent";
            const string expectedStatus = "Test";

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var target = new NfieldSurveyInviteRespondentsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var expectedResult = new
            {
                RespondentKey = respondentKey,
                Status = expectedStatus
            };

            var url = new Uri(ServiceAddress, $"Surveys/{SurveyId}/InviteRespondents/InvitationStatus/{batchName}");
            mockedHttpClient.Setup(client => client.GetAsync(url))
                            .Returns(CreateTask(HttpStatusCode.OK, 
                                                new StringContent(JsonConvert.SerializeObject(new[] {expectedResult}))));

            var result = target.GetInvitationStatusAsync(SurveyId, batchName).Result.ToArray();

            Assert.Equal(1, result.Length);
            Assert.Equal(respondentKey, result[0].RespondentKey);
            Assert.Equal(expectedStatus, result[0].Status);
        }

        #endregion

    }
}

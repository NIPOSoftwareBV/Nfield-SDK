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
        public void TestPostAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyInviteRespondentsService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.SendInvitationsAsync(null, null)));
        }

        [Fact]
        public void TestPostAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveyInviteRespondentsService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.SendInvitationsAsync("  ", null)));
        }

        [Fact]
        public void TestPostAsync_BatchDataIsNull_Throws()
        {
            var target = new NfieldSurveyInviteRespondentsService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.SendInvitationsAsync(SurveyId, null)));
        }

        [Fact]
        public void TestPostAsync_BatchDataBothRespondentKeysAndFilter_Throws()
        {
            var target = new NfieldSurveyInviteRespondentsService();
            var batch = new InvitationBatch()
            {
                RespondentKeys = new List<string>() { "r1", "r2" },
                EmailColumnName = "email",
                InvitationTemplateId = 2,
                Name = "FirstBatch",
                ScheduledFor = DateTime.Now.AddDays(2),
                Filters = new List<SampleFilter>()
                {
                    new SampleFilter(){Name = "RespondentKeys", Op = "eq", Value = "r5"}
                }
            };

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.SendInvitationsAsync(SurveyId, batch)));
        }

        [Fact]
        public void TestPostAsync_ServerAccepts_ReturnsCorrectInviteRespondentsStatus()
        {
            var scheduledFor = DateTime.Now.AddDays(2);
            var batch = new InvitationBatch()
            {
                RespondentKeys = new List<string>() { "r1", "r2" },
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

            var url = $"{ServiceAddress}Surveys/{SurveyId}/InviteRespondents";
            mockedHttpClient.Setup(client => client
                    .PostAsJsonAsync(url, It.Is<InvitationBatch>(b =>
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
                It.Is<InvitationBatch>(b => 
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


    }
}

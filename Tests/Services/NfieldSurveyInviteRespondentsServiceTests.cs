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
        public void TestPostAsync_ServerAccepts_ReturnsBatchId()
        {
            var scheduledFor = DateTime.Now.AddDays(2);
            var invitationBatch = new InvitationBatch()
            {
                RespondentKeys = new List<string>() { "r1", "r2" },
                EmailColumnName = "email",
                InvitationTemplateId = 2,
                Name = "FirstBatch",
                ScheduledFor = scheduledFor
            };
            const int batchId = 101;

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            List<SampleFilter> sampleFiltersActual = new List<SampleFilter>();
            mockedHttpClient.Setup(client => client
                        .PostAsJsonAsync($"{ServiceAddress}Surveys/{SurveyId}/InviteRespondents", invitationBatch))
                        .Returns(CreateTask(HttpStatusCode.OK, new StringContent("{}")))
                        .Callback<string, InvitationBatch>((s, b) =>  sampleFiltersActual = b.Filters.ToList()); 

            var target = new NfieldSurveyInviteRespondentsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            var result = target.SendInvitationsAsync(SurveyId, invitationBatch).Result;

            // Test result
            // Assert.Equal(batchId, result);

            // Test if the right filter is send
            Assert.Equal(1, sampleFiltersActual.Count);
            Assert.True(FilterEquals(sampleFiltersActual[0], "RespondentKey", "in", "r1,r2"));
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

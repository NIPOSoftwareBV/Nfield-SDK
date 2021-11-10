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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldInterviewersService"/>
    /// </summary>
    public class NfieldInterviewerAssignmentsServiceTests : NfieldServiceTestsBase
    {
        private NfieldInterviewerAssignmentsService _target;
        private string _interviewerId = Guid.NewGuid().ToString();

        public NfieldInterviewerAssignmentsServiceTests()
        {
            _target = new NfieldInterviewerAssignmentsService();
        }

        #region QueryAsync

        [Fact]
        public async Task TestQueryAsync_ServerReturnsQuery_ReturnsListWithInterviewersAssignments()
        {
            var expectedInterviewersAssignments = new[]
            {
                new InterviewerAssignmentModel
                {
                    InterviewerId = _interviewerId,
                    Interviewer = "Interviewer",
                    IsActive = false,
                    IsAssigned = true,
                    Discriminator = "Discriminator",
                    IsLastSyncSuccessful = true,
                    LastSyncDate = DateTime.Now,
                    IsFullSynced = true,
                    SurveyId = "SurveyId",
                    SurveyName = "SurveyName"
                },
                new InterviewerAssignmentModel
                {
                    InterviewerId = _interviewerId,
                    AssignedTarget = 2,
                    IsGroupAssignment = false,
                    AssignedSamplingPointTarget = 8,
                    IsActive = true,
                    IsAssigned = true,
                    DroppedOutCount = 1,
                    RejectedCount = 2,
                    SuccessfulCount = 3,
                    ScreenedOutCount = 4
                }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(ApiUri(_interviewerId)))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedInterviewersAssignments))));

            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualInterviewersAssignments = (await _target.QueryAsync(_interviewerId)).ToList();
            Assert.Equal(expectedInterviewersAssignments.Length, actualInterviewersAssignments.Count());
            AssertAssignment(expectedInterviewersAssignments[0], actualInterviewersAssignments[0]);
            AssertAssignment(expectedInterviewersAssignments[1], actualInterviewersAssignments[1]);
        }

        private static void AssertAssignment(InterviewerAssignmentModel expectedInterviewerAssignment, InterviewerAssignmentModel actualInterviewerAssignment)
        {
            Assert.Equal(expectedInterviewerAssignment.InterviewerId, actualInterviewerAssignment.InterviewerId);
            Assert.Equal(expectedInterviewerAssignment.Interviewer, actualInterviewerAssignment.Interviewer);
            Assert.Equal(expectedInterviewerAssignment.IsActive, actualInterviewerAssignment.IsActive);
            Assert.Equal(expectedInterviewerAssignment.IsAssigned, actualInterviewerAssignment.IsAssigned);
            Assert.Equal(expectedInterviewerAssignment.AssignedSamplingPointTarget, actualInterviewerAssignment.AssignedSamplingPointTarget);
            Assert.Equal(expectedInterviewerAssignment.AssignedTarget, actualInterviewerAssignment.AssignedTarget);
            Assert.Equal(expectedInterviewerAssignment.Discriminator, actualInterviewerAssignment.Discriminator);
            Assert.Equal(expectedInterviewerAssignment.DroppedOutCount, actualInterviewerAssignment.DroppedOutCount);
            Assert.Equal(expectedInterviewerAssignment.IsFullSynced, actualInterviewerAssignment.IsFullSynced);
            Assert.Equal(expectedInterviewerAssignment.IsGroupAssignment, actualInterviewerAssignment.IsGroupAssignment);
            Assert.Equal(expectedInterviewerAssignment.IsLastSyncSuccessful, actualInterviewerAssignment.IsLastSyncSuccessful);
            Assert.Equal(expectedInterviewerAssignment.LastSyncDate, actualInterviewerAssignment.LastSyncDate);
            Assert.Equal(expectedInterviewerAssignment.RejectedCount, actualInterviewerAssignment.RejectedCount);
            Assert.Equal(expectedInterviewerAssignment.ScreenedOutCount, actualInterviewerAssignment.ScreenedOutCount);
            Assert.Equal(expectedInterviewerAssignment.SuccessfulCount, actualInterviewerAssignment.SuccessfulCount);
            Assert.Equal(expectedInterviewerAssignment.SurveyId, actualInterviewerAssignment.SurveyId);
            Assert.Equal(expectedInterviewerAssignment.SurveyName, actualInterviewerAssignment.SurveyName);
        }
        private Uri ApiUri(string interviewerId)
        {
            return new Uri(ServiceAddress, "interviewers/" + interviewerId + "/assignments");
        }


        #endregion

    }
}

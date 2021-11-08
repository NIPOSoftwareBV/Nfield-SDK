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

        #region PutAsync        

        [Fact]
        public void TestUpdateAsync_ModelArgumentIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.PutAsync("id", null)));
        }

        [Fact]
        public async Task TestUpdateAsync_InterviewerExists_ReturnsInterviewer()
        {
            var interviewerAssignmentModel = new InterviewerAssignmentModel
            {
                AssignmentType = "AssignmentType",
                Description = "Description",
                SamplingPointsFilter = new[] { "samplingPointId" },
                SurveyId = "SurveyId",
                TargetToDistribute = 5
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(ApiUri(_interviewerId), It.Is<InterviewerAssignmentModel>
                (ia =>
                        ia.AssignmentType == interviewerAssignmentModel.AssignmentType &&
                        ia.Description == interviewerAssignmentModel.Description &&
                        ia.SamplingPointsFilter == interviewerAssignmentModel.SamplingPointsFilter &&
                        ia.SurveyId == interviewerAssignmentModel.SurveyId &&
                        ia.TargetToDistribute == interviewerAssignmentModel.TargetToDistribute
                    )))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(interviewerAssignmentModel)))).Verifiable();

            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            await _target.PutAsync(_interviewerId, interviewerAssignmentModel);

            mockedHttpClient.Verify();
        }

        #endregion

        #region QueryAsync

        [Fact]
        public async Task TestQueryAsync_ServerReturnsQuery_ReturnsListWithInterviewersAssignments()
        {
            var expectedInterviewersAssignments = new InterviewerAssignmentDataModel[]
            {
                new InterviewerAssignmentDataModel
                {
                    InterviewerId = _interviewerId,
                    Interviewer = "Interviewer",
                    Active = false,
                    Assigned = true,
                    Discriminator = "Discriminator",
                    IsLastSyncSuccessful = true,
                    LastSyncDate = DateTime.Now,
                    IsFullSynced = true,
                    SurveyId = "SurveyId",
                    SurveyName = "SurveyName"
                },
                new InterviewerAssignmentDataModel
                {
                    InterviewerId = _interviewerId,
                    AssignedTarget = 2,
                    IsGroupAssignment = false,
                    AssignedSamplingPointTarget = 8,
                    Active = true,
                    Assigned = true,
                    DroppedOut = 1,
                    Rejected = 2,
                    Successful = 3,
                    ScreenedOut = 4
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

        private static void AssertAssignment(InterviewerAssignmentDataModel expectedInterviewerAssignment, InterviewerAssignmentDataModel actualInterviewerAssignment)
        {
            Assert.Equal(expectedInterviewerAssignment.InterviewerId, actualInterviewerAssignment.InterviewerId);
            Assert.Equal(expectedInterviewerAssignment.Interviewer, actualInterviewerAssignment.Interviewer);
            Assert.Equal(expectedInterviewerAssignment.Active, actualInterviewerAssignment.Active);
            Assert.Equal(expectedInterviewerAssignment.Assigned, actualInterviewerAssignment.Assigned);
            Assert.Equal(expectedInterviewerAssignment.AssignedSamplingPointTarget, actualInterviewerAssignment.AssignedSamplingPointTarget);
            Assert.Equal(expectedInterviewerAssignment.AssignedTarget, actualInterviewerAssignment.AssignedTarget);
            Assert.Equal(expectedInterviewerAssignment.Discriminator, actualInterviewerAssignment.Discriminator);
            Assert.Equal(expectedInterviewerAssignment.DroppedOut, actualInterviewerAssignment.DroppedOut);
            Assert.Equal(expectedInterviewerAssignment.IsFullSynced, actualInterviewerAssignment.IsFullSynced);
            Assert.Equal(expectedInterviewerAssignment.IsGroupAssignment, actualInterviewerAssignment.IsGroupAssignment);
            Assert.Equal(expectedInterviewerAssignment.IsLastSyncSuccessful, actualInterviewerAssignment.IsLastSyncSuccessful);
            Assert.Equal(expectedInterviewerAssignment.LastSyncDate, actualInterviewerAssignment.LastSyncDate);
            Assert.Equal(expectedInterviewerAssignment.Rejected, actualInterviewerAssignment.Rejected);
            Assert.Equal(expectedInterviewerAssignment.ScreenedOut, actualInterviewerAssignment.ScreenedOut);
            Assert.Equal(expectedInterviewerAssignment.Successful, actualInterviewerAssignment.Successful);
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

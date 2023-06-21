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
using System.Dynamic;
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
            var expectedInterviewersAssignments = new dynamic[]
            {
                new 
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
                new 
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
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedInterviewersAssignments)))).Verifiable();

            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualInterviewersAssignments = (await _target.QueryAsync(_interviewerId)).ToList();
            Assert.Equal(expectedInterviewersAssignments.Length, actualInterviewersAssignments.Count());
            AssertAssignment(expectedInterviewersAssignments[0], actualInterviewersAssignments[0]);
            AssertAssignment(expectedInterviewersAssignments[1], actualInterviewersAssignments[1]);
            mockedHttpClient.Verify();
        }

        private static void AssertAssignment(dynamic expectedInterviewerAssignment, InterviewerAssignmentModel actualInterviewerAssignment)
        {
           if (PropertyExist(expectedInterviewerAssignment,"IsLastSyncSuccessful")) Assert.Equal(expectedInterviewerAssignment.IsLastSyncSuccessful, actualInterviewerAssignment.IsLastSyncSuccessful);
           if (PropertyExist(expectedInterviewerAssignment,"LastSyncDate")) Assert.Equal(expectedInterviewerAssignment.LastSyncDate, actualInterviewerAssignment.LastSyncDate);
           if (PropertyExist(expectedInterviewerAssignment,"Interviewer")) Assert.Equal(expectedInterviewerAssignment.Interviewer, actualInterviewerAssignment.Interviewer);
           if (PropertyExist(expectedInterviewerAssignment,"Active")) Assert.Equal(expectedInterviewerAssignment.Active, actualInterviewerAssignment.IsActive);
           if (PropertyExist(expectedInterviewerAssignment,"Assigned")) Assert.Equal(expectedInterviewerAssignment.Assigned, actualInterviewerAssignment.IsAssigned);
           if (PropertyExist(expectedInterviewerAssignment,"AssignedSamplingPointTarget")) Assert.Equal(expectedInterviewerAssignment.AssignedSamplingPointTarget, actualInterviewerAssignment.AssignedSamplingPointTarget);
           if (PropertyExist(expectedInterviewerAssignment,"AssignedTarget")) Assert.Equal(expectedInterviewerAssignment.AssignedTarget, actualInterviewerAssignment.AssignedTarget);
           if (PropertyExist(expectedInterviewerAssignment,"Discriminator")) Assert.Equal(expectedInterviewerAssignment.Discriminator, actualInterviewerAssignment.Discriminator);
           if (PropertyExist(expectedInterviewerAssignment,"DroppedOut")) Assert.Equal(expectedInterviewerAssignment.DroppedOut, actualInterviewerAssignment.DroppedOutCount);
           if (PropertyExist(expectedInterviewerAssignment,"IsFullSynced")) Assert.Equal(expectedInterviewerAssignment.IsFullSynced, actualInterviewerAssignment.IsFullSynced);
           if (PropertyExist(expectedInterviewerAssignment,"IsGroupAssignment")) Assert.Equal(expectedInterviewerAssignment.IsGroupAssignment, actualInterviewerAssignment.IsGroupAssignment);
           if (PropertyExist(expectedInterviewerAssignment,"InterviewerId")) Assert.Equal(expectedInterviewerAssignment.InterviewerId, actualInterviewerAssignment.InterviewerId);
           if (PropertyExist(expectedInterviewerAssignment,"Rejected")) Assert.Equal(expectedInterviewerAssignment.Rejected, actualInterviewerAssignment.RejectedCount);
           if (PropertyExist(expectedInterviewerAssignment,"ScreenedOut")) Assert.Equal(expectedInterviewerAssignment.ScreenedOut, actualInterviewerAssignment.ScreenedOutCount);
           if (PropertyExist(expectedInterviewerAssignment,"Successful")) Assert.Equal(expectedInterviewerAssignment.Successful, actualInterviewerAssignment.SuccessfulCount);
           if (PropertyExist(expectedInterviewerAssignment,"SurveyId")) Assert.Equal(expectedInterviewerAssignment.SurveyId, actualInterviewerAssignment.SurveyId);
           if (PropertyExist(expectedInterviewerAssignment,"SurveyName")) Assert.Equal(expectedInterviewerAssignment.SurveyName, actualInterviewerAssignment.SurveyName);
        }
        private Uri ApiUri(string interviewerId)
        {
            return new Uri(ServiceAddress, "Interviewers/" + interviewerId + "/Assignments");
        }

        static private bool PropertyExist(dynamic obj, string property)
        {
            Type type = obj.GetType();
            var props = type.GetProperties();
            return props.Any(p => p.Name == property);
        }


        #endregion

    }
}

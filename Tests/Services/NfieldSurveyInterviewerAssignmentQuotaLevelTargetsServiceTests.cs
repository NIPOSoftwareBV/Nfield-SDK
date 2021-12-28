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
using System.Net;
using Moq;
using Nfield.Infrastructure;
using Nfield.SDK.Models;
using Nfield.SDK.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyInterviewerAssignmentQuotaLevelTargetsService"/>
    /// </summary>
    public class NfieldSurveyInterviewerAssignmentQuotaLevelTargetsServiceTests : NfieldServiceTestsBase
    {
        const string surveyId = "MySurvey";
        const string interviewerId = "InterviewerId";
        private IEnumerable<WorkPackageTarget> _workPackageTargets;

        public NfieldSurveyInterviewerAssignmentQuotaLevelTargetsServiceTests()
        {
            _workPackageTargets = new List<WorkPackageTarget>()
            {
                new WorkPackageTarget()
                {
                    LevelId = "LevelId1",
                    Target = 100
                },
                new WorkPackageTarget()
                {
                    LevelId = "LevelId2",
                    Target = 200
                }
            };
        }

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_SurveyIdIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveyInterviewerAssignmentQuotaLevelTargetsService();
            
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.UpdateAsync(null, interviewerId, _workPackageTargets)));
        }

        [Fact]
        public void TestUpdateAsync_SurveyIdIsEmpty_ThrowsArgumentException()
        {
            var target = new NfieldSurveyInterviewerAssignmentQuotaLevelTargetsService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.UpdateAsync(string.Empty, interviewerId, _workPackageTargets)));
        }

        [Fact]
        public void TestUpdateAsync_InterviewIdIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveyInterviewerAssignmentQuotaLevelTargetsService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.UpdateAsync(surveyId, null, _workPackageTargets)));
        }

        [Fact]
        public void TestUpdateAsync_InterviewIdIsEmpty_ThrowsArgumentException()
        {
            var target = new NfieldSurveyInterviewerAssignmentQuotaLevelTargetsService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.UpdateAsync(surveyId, string.Empty, _workPackageTargets)));
        }

        [Fact]
        public void TestUpdateAsync_WorkPackageTargetsIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveyInterviewerAssignmentQuotaLevelTargetsService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.UpdateAsync(surveyId, interviewerId, null)));
        }

        [Fact]
        public void TestUpdateAsync_WorkPackageTargetsIsEmpty_ThrowsArgumentException()
        {
            var target = new NfieldSurveyInterviewerAssignmentQuotaLevelTargetsService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.UpdateAsync(surveyId, string.Empty, new List<WorkPackageTarget>())));
        }


        [Fact]
        public async void TestUpdateAsync_DoesNotThrow()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var target = new NfieldSurveyInterviewerAssignmentQuotaLevelTargetsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{surveyId}/Assignments/{interviewerId}"), _workPackageTargets))
                .Returns(CreateTask(HttpStatusCode.OK));

            // Succcess if it does not throw
            await target.UpdateAsync(surveyId, interviewerId, _workPackageTargets);
        }
        #endregion

    }
}

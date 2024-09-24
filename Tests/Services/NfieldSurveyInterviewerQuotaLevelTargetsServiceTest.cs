using Moq;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.SDK.Models;
using Nfield.SDK.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace Nfield.Services
{
    public class NfieldSurveyInterviewerQuotaLevelTargetsServiceTest : NfieldServiceTestsBase
    {
        const string surveyId = "MySurvey";
        const string interviewerId = "InterviewerId";
        private IEnumerable<WorkPackageTarget> _workPackageTargets;
        private IEnumerable<WorkPackageTargetCounts> _workPackageTargetsCounts;
        private Mock<INfieldConnectionClient> MockedNfieldConnection = new Mock<INfieldConnectionClient>();
        private NfieldSurveyInterviewerQuotaLevelTargetsService _target;

        public NfieldSurveyInterviewerQuotaLevelTargetsServiceTest()
        {
            _target = new NfieldSurveyInterviewerQuotaLevelTargetsService();
            _target.InitializeNfieldConnection(MockedNfieldConnection.Object);
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

            _workPackageTargetsCounts = new List<WorkPackageTargetCounts>()
            {
                new WorkPackageTargetCounts()
                {
                    LevelId = "LevelId1",
                    Target = 100,
                    Successful = 20,
                    SurveySuccessful = 50,
                },
                new WorkPackageTargetCounts()
                {
                    LevelId = "LevelId2",
                    Target = 200,
                    Successful = 10,
                    SurveySuccessful = 51,
                }
            };

        }

        #region PutAsync

        [Fact]
        public void TestPutAsync_SurveyIdIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.PutAsync(null, interviewerId, _workPackageTargets)));
        }

        [Fact]
        public void TestPutAsync_SurveyIdIsEmpty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.PutAsync(string.Empty, interviewerId, _workPackageTargets)));
        }

        [Fact]
        public void TestPutAsync_InterviewIdIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.PutAsync(surveyId, null, _workPackageTargets)));
        }

        [Fact]
        public void TestPutAsync_InterviewIdIsEmpty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.PutAsync(surveyId, string.Empty, _workPackageTargets)));
        }

        [Fact]
        public void TestPutAsync_workPackageTargetsIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.PutAsync(surveyId, interviewerId, null)));
        }

        [Fact]
        public void TestPutAsync_workPackageTargetsIsEmpty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.PutAsync(surveyId, string.Empty, new List<WorkPackageTarget>())));
        }


        [Fact]
        public async void TestPutAsync_DoesNotThrow()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var _target = new NfieldSurveyInterviewerAssignmentQuotaLevelTargetsService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            mockedHttpClient
            .Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{surveyId}/Assignments/{interviewerId}"), _workPackageTargets))
                .Returns(CreateTask(HttpStatusCode.OK));

            // Succcess if it does not throw
            await _target.UpdateAsync(surveyId, interviewerId, _workPackageTargets);
        }
        #endregion

        #region GetAsync

        [Fact]
        public void TestGetAsync_ReturnsData()
        {
            var mockClient = InitMockClientGet(new Uri($"{ServiceAddress.ToString()}Surveys/{surveyId}/Interviewers/{interviewerId}/QuotaLevelTargets/"), _workPackageTargetsCounts);
            _target.InitializeNfieldConnection(mockClient);

            var actual = _target.GetAsync(surveyId, interviewerId).Result;

            Assert.Equal(_workPackageTargetsCounts.First().LevelId, actual.First().LevelId);
            Assert.Equal(_workPackageTargetsCounts.First().Target, actual.First().Target);
            Assert.Equal(_workPackageTargetsCounts.First().Successful, actual.First().Successful);
            Assert.Equal(_workPackageTargetsCounts.First().SurveySuccessful, actual.First().SurveySuccessful);

        }

        #endregion
    }
}

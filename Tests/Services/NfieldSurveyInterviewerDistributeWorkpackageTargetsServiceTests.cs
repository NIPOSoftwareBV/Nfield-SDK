using Moq;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.SDK.Models;
using Nfield.SDK.Services.Implementation;
using Nfield.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Nfield.Services
{
    public class NfieldSurveyInterviewerDistributeWorkpackageTargetsServiceTests : NfieldServiceTestsBase
    {
        const string surveyId = "MySurvey";
        const string interviewerId = "InterviewerId";
        private SurveyInterviewerDistributeModel _surveyInterviewerDistribute;
        private Mock<INfieldConnectionClient> MockedNfieldConnection = new Mock<INfieldConnectionClient>();
        private NfieldSurveyInterviewerDistributeWorkpackageTargetsService _target;

        public NfieldSurveyInterviewerDistributeWorkpackageTargetsServiceTests()
        {
            _target = new NfieldSurveyInterviewerDistributeWorkpackageTargetsService();
            _target.InitializeNfieldConnection(MockedNfieldConnection.Object);
            _surveyInterviewerDistribute = new SurveyInterviewerDistributeModel()
            {
                Description = "Test Description",
                InterviewerIds = new List<string>()
                {
                    "InterviewerId1",
                    "InterviewerId2"
                },
                SurveyGrossTargetToDistribute = 100                
            };
                      
        }       

        #region PostAsync

        [Fact]
        public void TestPutAsync_SurveyIdIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.PostAsync(null, interviewerId, _surveyInterviewerDistribute)));
        }

        [Fact]
        public void TestPutAsync_SurveyIdIsEmpty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.PostAsync(string.Empty, interviewerId, _surveyInterviewerDistribute)));
        }

        [Fact]
        public void TestPutAsync_InterviewIdIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.PostAsync(surveyId, null, _surveyInterviewerDistribute)));
        }

        [Fact]
        public void TestPutAsync_InterviewIdIsEmpty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.PostAsync(surveyId, string.Empty, _surveyInterviewerDistribute)));
        }

        [Fact]
        public void TestPutAsync_surveyInterviewerDistributeIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.PostAsync(surveyId, interviewerId, null)));
        }

        [Fact]
        public void TestPutAsync_surveyInterviewerDistributeIsEmpty_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(_target.PostAsync(surveyId, string.Empty, new SurveyInterviewerDistributeModel())));
        }


        [Fact]
        public async void TestPutAsync_DoesNotThrow()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var _target = new NfieldSurveyInterviewerDistributeWorkpackageTargetsService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            mockedHttpClient
            .Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{surveyId}/Assignments/{interviewerId}"), _surveyInterviewerDistribute))
                .Returns(CreateTask(HttpStatusCode.OK));

            // Succcess if it does not throw
            await _target.PostAsync(surveyId, interviewerId, _surveyInterviewerDistribute);
        }
        #endregion

        
    }
}

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
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.SDK.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyInterviewerAssignmentsService"/>
    /// </summary>
    public class NfieldSurveyInterviewerAssignmentsServiceTests : NfieldServiceTestsBase
    {
        #region AssignAsync tests

        [Fact]
        public void TestAssignAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.AssignAsync(null, "interviewerId")));
        }

        [Fact]
        public void TestAssignAsync_InterviewerIdIsNull_Throws()
        {
            var target = new NfieldSurveyInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.AssignAsync("surveyId", null)));
        }

        [Fact]
        public void TestAssignAsync_ValidSurveyResponseCode_CallsCorrectUrl()
        {
            // Arrange
            const string surveyId = "surveyId";
            const string interviewerId = "interviewerId";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.PutAsJsonAsync(It.IsAny<Uri>(), It.IsAny<SurveyInterviewerAssignmentChangeModel>()))
                .Returns(CreateTask(HttpStatusCode.OK));
            var target = new NfieldSurveyInterviewerAssignmentsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            target.AssignAsync(surveyId, interviewerId).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                    hc.PutAsJsonAsync(
                        It.Is<Uri>(url => url.AbsolutePath.EndsWith("Surveys/" + surveyId + "/Assignment/")),
                        It.Is<SurveyInterviewerAssignmentChangeModel>(model => model.InterviewerId == interviewerId && model.Assign)),
                    Times.Once());
        }

        #endregion

        #region UnassignAsync tests

        [Fact]
        public void TestUnassignAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.UnassignAsync(null, "interviewerId")));
        }

        [Fact]
        public void TestUnassignAsync_InterviewerIdIsNull_Throws()
        {
            var target = new NfieldSurveyInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.UnassignAsync("surveyId", null)));
        }

        [Fact]
        public void TestUnassignAsync_ValidSurveyResponseCode_CallsCorrectUrl()
        {
            // Arrange
            const string surveyId = "surveyId";
            const string interviewerId = "interviewerId";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.PutAsJsonAsync(It.IsAny<Uri>(), It.IsAny<SurveyInterviewerAssignmentChangeModel>()))
                .Returns(CreateTask(HttpStatusCode.OK));
            var target = new NfieldSurveyInterviewerAssignmentsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            target.UnassignAsync(surveyId, interviewerId).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                    hc.PutAsJsonAsync(
                        It.Is<Uri>(url => url.AbsolutePath.EndsWith("Surveys/" + surveyId + "/Assignment/")),
                        It.Is<SurveyInterviewerAssignmentChangeModel>(model => model.InterviewerId == interviewerId && !model.Assign)),
                    Times.Once());
        }

        #endregion

        #region PutAsync        

        [Fact]
        public void TestUpdateAsync_ModelArgumentIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveyInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.PutAsync("survey", "interviewer", null)));
        }

        [Theory]
        [InlineData(null, "id", true)]
        [InlineData(null, "", true)]
        [InlineData(null, null, true)]
        [InlineData("id", null, true)]
        [InlineData("", null, true)]
        [InlineData("", "", true)]
        [InlineData(null, "id", false)]
        [InlineData(null, "", false)]
        [InlineData(null, null, false)]
        [InlineData("id", null, false)]
        [InlineData("", null, false)]
        [InlineData("", "", false)]
        public void TestPutAsync_ThrowsArgumentNullException(string surveyId, string interviewerId, bool modelIsNotNull)
        {
            var target = new NfieldSurveyInterviewerAssignmentsService();
            SurveyInterviewerAssignmentModel interviewerAssignmentModel = (modelIsNotNull) ? new SurveyInterviewerAssignmentModel() : null;

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await target.PutAsync(surveyId, interviewerId, interviewerAssignmentModel);
            });
        }

        [Fact]
        public async Task TestPutAsync_CreateAssignment()
        {
            var target = new NfieldSurveyInterviewerAssignmentsService();
            string surveyId = "survey-id";
            string interviewerId = "interviewer-id";
            var interviewerAssignmentModel = new SurveyInterviewerAssignmentModel
            {
                AssignmentType = "AssignmentType",
                Description = "Description",
                SamplingPointsFilter = new[] { new FilterWithOr { Name = "samplingPointId", Value = "id", Op = "eq" } },                
                TargetToDistribute = 5
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(It.Is<Uri>(u => u.ToString().EndsWith($"Surveys/{surveyId}/Interviewers/{interviewerId}/Assignments")), It.Is<SurveyInterviewerAssignmentModel>
                (ia =>
                        ia.AssignmentType == interviewerAssignmentModel.AssignmentType &&
                        ia.Description == interviewerAssignmentModel.Description &&
                        ia.SamplingPointsFilter == interviewerAssignmentModel.SamplingPointsFilter &&
                        ia.TargetToDistribute == interviewerAssignmentModel.TargetToDistribute
                    )))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(interviewerAssignmentModel)))).Verifiable();

            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

     
            await target.PutAsync(surveyId, interviewerId, interviewerAssignmentModel);

            mockedHttpClient.Verify();
        }


        [Fact]
        public async Task TestGetAsync_GetTargetsAsync()
        {
            var target = new NfieldSurveyInterviewerAssignmentsService();
            string surveyId = "survey-id";
            string clientInterviewerId = "clientInterviewer-id";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var targetsList = new List<SurveyInterviewerAssignmentQuotaTargetModel>() { new SurveyInterviewerAssignmentQuotaTargetModel() { LevelId = "1", Successful = 2, SurveySuccessful = 3, Target = 4 } };
            mockedHttpClient
                .Setup(client => client.GetAsync(It.Is<Uri>(u => u.ToString().EndsWith($"Surveys/{surveyId}/Interviewers/{clientInterviewerId}/Assignments/QuotaTargets"))))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(targetsList)))).Verifiable();

            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var result  = (await target.GetTargetsAsync(surveyId, clientInterviewerId)).ToArray();

            mockedHttpClient.Verify();

            Assert.Equal("1", result[0].LevelId);
            Assert.Equal(2, result[0].Successful);
            Assert.Equal(3, result[0].SurveySuccessful);
            Assert.Equal(4, result[0].Target);
        }

        [Theory]
        [InlineData(null, "id")]
        [InlineData(null, "")]
        [InlineData(null, null)]
        [InlineData("id", null)]
        [InlineData("", null)]
        [InlineData("", "")]
        public void TestGetAsync_ThrowsArgumentNullException(string surveyId, string clienInterviewerId)
        {
            var target = new NfieldSurveyInterviewerAssignmentsService();

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await target.GetTargetsAsync(surveyId, clienInterviewerId);
            });
        }

        #endregion

    }
}

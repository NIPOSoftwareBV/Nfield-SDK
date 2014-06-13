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
using System.Net;
using Moq;
using Nfield.Infrastructure;
using Nfield.Models;
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

            mockedHttpClient.Setup(client => client.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<SurveyInterviewerAssignmentChangeModel>()))
                .Returns(CreateTask(HttpStatusCode.OK));
            var target = new NfieldSurveyInterviewerAssignmentsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            target.AssignAsync(surveyId, interviewerId).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                    hc.PutAsJsonAsync(
                        It.Is<string>(url => url.EndsWith("Surveys/" + surveyId + "/Assignment/")),
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

            mockedHttpClient.Setup(client => client.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<SurveyInterviewerAssignmentChangeModel>()))
                .Returns(CreateTask(HttpStatusCode.OK));
            var target = new NfieldSurveyInterviewerAssignmentsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            target.UnassignAsync(surveyId, interviewerId).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                    hc.PutAsJsonAsync(
                        It.Is<string>(url => url.EndsWith("Surveys/" + surveyId + "/Assignment/")),
                        It.Is<SurveyInterviewerAssignmentChangeModel>(model => model.InterviewerId == interviewerId && !model.Assign)),
                    Times.Once());
        }

        #endregion

    }
}

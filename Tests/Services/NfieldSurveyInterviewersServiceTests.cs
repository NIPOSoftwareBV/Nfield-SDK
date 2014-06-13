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
    /// <summary>
    /// Tests for <see cref="NfieldSurveyInterviewersService"/>
    /// </summary>
    public class NfieldSurveyInterviewersServiceTests : NfieldServiceTestsBase
    {
        #region GetAsync tests

        [Fact]
        public void TestGetAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyInterviewersService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.GetAsync(null)));
        }

        [Fact]
        public void TestGetAsync_ReturnsCorrectResults()
        {
            var expectedInterviewers = new[]
            {
                new SurveyInterviewerAssignmentModel {InterviewerId = "id1", IsActive = true, IsAssigned = true},
                new SurveyInterviewerAssignmentModel {InterviewerId = "id2", IsActive = false, IsAssigned = false}
            };
            const string surveyId = "surveyId";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.GetAsync(It.Is<string>(url => url.EndsWith("Surveys/" + surveyId + "/Interviewers/"))))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedInterviewers))));
            var target = new NfieldSurveyInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            var actual = target.GetAsync(surveyId).Result.ToArray();

            // Assert
            Assert.Equal(expectedInterviewers.Length, actual.Length);
            Assert.Equal(expectedInterviewers[0].InterviewerId, actual[0].InterviewerId);
            Assert.Equal(expectedInterviewers[0].IsActive, actual[0].IsActive);
            Assert.Equal(expectedInterviewers[0].IsAssigned, actual[0].IsAssigned);
            Assert.Equal(expectedInterviewers[1].InterviewerId, actual[1].InterviewerId);
            Assert.Equal(expectedInterviewers[1].IsActive, actual[1].IsActive);
            Assert.Equal(expectedInterviewers[1].IsAssigned, actual[1].IsAssigned);
        }

        #endregion

        #region AddAsync tests

        [Fact]
        public void TestAddAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyInterviewersService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.AddAsync(null, "interviewerId")));
        }

        [Fact]
        public void TestAddAsync_InterviewerIdIsNull_Throws()
        {
            var target = new NfieldSurveyInterviewersService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.AddAsync("surveyId", null)));
        }

        [Fact]
        public void TestAddAsync_ValidSurveyResponseCode_CallsCorrectUrl()
        {
            // Arrange
            const string surveyId = "surveyId";
            const string interviewerId = "interviewerId";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<SurveyInterviewerAddModel>()))
                .Returns(CreateTask(HttpStatusCode.OK));
            var target = new NfieldSurveyInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            target.AddAsync(surveyId, interviewerId).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                    hc.PostAsJsonAsync(
                        It.Is<string>(url => url.EndsWith("Surveys/" + surveyId + "/Interviewers/")), 
                        It.Is<SurveyInterviewerAddModel>(model => model.InterviewerId == interviewerId)),
                    Times.Once());
        }

        #endregion

    }
}

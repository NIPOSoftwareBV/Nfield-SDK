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
    /// Tests for <see cref="NfieldSamplingPointInterviewerAssignmentsService"/>
    /// </summary>
    public class NfieldSamplingPointInterviewerAssignmentsServiceTests : NfieldServiceTestsBase
    {
        const string SurveyId = "MySurvey";
        const string SamplingPointId = "MySamplingPoint";
        const string InterviewerId = "MyInterviewer";

        #region BatchAssignAsync

        [Fact]
        public void TestBatchAssignAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.AssignAsync(null, new SamplingPointInterviewerAssignmentsModel())));
        }

        [Fact]
        public void TestBatchAssignAsync_ModelIsNull_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.AssignAsync(SurveyId, null)));
        }

        [Fact]
        public void TestBatchAssignAsync_ServerAcceptsAssign_ReturnsNoError()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.PostAsJsonAsync(It.IsAny<Uri>(), It.IsAny<SamplingPointInterviewerAssignmentsModel>()))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var model = new SamplingPointInterviewerAssignmentsModel();
            target.AssignAsync(SurveyId, model);

            mockedHttpClient.Verify(hc =>
                hc.PostAsJsonAsync(
                    It.Is<Uri>(url => url.AbsolutePath.EndsWith("Surveys/" + SurveyId + "/SamplingPointsAssignments")),
                    It.Is<SamplingPointInterviewerAssignmentsModel>(m => m == model)),
                Times.Once());
        }

        #endregion

        #region AssignAsync

        [Fact]
        public void TestAssignAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.AssignAsync(null, SamplingPointId, InterviewerId)));
        }

        [Fact]
        public void TestAssignAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.AssignAsync("", SamplingPointId, InterviewerId)));
        }

        [Fact]
        public void TestAssignAsync_SamplingPointIdIsNull_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.AssignAsync(SurveyId, null, InterviewerId)));
        }

        [Fact]
        public void TestAssignAsync_SamplingPointIdIsEmpty_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.AssignAsync(SurveyId, "", InterviewerId)));
        }

        [Fact]
        public void TestAssignAsync_InterviewerIdIsNull_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.AssignAsync(SurveyId, SamplingPointId, null)));
        }

        [Fact]
        public void TestAssignAsync_InterviewerIdIsEmpty_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.AssignAsync(SurveyId, SamplingPointId, "")));
        }

        [Fact]
        public void TestAssignAsync_ServerAcceptsAssign_ReturnsNoError()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.PostAsync(It.IsAny<Uri>(), It.IsAny<HttpContent>())).Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.AssignAsync(SurveyId, SamplingPointId, InterviewerId);

            mockedHttpClient.Verify(hc =>
                hc.PostAsync(
                    It.Is<Uri>(url => url.AbsolutePath.EndsWith("Surveys/" + SurveyId + "/SamplingPoints/" + SamplingPointId + "/Assignments/" + InterviewerId)),
                    null),
                Times.Once());
        }

        #endregion

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(() =>
                    UnwrapAggregateException(target.QueryAsync(null, SamplingPointId)));
        }

        [Fact]
        public void TestQueryAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.QueryAsync("", SamplingPointId)));
        }

        [Fact]
        public void TestQueryAsync_SamplingPointIdIsNull_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(() =>
                    UnwrapAggregateException(target.QueryAsync(SurveyId, null)));
        }

        [Fact]
        public void TestQueryAsync_SamplingPointIdIsEmpty_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.QueryAsync(SurveyId, "")));
        }

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithAssignments()
        {
            var expectedAssignments = new[]
            {
                new InterviewerSamplingPointAssignmentModel {Active = false, Assigned = false, FirstName = "fn1", InterviewerId = "id1", LastName = "ln1", UserName = "un1"},
                new InterviewerSamplingPointAssignmentModel {Active = true, Assigned = true, FirstName = "fn2", InterviewerId = "id2", LastName = "ln2", UserName = "un2"}
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.GetAsync(It.IsAny<Uri>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedAssignments))));

            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualAssignments = target.QueryAsync(SurveyId, SamplingPointId).Result;
            Assert.Equal(expectedAssignments[0].InterviewerId, actualAssignments.ToArray()[0].InterviewerId);
            Assert.Equal(expectedAssignments[1].InterviewerId, actualAssignments.ToArray()[1].InterviewerId);
            Assert.Equal(2, actualAssignments.Count());

        }


        #endregion

        #region BatchUnassignAsync

        [Fact]
        public void TestBatchUnassignAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.AssignAsync(null, new SamplingPointInterviewerAssignmentsModel())));
        }

        [Fact]
        public void TestBatchUnassignAsync_ModelIsNull_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.AssignAsync(SurveyId, null)));
        }

        [Fact]
        public void TestBatchUnassignAsync_ServerAcceptsUnassign_ReturnsNoError()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.DeleteAsJsonAsync(It.IsAny<Uri>(), It.IsAny<SamplingPointInterviewerAssignmentsModel>()))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var model = new SamplingPointInterviewerAssignmentsModel();
            target.UnassignAsync(SurveyId, model);

            mockedHttpClient.Verify(hc =>
                hc.DeleteAsJsonAsync(
                    It.Is<Uri>(url => url.AbsolutePath.EndsWith("Surveys/" + SurveyId + "/SamplingPointsAssignments")),
                    It.Is<SamplingPointInterviewerAssignmentsModel>(m => m == model)),
                Times.Once());
        }

        #endregion

        #region UnassignAsync

        [Fact]
        public void TestUnassignAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.UnassignAsync(null, SamplingPointId, InterviewerId)));
        }

        [Fact]
        public void TestUnassignAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.UnassignAsync("", SamplingPointId, InterviewerId)));
        }

        [Fact]
        public void TestUnassignAsync_SamplingPointIdIsNull_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.UnassignAsync(SurveyId, null, InterviewerId)));
        }

        [Fact]
        public void TestUnassignAsync_SamplingPointIdIsEmpty_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.UnassignAsync(SurveyId, "", InterviewerId)));
        }

        [Fact]
        public void TestUnassignAsync_InterviewerIdIsNull_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.UnassignAsync(SurveyId, SamplingPointId, null)));
        }

        [Fact]
        public void TestUnassignAsync_InterviewerIdIsEmpty_Throws()
        {
            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.UnassignAsync(SurveyId, SamplingPointId, "")));
        }

        [Fact]
        public void TestUnassignAsync_ServerAcceptsUnassign_ReturnsNoError()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.DeleteAsync(It.IsAny<Uri>()))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldSamplingPointInterviewerAssignmentsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.UnassignAsync(SurveyId, SamplingPointId, InterviewerId).Wait();

            mockedHttpClient.Verify(hc =>
            hc.DeleteAsync(
                It.Is<Uri>(url => url.AbsolutePath.EndsWith("Surveys/" + SurveyId + "/SamplingPoints/" + SamplingPointId + "/Assignments/" + InterviewerId))),
            Times.Once());
        }
        #endregion
    }
}

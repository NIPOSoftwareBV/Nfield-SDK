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
using Nfield.Infrastructure;
using Nfield.Services.Implementation;
using Xunit;
using Nfield.Models;
using Moq;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldCapiInterviewersService"/>
    /// </summary>
    public class NfieldCapiInterviewersServiceTests : NfieldServiceTestsBase
    {
        const string CapiInterviewersEndpoint = "CapiInterviewers/";

        #region AddAsync

        [Fact]
        public void TestAddAsync_ServerAcceptsInterviewer_ReturnsInterviewer()
        {
            var interviewer = new Interviewer { UserName = "User X" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(interviewer));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, CapiInterviewersEndpoint), interviewer))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldCapiInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddAsync(interviewer).Result;

            Assert.Equal(interviewer.UserName, actual.UserName);
        }

        #endregion

        #region RemoveAsync

        [Fact]
        public void TestRemoveAsync_InterviewerIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldCapiInterviewersService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(null)));
        }

        [Fact]
        public void TestRemoveAsync_ServerRemovedInterviewer_DoesNotThrow()
        {
            const string InterviewerId = "Interviewer X";
            var interviewer = new Interviewer { InterviewerId = InterviewerId };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, CapiInterviewersEndpoint + InterviewerId)))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldCapiInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // assert: no throw
            target.RemoveAsync(interviewer).Wait();
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_InterviewerArgumentIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldCapiInterviewersService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync(null)));
        }

        [Fact]
        public void TestUpdateAsync_InterviewerExists_ReturnsInterviewer()
        {
            const string InterviewerId = "Interviewer X";

            var interviewerIn = new Interviewer
            {
                InterviewerId = InterviewerId,
                UserName = "XXX"
            };

            var interviewerOut = new InterviewerChanged
            {
                InterviewerId = InterviewerId,
                UserName = "XXX"
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(new Uri(ServiceAddress, CapiInterviewersEndpoint + InterviewerId), It.IsAny<UpdateInterviewer>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(interviewerOut))));

            var target = new NfieldCapiInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.UpdateAsync(interviewerIn).Result;

            Assert.Equal(interviewerIn.InterviewerId, actual.InterviewerId);
        }

        #endregion

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithInterviewers()
        {
            var expectedInterviewers = new Interviewer[]
            { new Interviewer{InterviewerId = "TestInterviewer"},
              new Interviewer{InterviewerId = "AnotherTestInterviewer"}
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, CapiInterviewersEndpoint)))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedInterviewers))));

            var target = new NfieldCapiInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualInterviewers = target.QueryAsync().Result;
            Assert.Equal(expectedInterviewers[0].InterviewerId, actualInterviewers.ToArray()[0].InterviewerId);
            Assert.Equal(expectedInterviewers[1].InterviewerId, actualInterviewers.ToArray()[1].InterviewerId);
            Assert.Equal(2, actualInterviewers.Count());
        }

        #endregion

        #region ChangePasswordAsync

        [Fact]
        public void TestChangePasswordAsync_InterviewerIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldCapiInterviewersService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.ChangePasswordAsync(null, string.Empty)));
        }

        [Fact]
        public void TestChangePasswordAsync_ServerChangesPassword_ReturnsInterviewer()
        {
            const string Password = "Password";
            const string InterviewerId = "Interviewer X";
            var interviewer = new Interviewer { InterviewerId = InterviewerId };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, CapiInterviewersEndpoint + InterviewerId), It.IsAny<object>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(interviewer))));

            var target = new NfieldCapiInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.ChangePasswordAsync(interviewer, Password).Result;

            Assert.Equal(interviewer.InterviewerId, actual.InterviewerId);
        }

        #endregion

        #region QueryOfficesOfInterviewerAsync

        [Fact]
        public void TestQueryOfficesOfInterviewerAsync_ServerReturnsQuery_ReturnsListWithFieldworkOffices()
        {
            const string interviewerId = "interviewerId";

            var expectedFieldworkOffices = new[]
            {
                "Amsterdam",
                "Barcelona",
                "Headquarters"
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(
                    new Uri(ServiceAddress, $"{CapiInterviewersEndpoint}{interviewerId}/Offices"))
                )
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(expectedFieldworkOffices))));

            var target = new NfieldCapiInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualFieldworkOffices = target.QueryOfficesOfInterviewerAsync(interviewerId).Result;
            var fieldworkOffices = actualFieldworkOffices as string[] ?? actualFieldworkOffices.ToArray();
            Assert.Equal(expectedFieldworkOffices[0], fieldworkOffices[0]);
            Assert.Equal(expectedFieldworkOffices[1], fieldworkOffices[1]);
            Assert.Equal(expectedFieldworkOffices[2], fieldworkOffices[2]);
            Assert.Equal(3, fieldworkOffices.Count());
        }

        #endregion

        #region AddInterviewerToFieldworkOfficesAsync

        [Fact]
        public void TestAddInterviewerToFieldworkOfficesAsync_WhenExecuted_CallsClientPostAsJsonAsyncWithCorrectArgs()
        {
            const string interviewerId = "interviewerId";
            const string fieldworkOfficeId = "Barcelona";

            var expectedUrl = new Uri(ServiceAddress, $"{CapiInterviewersEndpoint}{interviewerId}/Offices");

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(It.IsAny<Uri>(), It.IsAny<InterviewerFieldworkOfficeModel>()))
                .Returns(CreateTask(HttpStatusCode.OK));


            var target = new NfieldCapiInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.AddInterviewerToFieldworkOfficesAsync(interviewerId, fieldworkOfficeId);

            mockedHttpClient.Verify(
                h =>
                    h.PostAsJsonAsync(expectedUrl, It.Is<InterviewerFieldworkOfficeModel>(f => f.OfficeId == fieldworkOfficeId)),
                Times.Once());
        }

        #endregion

        #region RemoveInterviewerFromFieldworkOfficesAsync

        [Fact]
        public void TestRemoveInterviewerFromFieldworkOfficesAsync_WhenExecuted_CallsClientPostAsJsonAsyncWithCorrectArgs()
        {
            const string interviewerId = "interviewerId";
            const string fieldworkOfficeId = "Barcelona";

            var expectedUrl = $"{CapiInterviewersEndpoint}{interviewerId}/Offices/{fieldworkOfficeId}";

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient
                .Setup(client => client.DeleteAsync(It.IsAny<Uri>()))
                .Returns(CreateTask(HttpStatusCode.OK));


            var target = new NfieldCapiInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.RemoveInterviewerFromFieldworkOfficesAsync(interviewerId, fieldworkOfficeId).Wait();



            mockedHttpClient.Verify(
                h =>
                    h.DeleteAsync(new Uri(ServiceAddress, expectedUrl)),
                Times.Once());
        }

        #endregion

        #region GetInterviewersWorklogDownloadLinkAsync

        [Fact]
        public async Task DownloadLogs()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var target = new NfieldCapiInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            const string activityId = "activity-id";
            const string logsLink1 = "logs-link-1";
            var query = new LogQueryModel
            {
                From = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                To = DateTime.UtcNow
            };

            mockedHttpClient
               .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, $"InterviewersWorklog"), It.Is<LogQueryModel>(
                   q => q.From == query.From && q.To == query.To)))
               .Returns(
                Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { ActivityId = activityId }))
                    })).Verifiable();

            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"BackgroundActivities/{activityId}")))
                .Returns(Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { DownloadDataUrl = logsLink1, ActivityId = activityId, Status = 2 /* Succeeded */ }))
                    })).Verifiable();

            // Test it using the model
            var result = await target.QueryLogsAsync(query);
            mockedHttpClient.Verify();
            Assert.Equal(logsLink1, result);

        }

        #endregion

    }
}

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
using static Nfield.Services.Implementation.NfieldCapiInterviewersService;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldCapiInterviewersService"/>
    /// </summary>
    public class NfieldCapiInterviewersServiceTests : NfieldServiceTestsBase
    {
        private const string CapiInterviewersEndpoint = "CapiInterviewers/";
        private const string InterviewerId = "Interviewer X";
        private const string Password = "password";
        private const string FieldworkOfficeId = "Barcelona";
        private const string ActivityId = "activity-id";
        private const string LogsLink1 = "logs-link-1";

        private readonly Uri _capiInterviewersApi;
        private readonly Interviewer _interviewer;
        private readonly CapiInterviewer _capiInterviewer;
        private readonly NfieldCapiInterviewersService _target;
        private readonly Mock<INfieldHttpClient> _mockedHttpClient;
        private readonly Mock<INfieldConnectionClient> _mockedNfieldConnection;

        public NfieldCapiInterviewersServiceTests()
        {
            _mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(_mockedNfieldConnection);

            _target = new NfieldCapiInterviewersService();
            _target.InitializeNfieldConnection(_mockedNfieldConnection.Object);
            _capiInterviewersApi = new Uri(ServiceAddress, CapiInterviewersEndpoint);

            _interviewer = new Interviewer { InterviewerId = InterviewerId, UserName = "User X" };
            _capiInterviewer = new CapiInterviewer {
                InterviewerId = _interviewer.InterviewerId,
                UserName = _interviewer.UserName
            };
        }

        #region AddAsync

        [Fact]
        public async Task TestAddAsync_ServerAcceptsInterviewer_ReturnsInterviewerAsync()
        {
            // Arrange
            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(_capiInterviewersApi,
                            It.Is<NewCapiInterviewer>(nci => nci.UserName == _interviewer.UserName)))
                .Returns(CreateTask(HttpStatusCode.OK, SerializedCapiInterviewer()));

            // Act
            var actual = await _target.AddAsync(_interviewer);

            // Assert
            Assert.Equal(_interviewer.UserName, actual.UserName);
            Assert.Equal(_capiInterviewer.InterviewerId, actual.InterviewerId);
        }

        #endregion

        #region RemoveAsync

        [Fact]
        public void TestRemoveAsync_InterviewerIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.RemoveAsync(null)));
        }

        [Fact]
        public async Task TestRemoveAsync_ServerRemovedInterviewer_CallsDeleteAsync()
        {
            // Arrange
            var expectedUrl = new Uri(_capiInterviewersApi, InterviewerId);
            _mockedHttpClient
                .Setup(client => client.DeleteAsync(expectedUrl))
                .Returns(CreateTask(HttpStatusCode.OK));

            // Act & Assert
            await _target.RemoveAsync(_interviewer);

            // Assert
            _mockedHttpClient.Verify(
                h =>
                    h.DeleteAsync(expectedUrl),
                Times.Once());
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_InterviewerArgumentIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.UpdateAsync(null)));
        }

        [Fact]
        public async Task TestUpdateAsync_InterviewerExists_ReturnsInterviewerAsync()
        {
            // Arrange
            _mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(new Uri(_capiInterviewersApi, InterviewerId),
                                        It.Is<EditCapiInterviewer>(uci => uci.FirstName == _interviewer.FirstName)))
                .Returns(CreateTask(HttpStatusCode.OK, SerializedCapiInterviewer()));

            // Act
            var actual = await _target.UpdateAsync(_interviewer);

            // Assert
            Assert.Equal(_interviewer.InterviewerId, actual.InterviewerId);
            Assert.Equal(_interviewer.FirstName, actual.FirstName);
        }

        #endregion

        #region QueryAsync

        [Fact]
        public async Task TestQueryAsync_ServerReturnsQuery_ReturnsListWithInterviewersAsync()
        {
            // Arrange
            var expectedInterviewers = new Interviewer[]
            {
                new Interviewer { InterviewerId = "TestInterviewer" },
                new Interviewer { InterviewerId = "AnotherTestInterviewer" }
            };
            _mockedHttpClient
                .Setup(client => client.GetAsync(_capiInterviewersApi))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedInterviewers))));

            // Act
            var actualInterviewers = await _target.QueryAsync();

            // Assert
            Assert.Equal(2, actualInterviewers.Count());
            Assert.Equal(expectedInterviewers[0].InterviewerId, actualInterviewers.ToArray()[0].InterviewerId);
            Assert.Equal(expectedInterviewers[1].InterviewerId, actualInterviewers.ToArray()[1].InterviewerId);
        }

        #endregion

        #region ChangePasswordAsync

        [Fact]
        public void TestChangePasswordAsync_InterviewerIsNull_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.ChangePasswordAsync(null, string.Empty)));
        }

        [Fact]
        public async Task TestChangePasswordAsync_ServerChangesPassword_ReturnsInterviewerAsync()
        {
            // Arrange
            var expectedUrl = new Uri(_capiInterviewersApi, InterviewerId);
            _mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(expectedUrl, It.IsAny<object>()))
                .Returns(CreateTask(HttpStatusCode.OK, SerializedCapiInterviewer()));

            // Act
            var actual = await _target.ChangePasswordAsync(_interviewer, Password);

            // Assert
            Assert.Equal(_interviewer.InterviewerId, actual.InterviewerId);
            _mockedHttpClient.Verify(
                h =>
                    h.PutAsJsonAsync(expectedUrl, It.Is<ResetPasswordModel>(o => o.Password  == Password )),
                Times.Once());
        }

        #endregion

        #region QueryOfficesOfInterviewerAsync

        [Fact]
        public async Task TestQueryOfficesOfInterviewerAsync_ServerReturnsQuery_ReturnsListWithFieldworkOfficesAsync()
        {
            // Arrange
            var expectedFieldworkOffices = new[]
            {
                "Amsterdam",
                "Barcelona",
                "Headquarters"
            };

            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(_capiInterviewersApi, $"{InterviewerId}/Offices")))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(expectedFieldworkOffices))));

            // Act
            var actualFieldworkOffices = await _target.QueryOfficesOfInterviewerAsync(InterviewerId);

            // Assert
            var fieldworkOffices = actualFieldworkOffices as string[] ?? actualFieldworkOffices.ToArray();
            Assert.Equal(expectedFieldworkOffices[0], fieldworkOffices[0]);
            Assert.Equal(expectedFieldworkOffices[1], fieldworkOffices[1]);
            Assert.Equal(expectedFieldworkOffices[2], fieldworkOffices[2]);
            Assert.Equal(3, fieldworkOffices.Count());
        }

        #endregion

        #region AddInterviewerToFieldworkOfficesAsync

        [Fact]
        public async Task TestAddInterviewerToFieldworkOfficesAsync_WhenExecuted_CallsClientPostAsJsonAsyncWithCorrectArgsAsync()
        {
            // Arrange
            var expectedUrl = new Uri(_capiInterviewersApi, $"{InterviewerId}/Offices");
            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(expectedUrl, It.Is<InterviewerFieldworkOfficeModel>(f => f.OfficeId == FieldworkOfficeId)))
                .Returns(CreateTask(HttpStatusCode.OK));

            // Act
            await _target.AddInterviewerToFieldworkOfficesAsync(InterviewerId, FieldworkOfficeId);

            _mockedHttpClient.Verify(
                h =>
                    h.PostAsJsonAsync(expectedUrl, It.Is<InterviewerFieldworkOfficeModel>(f => f.OfficeId == FieldworkOfficeId)),
                Times.Once());
        }

        #endregion

        #region RemoveInterviewerFromFieldworkOfficesAsync

        [Fact]
        public async Task TestRemoveInterviewerFromFieldworkOfficesAsync_WhenExecuted_CallsClientPostAsJsonAsyncWithCorrectArgsAsync()
        {
            // Arrange
            var expectedUrl = new Uri(_capiInterviewersApi, $"{InterviewerId}/Offices/{FieldworkOfficeId}");

            _mockedHttpClient
                .Setup(client => client.DeleteAsync(expectedUrl))
                .Returns(CreateTask(HttpStatusCode.OK));

            // Act
            await _target.RemoveInterviewerFromFieldworkOfficesAsync(InterviewerId, FieldworkOfficeId);

            // Assert
            _mockedHttpClient.Verify(
                h =>
                    h.DeleteAsync(expectedUrl),
                Times.Once());
        }

        #endregion

        #region GetInterviewersWorklogDownloadLinkAsync

        [Fact]
        public async Task TestQueryLogsAsync_ReturnsCorrectLogsLink()
        {
            // Arrange
            var query = new LogQueryModel
            {
                From = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1)),
                To = DateTime.UtcNow
            };

            _mockedHttpClient
               .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, "InterviewersWorklog"), It.Is<LogQueryModel>(
                   q => q.From == query.From && q.To == query.To)))
               .Returns(
                Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { ActivityId = ActivityId }))
                    })).Verifiable();

            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"BackgroundActivities/{ActivityId}")))
                .Returns(Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new {
                            DownloadDataUrl = LogsLink1,
                            ActivityId = ActivityId,
                            Status = 2 /* Succeeded */ }))
                    })).Verifiable();

            // Act
            // Test it using the model
            var result = await _target.QueryLogsAsync(query);

            // Assert
            _mockedHttpClient.Verify();
            Assert.Equal(LogsLink1, result);

        }

        #endregion

        #region private methods
        private StringContent SerializedCapiInterviewer()
        {
            return new StringContent(JsonConvert.SerializeObject(_capiInterviewer));
        }
        #endregion
    }
}

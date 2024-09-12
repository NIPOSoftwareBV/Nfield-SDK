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

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldCapiInterviewersService"/>
    /// </summary>
    public class NfieldCapiInterviewersServiceTests : NfieldServiceTestsBase
    {
        private const string CapiInterviewersEndpoint = "CapiInterviewers/";
        private const string CapiInterviewersFieldworkOfficesRoutePart = "FieldworkOffices";
        private const string CapiInterviewersOfficesRoutePart = "Offices";
        private const string InterviewerId = "Interviewer X";

        private readonly Uri _capiInterviewersApi;
        private readonly CreateCapiInterviewer _createCapiInterviewer;
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

            _createCapiInterviewer = new CreateCapiInterviewer { InterviewerId = InterviewerId, UserName = "User X" };
            _capiInterviewer = new CapiInterviewer {
                InterviewerId = _createCapiInterviewer.InterviewerId,
                UserName = _createCapiInterviewer.UserName
            };
        }

        #region AddAsync

        [Fact]
        public async Task TestAddAsync_ServerAcceptsInterviewer_ReturnsInterviewerAsync()
        {
            // Arrange
            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(_capiInterviewersApi,
                            It.Is<CreateCapiInterviewer>(nci => nci.UserName == _createCapiInterviewer.UserName)))
                .Returns(CreateTask(HttpStatusCode.OK, SerializedCapiInterviewer()));

            // Act
            var actual = await _target.AddAsync(_createCapiInterviewer);

            // Assert
            Assert.Equal(_createCapiInterviewer.UserName, actual.UserName);
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
            await _target.RemoveAsync(_capiInterviewer);

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
                                        It.Is<CapiInterviewer>(uci => uci.FirstName == _capiInterviewer.FirstName)))
                .Returns(CreateTask(HttpStatusCode.OK, SerializedCapiInterviewer()));

            // Act
            var actual = await _target.UpdateAsync(_capiInterviewer);

            // Assert
            Assert.Equal(_capiInterviewer.InterviewerId, actual.InterviewerId);
            Assert.Equal(_capiInterviewer.FirstName, actual.FirstName);
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
            var target = new NfieldCapiInterviewersService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.ChangePasswordAsync(null, string.Empty)));
        }

        [Fact]
        public void TestChangePasswordAsync_ServerChangesPassword_ReturnsInterviewer()
        {
            const string Password = "Password";
            const string InterviewerId = "Interviewer X";
            var interviewer = new CapiInterviewer { InterviewerId = InterviewerId };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(new Uri(_capiInterviewersApi, InterviewerId), It.IsAny<object>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(interviewer))));

            var target = new NfieldCapiInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.ChangePasswordAsync(interviewer, Password).Result;

            Assert.Equal(interviewer.InterviewerId, actual.InterviewerId);
        }

        #endregion

        #region AddInterviewerToFieldworkOfficesAsync

        [Fact]
        public async Task TestAddInterviewerToFieldworkOfficesAsync_CallsToApiProperly()
        {
            // Arrange
            const string InterviewerId = "InterviewerId1";
            const string OfficeId = "OfficeID1";
            var endpointUri= new Uri(_capiInterviewersApi, $"{InterviewerId}/{CapiInterviewersFieldworkOfficesRoutePart}/{OfficeId}");
            _mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(endpointUri, string.Empty))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            // Act
            await _target.AddInterviewerToFieldworkOfficesAsync(InterviewerId, OfficeId);

            // Assert
            _mockedHttpClient.Verify();
        }

        #endregion

        #region RemoveInterviewerFromFieldworkOfficesAsync

        [Fact]
        public async Task TestRemoveInterviewerFromFieldworkOfficesAsync_CallsToApiProperly()
        {
            // Arrange
            const string InterviewerId = "InterviewerId1";
            const string OfficeId = "OfficeID1";
            var endpointUri = new Uri(_capiInterviewersApi, $"{InterviewerId}/{CapiInterviewersFieldworkOfficesRoutePart}/{OfficeId}");
            _mockedHttpClient
                .Setup(client => client.DeleteAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            // Act
            await _target.RemoveInterviewerFromFieldworkOfficesAsync(InterviewerId, OfficeId);

            // Assert
            _mockedHttpClient.Verify();
        }

        #endregion

        #region RemoveInterviewerFromFieldworkOfficesAsync

        [Fact]
        public async Task TestQueryFieldworkOfficesAsync_CallsToApiProperly()
        {
            // Arrange
            const string InterviewerId = "InterviewerId1";
            const string OfficeId = "OfficeID1";
            var endpointUri = new Uri(_capiInterviewersApi, $"{InterviewerId}/{CapiInterviewersFieldworkOfficesRoutePart}");
            _mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(new[] { OfficeId} )))).Verifiable();

            // Act
            var result = await _target.QueryFieldworkOfficesAsync(InterviewerId);

            // Assert
            _mockedHttpClient.Verify();
            Assert.Single(result);
            Assert.Equal(OfficeId, result.First());
        }

        #endregion

        #region AddInterviewerToOfficesAsync

        [Fact]
        public async Task TestAddInterviewerToOfficesAsync_CallsToApiProperly()
        {
            // Arrange
            const string InterviewerId = "InterviewerId1";
            const string OfficeId = "OfficeID1";
            var endpointUri = new Uri(_capiInterviewersApi, $"{InterviewerId}/{CapiInterviewersOfficesRoutePart}/{OfficeId}");
            _mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(endpointUri, string.Empty))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            // Act
            await _target.AddInterviewerToOfficeAsync(InterviewerId, OfficeId);

            // Assert
            _mockedHttpClient.Verify();
        }

        #endregion

        #region RemoveInterviewerFromOfficesAsync

        [Fact]
        public async Task TestRemoveInterviewerFromOfficesAsync_CallsToApiProperly()
        {
            // Arrange
            const string InterviewerId = "InterviewerId1";
            const string OfficeId = "OfficeID1";
            var endpointUri = new Uri(_capiInterviewersApi, $"{InterviewerId}/{CapiInterviewersOfficesRoutePart}/{OfficeId}");
            _mockedHttpClient
                .Setup(client => client.DeleteAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK)).Verifiable();

            // Act
            await _target.RemoveInterviewerFromOfficeAsync(InterviewerId, OfficeId);

            // Assert
            _mockedHttpClient.Verify();
        }

        #endregion

        #region RemoveInterviewerFromOfficesAsync

        [Fact]
        public async Task TestQueryOfficesAsync_CallsToApiProperly()
        {
            // Arrange
            const string InterviewerId = "InterviewerId1";
            const string OfficeId = "OfficeID1";
            var endpointUri = new Uri(_capiInterviewersApi, $"{InterviewerId}/{CapiInterviewersOfficesRoutePart}");
            _mockedHttpClient
                .Setup(client => client.GetAsync(endpointUri))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(new[] { OfficeId })))).Verifiable();

            // Act
            var result = await _target.QueryOfficesAsync(InterviewerId);

            // Assert
            _mockedHttpClient.Verify();
            Assert.Single(result);
            Assert.Equal(OfficeId, result.First());
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

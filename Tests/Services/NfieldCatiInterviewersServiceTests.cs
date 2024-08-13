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
using Xunit;

namespace Nfield.Services
{
    public class NfieldCatiInterviewersServiceTests : NfieldServiceTestsBase
    {
        #region AddAsync

        [Fact]
        public void TestAddAsync_ServerAcceptsCatiInterviewer_ReturnsCatiInterviewer()
        {
            var catiInterviewer = new CatiInterviewer { UserName = "User X" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(catiInterviewer));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, "CatiInterviewers/"), catiInterviewer))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldCatiInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddAsync(catiInterviewer).Result;

            Assert.Equal(catiInterviewer.UserName, actual.UserName);
        }

        #endregion

        #region RemoveAsync

        [Fact]
        public void TestRemoveAsync_CatiInterviewerIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldCatiInterviewersService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(null)));
        }

        [Fact]
        public void TestRemoveAsync_ServerRemovedCatiInterviewer_DoesNotThrow()
        {
            const string InterviewerId = "Interviewer X";
            var catiInterviewer = new CatiInterviewer { InterviewerId = InterviewerId };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, "CatiInterviewers/" + InterviewerId)))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldCatiInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // assert: no throw
            target.RemoveAsync(catiInterviewer).Wait();
        }

        #endregion

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithCatiInterviewers()
        {
            var expectedCatiInterviewers = new CatiInterviewer[]
            {
                new CatiInterviewer{InterviewerId = "TestInterviewer"},
                new CatiInterviewer{InterviewerId = "AnotherTestInterviewer"}
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "CatiInterviewers/")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedCatiInterviewers))));

            var target = new NfieldCatiInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualInterviewers = target.QueryAsync().Result;
            Assert.Equal(expectedCatiInterviewers[0].InterviewerId, actualInterviewers.ToArray()[0].InterviewerId);
            Assert.Equal(expectedCatiInterviewers[1].InterviewerId, actualInterviewers.ToArray()[1].InterviewerId);
            Assert.Equal(2, actualInterviewers.Count());
        }

        #endregion

        #region ChangePasswordAsync

        [Fact]
        public void TestChangePasswordAsync_CatiInterviewerIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldCatiInterviewersService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.ChangePasswordAsync(null, string.Empty)));
        }

        [Fact]
        public void TestChangePasswordAsync_ServerChangesPassword_ReturnsCatiInterviewer()
        {
            const string Password = "Password";
            const string InterviewerId = "Interviewer X";
            var catiInterviewer = new CatiInterviewer { InterviewerId = InterviewerId };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, "CatiInterviewers/" + InterviewerId), It.IsAny<object>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(catiInterviewer))));

            var target = new NfieldCatiInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.ChangePasswordAsync(catiInterviewer, Password).Result;

            Assert.Equal(catiInterviewer.InterviewerId, actual.InterviewerId);
        }

        #endregion
    }
}

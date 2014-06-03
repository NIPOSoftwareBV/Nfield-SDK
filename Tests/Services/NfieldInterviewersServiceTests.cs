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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using Nfield.Infrastructure;
using Nfield.Services.Implementation;
using Xunit;
using Nfield.Models;
using Moq;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Linq;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldInterviewersService"/>
    /// </summary>
    public class NfieldInterviewersServiceTests : NfieldServiceTestsBase
    {
        #region AddAsync

        [Fact]
        public void TestAddAsync_ServerAcceptsInterviewer_ReturnsInterviewer()
        {
            var interviewer = new Interviewer { UserName = "User X" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(interviewer));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(ServiceAddress + "interviewers/", interviewer))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddAsync(interviewer).Result;

            Assert.Equal(interviewer.UserName, actual.UserName);
        }

        #endregion

        #region RemoveAsync

        [Fact]
        public void TestRemoveAsync_InterviewerIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldInterviewersService();
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
                .Setup(client => client.DeleteAsync(ServiceAddress + "interviewers/" + InterviewerId))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.DoesNotThrow(() => target.RemoveAsync(interviewer).Wait());
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_InterviewerArgumentIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldInterviewersService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync(null)));
        }

        [Fact]
        public void TestUpdateAsync_InterviewerExists_ReturnsInterviewer()
        {
            const string InterviewerId = "Interviewer X";
            var interviewer = new Interviewer
            { 
                InterviewerId = InterviewerId,  
                FirstName = "XXX" 
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(ServiceAddress + "interviewers/" + InterviewerId, It.IsAny<UpdateInterviewer>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(interviewer))));

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.UpdateAsync(interviewer).Result;

            Assert.Equal(interviewer.FirstName, actual.FirstName);
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
                .Setup(client => client.GetAsync(ServiceAddress + "interviewers/"))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedInterviewers))));

            var target = new NfieldInterviewersService();
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
            var target = new NfieldInterviewersService();
            Assert.Throws(typeof(ArgumentNullException), () => UnwrapAggregateException(target.ChangePasswordAsync(null, string.Empty)));
        }

        [Fact]
        public void TestChangePasswordAsync_ServerChangesPassword_ReturnsInterviewer()
        {
            const string Password = "Password";
            const string InterviewerId = "Interviewer X";
            var interviewer = new Interviewer {InterviewerId = InterviewerId };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(ServiceAddress + "interviewers/" + InterviewerId, It.IsAny<object>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(interviewer))));

            var target = new NfieldInterviewersService();
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
                    string.Format(CultureInfo.InvariantCulture, "{0}interviewers/{1}/Offices", ServiceAddress,
                        interviewerId))
                )
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(expectedFieldworkOffices))));

            var target = new NfieldInterviewersService();
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

            var expectedUrl = string.Format(CultureInfo.InvariantCulture, "{0}interviewers/{1}/Offices",
                ServiceAddress,
                interviewerId);

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<InterviewerFieldworkOfficeModel>()))
                .Returns(CreateTask(HttpStatusCode.OK));
            

            var target = new NfieldInterviewersService();
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

            var expectedUrl = string.Format(CultureInfo.InvariantCulture, "{0}interviewers/{1}/Offices/{2}",
                ServiceAddress,
                interviewerId,
                fieldworkOfficeId);

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient
                .Setup(client => client.DeleteAsync(It.IsAny<string>()))
                .Returns(CreateTask(HttpStatusCode.OK));


            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.RemoveInterviewerFromFieldworkOfficesAsync(interviewerId, fieldworkOfficeId).Wait();

            

            mockedHttpClient.Verify(
                h =>
                    h.DeleteAsync(expectedUrl),
                Times.Once());
        }

        #endregion


    }
}

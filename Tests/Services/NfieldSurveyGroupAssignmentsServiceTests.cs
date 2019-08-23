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
using Nfield.SDK.Models;
using Nfield.SDK.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace Nfield.Services
{
    public class NfieldSurveyGroupAssignmentsServiceTests : NfieldServiceTestsBase
    {
        private readonly NfieldSurveyGroupAssignmentsService _target;

        readonly Mock<INfieldHttpClient> _mockedHttpClient;

        public NfieldSurveyGroupAssignmentsServiceTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldSurveyGroupAssignmentsService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);
        }

        [Fact]
        public async Task CanGetAllLocalAssignments()
        {
            const int surveyGroupId = 2;

            var expectedAssignments = new[]
            {
                new SurveyGroupNativeAssignment
                {
                    SurveyGroupId = surveyGroupId,
                    NativeIdentityId = Guid.NewGuid().ToString(),
                    DateAdded = DateTime.UtcNow
                },

                new SurveyGroupNativeAssignment
                {
                    SurveyGroupId = surveyGroupId,
                    NativeIdentityId = Guid.NewGuid().ToString(),
                    DateAdded = DateTime.UtcNow
                }
            };

            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"SurveyGroups/{surveyGroupId}/Local-Assignments")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedAssignments))));

            var actualAssignments = await _target.GetLocalAssignmentsAsync(surveyGroupId);

            Assert.Equal(2, actualAssignments.Count());
            Assert.Equal(expectedAssignments[0].NativeIdentityId, actualAssignments.ToArray()[0].NativeIdentityId);
            Assert.Equal(expectedAssignments[0].DateAdded, actualAssignments.ToArray()[0].DateAdded);
            Assert.Equal(expectedAssignments[0].SurveyGroupId, actualAssignments.ToArray()[0].SurveyGroupId);
            Assert.Equal(expectedAssignments[1].NativeIdentityId, actualAssignments.ToArray()[1].NativeIdentityId);
            Assert.Equal(expectedAssignments[1].DateAdded, actualAssignments.ToArray()[1].DateAdded);
            Assert.Equal(expectedAssignments[1].SurveyGroupId, actualAssignments.ToArray()[1].SurveyGroupId);
        }

        [Fact]
        public async Task CanGetAllDirectoryAssignments()
        {
            const int surveyGroupId = 2;

            var expectedAssignments = new[]
            {
                new SurveyGroupDirectoryAssignment
                {
                    SurveyGroupId = surveyGroupId,
                    TenantId = Guid.NewGuid(),
                    ObjectId = Guid.NewGuid(),
                    ObjectType = AadObjectType.SecurityGroup,
                    DateAdded = DateTime.UtcNow
                },

                new SurveyGroupDirectoryAssignment
                {
                    SurveyGroupId = surveyGroupId,
                    TenantId = Guid.NewGuid(),
                    ObjectId = Guid.NewGuid(),
                    ObjectType = AadObjectType.User,
                    DateAdded = DateTime.UtcNow
                }
            };

            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"SurveyGroups/{surveyGroupId}/Directory-Assignments")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedAssignments))));

            var actualAssignments = await _target.GetDirectoryAssignmentsAsync(surveyGroupId);

            Assert.Equal(2, actualAssignments.Count());
            Assert.Equal(expectedAssignments[0].TenantId, actualAssignments.ToArray()[0].TenantId);
            Assert.Equal(expectedAssignments[0].ObjectId, actualAssignments.ToArray()[0].ObjectId);
            Assert.Equal(expectedAssignments[0].ObjectType, actualAssignments.ToArray()[0].ObjectType);
            Assert.Equal(expectedAssignments[0].DateAdded, actualAssignments.ToArray()[0].DateAdded);
            Assert.Equal(expectedAssignments[0].SurveyGroupId, actualAssignments.ToArray()[0].SurveyGroupId);
            Assert.Equal(expectedAssignments[1].TenantId, actualAssignments.ToArray()[1].TenantId);
            Assert.Equal(expectedAssignments[1].ObjectId, actualAssignments.ToArray()[1].ObjectId);
            Assert.Equal(expectedAssignments[1].ObjectType, actualAssignments.ToArray()[1].ObjectType);
            Assert.Equal(expectedAssignments[1].DateAdded, actualAssignments.ToArray()[1].DateAdded);
            Assert.Equal(expectedAssignments[1].SurveyGroupId, actualAssignments.ToArray()[1].SurveyGroupId);
        }

        [Fact]
        public async Task CanAddLocalAssignment()
        {
            const int surveyGroupId = 2;

            var nativeIdentityId = Guid.NewGuid().ToString();

            var expectedAssignment = new SurveyGroupNativeAssignment
            {
                NativeIdentityId = nativeIdentityId,
                SurveyGroupId = surveyGroupId
            };

            _mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"SurveyGroups/{surveyGroupId}/Assign-Local"), nativeIdentityId))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedAssignment))));

            var actualAssignment = await _target.AssignLocalAsync(surveyGroupId, nativeIdentityId);

            Assert.Equal(nativeIdentityId, actualAssignment.NativeIdentityId);
            Assert.Equal(surveyGroupId, actualAssignment.SurveyGroupId);
        }

        [Fact]
        public async Task CanAddDirectoryAssignment()
        {
            const int surveyGroupId = 2;

            var directoryIdentity = new DirectoryIdentityModel
            {
                TenantId = Guid.NewGuid(),
                ObjectId = Guid.NewGuid(),
                ObjectType = AadObjectType.User
            };

            var expectedAssignment = new SurveyGroupDirectoryAssignment
            {
                TenantId = directoryIdentity.TenantId,
                ObjectId = directoryIdentity.ObjectId,
                ObjectType = directoryIdentity.ObjectType,
                SurveyGroupId = surveyGroupId
            };

            _mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"SurveyGroups/{surveyGroupId}/Assign-Directory"), directoryIdentity))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedAssignment))));

            var actualAssignment = await _target.AssignDirectoryAsync(surveyGroupId, directoryIdentity);

            Assert.Equal(expectedAssignment.TenantId, actualAssignment.TenantId);
            Assert.Equal(expectedAssignment.ObjectId, actualAssignment.ObjectId);
            Assert.Equal(expectedAssignment.ObjectType, actualAssignment.ObjectType);
            Assert.Equal(expectedAssignment.SurveyGroupId, actualAssignment.SurveyGroupId);
        }

        [Fact]
        public async Task DeleteLocalAssignment_ReturnsNoError()
        {
            const int surveyGroupId = 2;

            var nativeIdentityId = Guid.NewGuid().ToString();

            _mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"SurveyGroups/{surveyGroupId}/Unassign-Local"), nativeIdentityId))
                .Returns(CreateTask(HttpStatusCode.NoContent));

            await _target.UnassignLocalAsync(surveyGroupId, nativeIdentityId);
        }

        [Fact]
        public async Task DeleteDirectoryAssignment_ReturnsNoError()
        {
            const int surveyGroupId = 2;

            var directoryIdentity = new DirectoryIdentityModel
            {
                TenantId = Guid.NewGuid(),
                ObjectId = Guid.NewGuid(),
                ObjectType = AadObjectType.User
            };

            _mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(new Uri(ServiceAddress, $"SurveyGroups/{surveyGroupId}/Unassign-Directory"), directoryIdentity))
                .Returns(CreateTask(HttpStatusCode.NoContent));

            await _target.UnassignDirectoryAsync(surveyGroupId, directoryIdentity);
        }

    }
}

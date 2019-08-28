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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Nfield.Services
{

    public class NfieldLocalUserServiceTests : NfieldServiceTestsBase
    {
        private readonly NfieldLocalUserService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;

        public NfieldLocalUserServiceTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldLocalUserService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);
        }

        [Fact]
        public async Task CreateThrows_WhenArgumentNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _target.CreateAsync(null));
        }

        [Fact]
        public async Task UpdateThrows_WhenArgumentNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _target.UpdateAsync("test", null));
        }

        [Fact]
        public async Task CanCreateNewUser()
        {
            var expectedUser = new LocalUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user1",
                Email = "user@neverland.com",
                UserRole = "RegularUser"
            };

            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, "LocalUsers"), It.IsAny<NewLocalUser>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedUser))));

            var result = await _target.CreateAsync(new NewLocalUser
            {
                Email = "user@neverland.com",
                UserName = "user1",
                UserRole = "RegularUser",
                Password = "Secret"                
            });

            Assert.NotNull(result);
            Assert.Equal("user@neverland.com", result.Email);
            Assert.Equal("RegularUser", result.UserRole);
            Assert.Equal("user1", result.UserName);
        }

        [Fact]
        public async Task CanCreateModifyAndDeleteUser()
        {
            var createdUser = new LocalUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "user1",
                Email = "user@neverland.com",
                UserRole = "RegularUser"
            };

            var updatedUser = new LocalUser
            {
                Id = createdUser.Id,
                UserName = "user1",
                Email = "user@neverland.com",
                UserRole = "RegularUser",
                FirstName = "Peter",
                LastName = "Pan"
            };

            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, "LocalUsers"), It.IsAny<NewLocalUser>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(createdUser))));
            _mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(new Uri(ServiceAddress, $"LocalUsers/{createdUser.Id}"), It.IsAny<LocalUserValues>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(updatedUser))));
            _mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, $"LocalUsers/{createdUser.Id}")))
                .Returns(CreateTask(HttpStatusCode.OK));

            var result = await _target.CreateAsync(new NewLocalUser
            {
                UserName = "user1",
                Email = "user@neverland.com",
                UserRole = "RegularUser"
            });

            Assert.NotNull(result);
            Assert.Equal("user@neverland.com", result.Email);
            Assert.Equal("RegularUser", result.UserRole);
            Assert.Equal("user1", result.UserName);
            Assert.Null(result.FirstName);
            Assert.Null(result.LastName);

            // modify survey group object we got back, then post it
            result.FirstName = "Peter";
            result.LastName = "Pan";

            result = await _target.UpdateAsync(result.Id, result);

            Assert.NotNull(result);
            Assert.Equal("user@neverland.com", result.Email);
            Assert.Equal("RegularUser", result.UserRole);
            Assert.Equal("user1", result.UserName);
            Assert.Equal("Peter", result.FirstName);
            Assert.Equal("Pan", result.LastName);

            await _target.DeleteAsync(result.Id);
        }

        [Fact]
        public async Task CanGetAllUsers()
        {
            var user1Id = Guid.NewGuid().ToString();
            var user2Id = Guid.NewGuid().ToString();

            var expectedUsers = new[]
            {
                new LocalUser
                {
                    Id = user1Id,
                    UserName = "user1",
                    Email = "p.pan@neverland.com",
                    UserRole = "RegularUser"
                },
                new LocalUser
                {
                    Id = user2Id,
                    UserName = "user2",
                    Email = "w.darling@neverland.com",
                    UserRole = "Scripter"
                }
            };

            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "LocalUsers")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedUsers))));

            var actualUsers = await _target.GetAllAsync();

            Assert.Equal(2, actualUsers.Count());
            var user1 = actualUsers.FirstOrDefault(u => u.Id == user1Id);
            Assert.Equal("user1", user1.UserName);
            var user2 = actualUsers.FirstOrDefault(u => u.Id == user2Id);
            Assert.Equal("user2", user2.UserName);
        }
    }
}

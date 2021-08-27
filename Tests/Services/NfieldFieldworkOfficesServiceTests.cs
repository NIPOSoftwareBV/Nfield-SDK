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
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Linq;
using static Nfield.Services.Implementation.NfieldFieldworkOfficesService;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldFieldworkOfficesService"/>
    /// </summary>
    public class NfieldFieldworkOfficesServiceTests : NfieldServiceTestsBase
    {
        #region QueryAsync

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithFieldworkOffices()
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.NotFound;
            var expectedFieldworkOffices = new FieldworkOffice[]
            { new FieldworkOffice{OfficeId = "TestOfficeId"},
              new FieldworkOffice{OfficeId = "AnotherTestOfficeId"}
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = new Mock<INfieldHttpClient>();
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "offices/")))
                .Returns(
                    Task.Factory.StartNew(
                        () =>
                            new HttpResponseMessage(httpStatusCode)
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(expectedFieldworkOffices))
                            }));
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(ServiceAddress);


            var target = new NfieldFieldworkOfficesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualFieldworkOffices = target.QueryAsync().Result;
            Assert.Equal(expectedFieldworkOffices[0].OfficeId, actualFieldworkOffices.ToArray()[0].OfficeId);
            Assert.Equal(expectedFieldworkOffices[1].OfficeId, actualFieldworkOffices.ToArray()[1].OfficeId);
            Assert.Equal(2, actualFieldworkOffices.Count());
        }

        #endregion

        #region AddAsync

        [Fact]
        public void TestAddAsync_OfficeIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldFieldworkOfficesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.AddAsync(null)));
        }

        [Fact]
        public void TestAddAsync_OfficeIdHasValue_ThrowsArgumentException()
        {
            var target = new NfieldFieldworkOfficesService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.AddAsync(
                new FieldworkOffice
                {
                    OfficeId = "Some value"
                })));
        }

        [Fact]
        public void TestAddAsync_ServerAcceptsOffice_ReturnsOffice()
        {
            var office = new FieldworkOffice { OfficeName = "Office X", Description = "New office"};
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(office));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, "offices/"), office))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldFieldworkOfficesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddAsync(office).Result;

            Assert.Equal(office.OfficeName, actual.OfficeName);
            Assert.Equal(office.Description, actual.Description);
        }

        #endregion

        #region RemoveAsync

        [Fact]
        public void TestRemoveAsync_OfficeIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldFieldworkOfficesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(null)));
        }

        [Fact]
        public void TestRemoveAsync_ServerRemovedFieldworkOffice_DoesNotThrow()
        {
            const string removeOfficeId = "Office X";
            var office = new FieldworkOffice { OfficeId = removeOfficeId };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, "offices/" + removeOfficeId)))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldFieldworkOfficesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // assert: no throw
            target.RemoveAsync(office).Wait();
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_FieldworkOfficeArgumentIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldFieldworkOfficesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync(null)));
        }

        [Fact]
        public void TestUpdateAsync_OfficeIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldFieldworkOfficesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync(null)));
        }

        [Fact]
        public void TestUpdateAsync_OfficeIdIsNull_ThrowsArgumentException()
        {
            var target = new NfieldFieldworkOfficesService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.UpdateAsync(
                new FieldworkOffice())));
        }

        [Fact]
        public void TestUpdateAsync_ServerAcceptsOffice_ReturnsOffice()
        {
            var office = new FieldworkOffice { OfficeId = "ID", OfficeName = "Office X Name", Description = "Updated Description" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(office));

            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(new Uri(ServiceAddress, "offices/" + office.OfficeId),
                    It.Is<UpdateOffice>(u => u.OfficeName == office.OfficeName && u.Description == office.Description)))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldFieldworkOfficesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.UpdateAsync(office).Result;

            Assert.Equal(office.OfficeId, actual.OfficeId);
            Assert.Equal(office.OfficeName, actual.OfficeName);
            Assert.Equal(office.Description, actual.Description);
        }

        #endregion
    }
}

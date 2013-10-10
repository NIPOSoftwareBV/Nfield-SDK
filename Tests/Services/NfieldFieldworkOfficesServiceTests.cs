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
                .Setup(client => client.GetAsync(ServiceAddress + "offices/"))
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
                .Returns(new Uri(ServiceAddress));

            
            var target = new NfieldFieldworkOfficesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualFieldworkOffices = target.QueryAsync().Result;
            Assert.Equal(expectedFieldworkOffices[0].OfficeId, actualFieldworkOffices.ToArray()[0].OfficeId);
            Assert.Equal(expectedFieldworkOffices[1].OfficeId, actualFieldworkOffices.ToArray()[1].OfficeId);
            Assert.Equal(2, actualFieldworkOffices.Count());
        }

        #endregion
    }
}

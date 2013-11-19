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
    /// Tests for <see cref="NfieldBackgroundTasksService"/>
    /// </summary>
    public class NfieldBackgroundTasksServiceTests : NfieldServiceTestsBase
    {
        #region QueryAsync

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithBackgroundTasks()
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.NotFound;
            var expectedBackgroundTasks = new[]
            { new BackgroundTask{Id = "TestId"},
              new BackgroundTask{Id = "AnotherTestId"}
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = new Mock<INfieldHttpClient>();
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "backgroundtasks/"))
                .Returns(
                    Task.Factory.StartNew(
                        () =>
                            new HttpResponseMessage(httpStatusCode)
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(expectedBackgroundTasks))
                            }));
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));


            var target = new NfieldBackgroundTasksService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualBackgroundTasks = target.QueryAsync().Result;
            Assert.Equal(expectedBackgroundTasks[0].Id, actualBackgroundTasks.ToArray()[0].Id);
            Assert.Equal(expectedBackgroundTasks[1].Id, actualBackgroundTasks.ToArray()[1].Id);
            Assert.Equal(2, actualBackgroundTasks.Count());
        }

        #endregion
    }
}

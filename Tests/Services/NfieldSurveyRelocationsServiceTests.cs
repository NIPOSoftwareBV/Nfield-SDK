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
using System.Linq;
using System.Net;
using System.Net.Http;
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyRelocationsService"/>
    /// </summary>
    public class NfieldSurveyRelocationsServiceTests : NfieldServiceTestsBase
    {
        const string SurveyId = "MySurvey";

        #region AddAsync

        [Fact]
        public void TestUpdateAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyRelocationsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync(null, new SurveyRelocation())));
        }

        [Fact]
        public void TestUpdateAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveyRelocationsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.UpdateAsync("", new SurveyRelocation())));
        }

        [Fact]
        public void TestUpdateAsync_ServerAcceptsRelocations_ReturnsOk()
        {
            var relocation = new SurveyRelocation {Reason = "reason X", Url = "url X"};
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(relocation));
            mockedHttpClient
                .Setup(
                    client => client.PutAsJsonAsync(new Uri(ServiceAddress, "Surveys/" + SurveyId + "/Relocations"), relocation))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldSurveyRelocationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.DoesNotThrow(() => target.UpdateAsync(SurveyId, relocation).Wait());

        }

        [Fact]
        public void TestPutAsync_Always_CallsCorrectURI()
        {
            var relocation = new SurveyRelocation { Reason = "reason X", Url = "url X" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(It.IsAny<Uri>(), It.IsAny<SurveyRelocation>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(It.IsAny<SurveyRelocation>()))));

            var target = new NfieldSurveyRelocationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.UpdateAsync(SurveyId, relocation).Wait();

            mockedHttpClient
                .Verify(
                    client =>
                        client.PutAsJsonAsync(new Uri(ServiceAddress, "Surveys/" + SurveyId + "/Relocations"), It.IsAny<SurveyRelocation>()),
                    Times.Once());
        }

        #endregion

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyRelocationsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.QueryAsync(null)));
        }

        [Fact]
        public void TestQueryAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveyRelocationsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.QueryAsync("")));
        }

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithRelocations()
        {
            var expectedRelocationss = new []
            { new SurveyRelocation { Reason = "reason X", Url = "url X"},
              new SurveyRelocation { Reason = "reason Y", Url = "url Y"}
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/" + SurveyId + "/Relocations")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedRelocationss))));

            var target = new NfieldSurveyRelocationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualRelocations = target.QueryAsync(SurveyId).Result.ToArray(); ;
            Assert.Equal(expectedRelocationss[0].Reason, actualRelocations[0].Reason);
            Assert.Equal(expectedRelocationss[0].Url, actualRelocations[0].Url);
            Assert.Equal(expectedRelocationss[1].Reason, actualRelocations[1].Reason);
            Assert.Equal(expectedRelocationss[1].Url, actualRelocations[1].Url);
            Assert.Equal(2, actualRelocations.Length);
        }

        #endregion
    }
}

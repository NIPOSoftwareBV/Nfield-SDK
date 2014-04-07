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
    /// Tests for <see cref="NfieldAddressesService"/>
    /// </summary>
    public class NfieldAddressesServiceTests : NfieldServiceTestsBase
    {
        const string SurveyId = "MySurvey";
        const string SamplingPointId = "MySamplingPoint";

        #region AddAsync

        [Fact]
        public void TestAddAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldAddressesService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.AddAsync(null, SamplingPointId, new Address())));
        }

        [Fact]
        public void TestAddAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldAddressesService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.AddAsync("", SamplingPointId, new Address())));
        }

        [Fact]
        public void TestAddAsync_SamplingPointIdIsNull_Throws()
        {
            var target = new NfieldAddressesService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.AddAsync(SurveyId, null, new Address())));
        }

        [Fact]
        public void TestAddAsync_SamplingPointIdIsEmpty_Throws()
        {
            var target = new NfieldAddressesService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.AddAsync(SurveyId, "", new Address())));
        }

        [Fact]
        public void TestAddAsync_ServerAcceptsAddress_ReturnsAddress()
        {
            var address = new Address { Details = "Language X" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(address));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(ServiceAddress + "Surveys/" + SurveyId +
                    "/SamplingPoints/" + SamplingPointId + "/Addresses", address))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldAddressesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddAsync(SurveyId, SamplingPointId, address).Result;

            Assert.Equal(address.Details, actual.Details);
        }

        #endregion

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldAddressesService();
            Assert.Throws<ArgumentNullException>(() =>
                    UnwrapAggregateException(target.QueryAsync(null, SamplingPointId)));
        }

        [Fact]
        public void TestQueryAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldAddressesService();
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.QueryAsync("", SamplingPointId)));
        }

        [Fact]
        public void TestQueryAsync_SamplingPointIdIsNull_Throws()
        {
            var target = new NfieldAddressesService();
            Assert.Throws<ArgumentNullException>(() =>
                    UnwrapAggregateException(target.QueryAsync(SurveyId, null)));
        }

        [Fact]
        public void TestQueryAsync_SamplingPointIdIsEmpty_Throws()
        {
            var target = new NfieldAddressesService();
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.QueryAsync(SurveyId, "")));
        }

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithAddresses()
        {
            var expectedAddresses = new Address[]
            { new Address{AddressId = "id1", Details = "X"},
              new Address{AddressId = "id2", Details = "Y"}
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "Surveys/" + SurveyId +
                    "/SamplingPoints/" + SamplingPointId + "/Addresses"))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedAddresses))));

            var target = new NfieldAddressesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualAddresses = target.QueryAsync(SurveyId, SamplingPointId).Result;
            Assert.Equal(expectedAddresses[0].AddressId, actualAddresses.ToArray()[0].AddressId);
            Assert.Equal(expectedAddresses[1].AddressId, actualAddresses.ToArray()[1].AddressId);
            Assert.Equal(2, actualAddresses.Count());
        }

        [Fact]
        public void TestDeleteAsync_ServerAcceptsDelete_ReturnsNoError()
        {
            const string surveyId = "SurveyId";
            const string samplingPointId = "SamplingPointId";
            const string addressId = "AddressId";

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.DeleteAsync(It.IsAny<string>()))
                                    .Returns(CreateTask(HttpStatusCode.NoContent));

            var target = new NfieldAddressesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.DeleteAsync(surveyId, samplingPointId, addressId).Wait();
        }

        #endregion
    }
}

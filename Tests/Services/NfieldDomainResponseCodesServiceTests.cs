using System;
using System.Collections.Generic;
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
    /// Tests for <see cref="NfieldDomainResponseCodesService"/>
    /// </summary>
    public class NfieldDomainResponseCodesServiceTests : NfieldServiceTestsBase
    {

        #region Query Async

        [Fact]
        public void TestQueryAsync_DomainIdIsNull_Throws()
        {
            var target = new NfieldDomainResponseCodesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.QueryAsync(null)));
        }

        [Fact]
        public void TestQueryAsync_DomainIdIsEmptyString_Throws()
        {
            var target = new NfieldDomainResponseCodesService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.QueryAsync(string.Empty)));
        }

        [Fact]
        public void TestQueryAsync_ValidDomainId_CAllsCorrectUrl()
        {
            // Arrange
            const string domainId = "domainId";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var target = new NfieldDomainResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            mockedHttpClient.Setup(client => client.GetAsync(It.IsAny<Uri>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(new List<DomainResponseCode>()))));

            // Act
            target.QueryAsync(domainId).Wait();

            // Assert
            mockedHttpClient.Verify(
                hc => hc.GetAsync(It.Is<Uri>(url => url.AbsolutePath.EndsWith("Domains/" + domainId + "/ResponseCodes/"))), Times.Once());
        }

        #endregion

        #region Query Async based on code

        [Fact]
        public void TestQueryAsyncBasedOnCode_DomainIdIsNull_Throws()
        {
            var target = new NfieldDomainResponseCodesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.QueryAsync(null, 20)));
        }

        [Fact]
        public void TestQueryAsyncBasedOnCode_DomainIdIsEmptyString_Throws()
        {
            var target = new NfieldDomainResponseCodesService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.QueryAsync(string.Empty, 20)));
        }

        [Fact]
        public void TestQueryAsyncBasedOnCode_ValidDomainId_CAllsCorrectUrl()
        {
            // Arrange
            const string domainId = "domainId";
            const int code = 20;
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var target = new NfieldDomainResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            mockedHttpClient.Setup(client => client.GetAsync(It.IsAny<Uri>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(new DomainResponseCode()))));

            // Act
            target.QueryAsync(domainId, code).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                hc.GetAsync(It.Is<Uri>(url =>
                    url.AbsolutePath.EndsWith($"Domains/{domainId}/ResponseCodes/{code}"))),
                Times.Once());
        }

        #endregion

        #region Add Async

        [Fact]
        public void TestAddAsync_DomainIdIsNull_Throws()
        {
            var target = new NfieldDomainResponseCodesService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.AddAsync(null, new DomainResponseCode())));
        }

        [Fact]
        public void TestAddAsync_DomainIdIsEmptyString_Throws()
        {
            var target = new NfieldDomainResponseCodesService();
            Assert.Throws<ArgumentException>(
                () => UnwrapAggregateException(target.AddAsync(string.Empty, new DomainResponseCode())));
        }

        [Fact]
        public void TestAddAsync_DomainResponseCodeIdIsNull_Throws()
        {
            var target = new NfieldDomainResponseCodesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.AddAsync("domainId", null)));
        }

        [Fact]
        public void TestAddAsync_ValidDomainResponseCode_CallsCorrectUrl()
        {
            // Arrange
            const string domainId = "domainId";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var responseCodeToAdd = new DomainResponseCode
            {
                ResponseCode = 15,
                Description = "Description",              
                Url = "http://www.google.com"
            };
            mockedHttpClient.Setup(client => client.PostAsJsonAsync(It.IsAny<Uri>(), It.IsAny<DomainResponseCode>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(new DomainResponseCode()))));
            var target = new NfieldDomainResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            target.AddAsync(domainId, responseCodeToAdd).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                   hc.PostAsJsonAsync(It.Is<Uri>(url => url.AbsolutePath.EndsWith("Domains/" + domainId + "/ResponseCodes/")), responseCodeToAdd),
                    Times.Once());
        }

        [Fact]
        public void TestAddAsync_ValidDomainResponseCode_ReturnsAddedResponseCode()
        {
            // Arrange
            const string domainId = "domainId";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var responseCodeToAdd = new DomainResponseCode
            {
                ResponseCode = 15,
                Description = "Description",             
                Url = "http://www.google.com"
            };
            mockedHttpClient.Setup(client => client.PostAsJsonAsync(It.IsAny<Uri>(), It.IsAny<DomainResponseCode>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(responseCodeToAdd))));
            var target = new NfieldDomainResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            var result = target.AddAsync(domainId, responseCodeToAdd).Result;

            // Assert
            Assert.Equal(responseCodeToAdd.ResponseCode, result.ResponseCode);
            Assert.Equal(responseCodeToAdd.Description, result.Description);
            Assert.Equal(responseCodeToAdd.Url, result.Url);
        }

        #endregion

        #region Update Async

        [Fact]
        public void TestUpdateAsync_DomainIdIsNull_Throws()
        {
            var target = new NfieldDomainResponseCodesService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.UpdateAsync(null, 2, new DomainResponseCodeData())));
        }

        [Fact]
        public void TestUpdateAsync_DomainIdIsEmptyString_Throws()
        {
            var target = new NfieldDomainResponseCodesService();
            Assert.Throws<ArgumentException>(
                () => UnwrapAggregateException(target.UpdateAsync(string.Empty, 1, new DomainResponseCodeData())));
        }

        [Fact]
        public void TestUpdateAsync_DomainresponseCodeIsNull_Throws()
        {
            var target = new NfieldDomainResponseCodesService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.UpdateAsync("sruveyId", 2, null)));
        }

        [Fact]
        public void TestUpdateAsync_ValidDomainResponseCode_CallsCorrectUrl()
        {
            // Arrange
            const string domainId = "domainId";
            const int code = 10;
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var responseCodeToUpdate = new DomainResponseCodeData
            {                
                Description = "Description",
                Url = "http://www.google.com"
            };
            mockedHttpClient.Setup(client => client.PatchAsJsonAsync(It.IsAny<Uri>(), It.IsAny<DomainResponseCodeData>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(new DomainResponseCode()))));
            var target = new NfieldDomainResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            target.UpdateAsync(domainId, code, responseCodeToUpdate).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                hc.PatchAsJsonAsync(
                    It.Is<Uri>(
                        url => url.AbsolutePath.EndsWith($"Domains/{domainId}/ResponseCodes/{code}")),
                    It.IsAny<DomainResponseCodeData>()),
                Times.Once());
        }

        [Fact]
        public void TestUpdateAsync_ValidDomainResponseCode_ReturnsModifiedResponseCode()
        {
            // Arrange
            const string domainId = "domainId";
            const int code = 10;
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var responseCodeToUpdate = new DomainResponseCode
            {
                ResponseCode = code,
                Description = "Description",
                Url = "http://www.google.com"
            };
            mockedHttpClient.Setup(client => client.PatchAsJsonAsync(It.IsAny<Uri>(), It.IsAny<DomainResponseCodeData>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(responseCodeToUpdate))));
            var target = new NfieldDomainResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            var result = target.UpdateAsync(domainId, code, responseCodeToUpdate).Result;

            // Assert
            Assert.Equal(responseCodeToUpdate.ResponseCode, result.ResponseCode);
            Assert.Equal(responseCodeToUpdate.Description, result.Description);
            Assert.Equal(responseCodeToUpdate.Url, result.Url);
        }

        #endregion

        #region Remove Async

        [Fact]
        public void TestRemoveAsync_DomainIdIsNull_Throws()
        {
            var target = new NfieldDomainResponseCodesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(null, 10)));
        }

        [Fact]
        public void TestRemoveAsync_DomainIdIsEmptyString_Throws()
        {
            var target = new NfieldDomainResponseCodesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(string.Empty, 10)));
        }

        [Fact]
        public void TestRemoveAsync_ValidDomainResponseCode_CallsCorrectUrl()
        {
            // Arrange
            const string domainId = "domainId";
            const int code = 10;
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.DeleteAsync(It.IsAny<Uri>()))
                .Returns(CreateTask(HttpStatusCode.OK));
            var target = new NfieldDomainResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            target.RemoveAsync(domainId, code).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                hc.DeleteAsync(It.Is<Uri>(
                        url => url.AbsolutePath.EndsWith($"Domains/{domainId}/ResponseCodes/{code}"))),
                Times.Once());
        }

        #endregion

    }
}
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
using static Nfield.Services.Implementation.NfieldDomainResponseCodesService;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldDomainResponseCodesService"/>
    /// </summary>
    public class NfieldDomainResponseCodesServiceTests : NfieldServiceTestsBase
    {

        #region Query Async 

        [Fact]
        public void TestQueryAsync_ValidDomainId_CallsCorrectUrl()
        {
            // Arrange        
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var target = new NfieldDomainResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            mockedHttpClient.Setup(client => client.GetAsync(It.IsAny<Uri>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(new List<DomainResponseCode>()))));

            // Act
            target.QueryAsync().Wait();

            // Assert
            mockedHttpClient.Verify(
                hc => hc.GetAsync(It.Is<Uri>(url => url.AbsolutePath.EndsWith("ResponseCodes"))), Times.Once());
        }

        #endregion

        #region Add Async
       

        [Fact]
        public void TestAddAsync_DomainResponseCodeIdIsNull_Throws()
        {
            var target = new NfieldDomainResponseCodesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.AddAsync(null)));
        }

        [Fact]
        public void TestAddAsync_ValidDomainResponseCode_CallsCorrectUrl()
        {
            // Arrange
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var responseCodeToAdd = new DomainResponseCode
            {
                Id = 15,
                Description = "Description",              
                Url = "http://www.google.com"
            };
            mockedHttpClient.Setup(client => client.PostAsJsonAsync(It.IsAny<Uri>(), It.IsAny<DomainResponseCode>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(new DomainResponseCode()))));
            var target = new NfieldDomainResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            target.AddAsync(responseCodeToAdd).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                   hc.PostAsJsonAsync(It.Is<Uri>(url => url.AbsolutePath.EndsWith("ResponseCodes")), responseCodeToAdd),
                    Times.Once());
        }

        [Fact]
        public void TestAddAsync_ValidDomainResponseCode_ReturnsAddedResponseCode()
        {
            // Arrange
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var responseCodeToAdd = new DomainResponseCode
            {
                Id = 15,
                Description = "Description",             
                Url = "http://www.google.com"
            };
            mockedHttpClient.Setup(client => client.PostAsJsonAsync(It.IsAny<Uri>(), It.IsAny<DomainResponseCode>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(responseCodeToAdd))));
            var target = new NfieldDomainResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            var result = target.AddAsync(responseCodeToAdd).Result;

            // Assert
            Assert.Equal(responseCodeToAdd.Id, result.Id);
            Assert.Equal(responseCodeToAdd.Description, result.Description);
            Assert.Equal(responseCodeToAdd.Url, result.Url);
        }

        #endregion

        #region Update Async
        

        [Fact]
        public void TestUpdateAsync_DomainresponseCodeIsNull_Throws()
        {
            var target = new NfieldDomainResponseCodesService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.UpdateAsync(null)));
        }

        [Fact]
        public void TestUpdateAsync_ValidDomainResponseCode_CallsCorrectUrl()
        {
            // Arrange       
            const int code = 10;
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var responseCodeToUpdate = new DomainResponseCode
            {
                Id = code,
                Description = "Description",
                Url = "http://www.google.com"
            };
            mockedHttpClient.Setup(client => client.PatchAsJsonAsync(It.IsAny<Uri>(), It.IsAny<UpdateDomainResponsecode>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(new DomainResponseCode()))));
            var target = new NfieldDomainResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            target.UpdateAsync(responseCodeToUpdate).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                hc.PatchAsJsonAsync(
                    It.Is<Uri>(
                        url => url.AbsolutePath.EndsWith($"ResponseCodes/{code}")),
                    It.IsAny<UpdateDomainResponsecode>()),
                Times.Once());
        }

        [Fact]
        public void TestUpdateAsync_ValidDomainResponseCode_ReturnsModifiedResponseCode()
        {
            // Arrange       
            const int code = 10;
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var responseCodeToUpdate = new DomainResponseCode
            {
                Id = code,
                Description = "Description",
                Url = "http://www.google.com"
            };
            mockedHttpClient.Setup(client => client.PatchAsJsonAsync(It.IsAny<Uri>(), It.IsAny<UpdateDomainResponsecode>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(responseCodeToUpdate))));
            var target = new NfieldDomainResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            var result = target.UpdateAsync(responseCodeToUpdate).Result;

            // Assert
            Assert.Equal(responseCodeToUpdate.Id, result.Id);
            Assert.Equal(responseCodeToUpdate.Description, result.Description);
            Assert.Equal(responseCodeToUpdate.Url, result.Url);
        }

        #endregion

        #region Remove Async
        

        [Fact]
        public void TestRemoveAsync_ValidDomainResponseCode_CallsCorrectUrl()
        {
            // Arrange        
            const int code = 10;
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.DeleteAsync(It.IsAny<Uri>()))
                .Returns(CreateTask(HttpStatusCode.OK));
            var target = new NfieldDomainResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            target.RemoveAsync(code).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                hc.DeleteAsync(It.Is<Uri>(
                        url => url.AbsolutePath.EndsWith($"ResponseCodes/{code}"))),
                Times.Once());
        }

        #endregion

    }
}
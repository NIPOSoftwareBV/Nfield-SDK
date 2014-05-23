using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyResponseCodesService"/>
    /// </summary>
    public class NfieldSurveyResponseCodesServiceTests : NfieldServiceTestsBase
    {

        #region Query Async

        [Fact]
        public void TestQueryAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyResponseCodesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.QueryAsync(null)));
        }

        [Fact]
        public void TestQueryAsync_SurveyIdIsEmptyString_Throws()
        {
            var target = new NfieldSurveyResponseCodesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.QueryAsync(string.Empty)));
        }

        [Fact]
        public void TestQueryAsync_ValidSurveyId_CAllsCorrectUrl()
        {
            // Arrange
            const string surveyId = "surveyId";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var target = new NfieldSurveyResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            mockedHttpClient.Setup(client => client.GetAsync(It.IsAny<string>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(new List<SurveyResponseCode>()))));

            // Act
            target.QueryAsync(surveyId).Wait();

            // Assert
            mockedHttpClient.Verify(
                hc => hc.GetAsync(It.Is<string>(url => url.EndsWith("Surveys/" + surveyId + "/ResponseCodes/"))), Times.Once());
        }

        #endregion

        #region Query Async based on code

        [Fact]
        public void TestQueryAsyncBasedOnCode_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyResponseCodesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.QueryAsync(null, 20)));
        }

        [Fact]
        public void TestQueryAsyncBasedOnCode_SurveyIdIsEmptyString_Throws()
        {
            var target = new NfieldSurveyResponseCodesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.QueryAsync(string.Empty, 20)));
        }

        [Fact]
        public void TestQueryAsyncBasedOnCode_ValidSurveyId_CAllsCorrectUrl()
        {
            // Arrange
            const string surveyId = "surveyId";
            const int code = 20;
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var target = new NfieldSurveyResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            mockedHttpClient.Setup(client => client.GetAsync(It.IsAny<string>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(new SurveyResponseCode()))));

            // Act
            target.QueryAsync(surveyId, code).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                hc.GetAsync(It.Is<string>(url =>
                    url.EndsWith(string.Format("Surveys/{0}/ResponseCodes/{1}", surveyId, code)))),
                Times.Once());
        }

        #endregion

        #region Add Async

        [Fact]
        public void TestAddAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyResponseCodesService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.AddAsync(null, new SurveyResponseCode())));
        }

        [Fact]
        public void TestAddAsync_SurveyIdIsEmptyString_Throws()
        {
            var target = new NfieldSurveyResponseCodesService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.AddAsync(string.Empty, new SurveyResponseCode())));
        }

        [Fact]
        public void TestAddAsync_SurveyResponseCodeIdIsNull_Throws()
        {
            var target = new NfieldSurveyResponseCodesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.AddAsync("surveyId", null)));
        }

        [Fact]
        public void TestAddAsync_ValidSurveyResponseCode_CallsCorrectUrl()
        {
            // Arrange
            const string surveyId = "surveyId";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var responseCodeToAdd = new SurveyResponseCode
            {
                ResponseCode = 15,
                Description = "Description",
                IsDefinite = true,
                AllowAppointment = false
            };
            mockedHttpClient.Setup(client => client.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<SurveyResponseCode>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(new SurveyResponseCode()))));
            var target = new NfieldSurveyResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            target.AddAsync(surveyId, responseCodeToAdd).Wait();

            // Assert
            mockedHttpClient.Verify( hc =>
                    hc.PostAsJsonAsync(It.Is<string>(url => url.EndsWith("Surveys/" + surveyId + "/ResponseCodes/")), responseCodeToAdd), 
                    Times.Once());
        }

        [Fact]
        public void TestAddAsync_ValidSurveyResponseCode_ReturnsAddedResponseCode()
        {
            // Arrange
            const string surveyId = "surveyId";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var responseCodeToAdd = new SurveyResponseCode
            {
                ResponseCode = 15,
                Description = "Description",
                IsDefinite = true,
                AllowAppointment = false
            };
            mockedHttpClient.Setup(client => client.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<SurveyResponseCode>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(responseCodeToAdd))));
            var target = new NfieldSurveyResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            var result = target.AddAsync(surveyId, responseCodeToAdd).Result;

            // Assert
            Assert.Equal(responseCodeToAdd.ResponseCode, result.ResponseCode);
            Assert.Equal(responseCodeToAdd.Description, result.Description);
            Assert.Equal(responseCodeToAdd.IsDefinite, result.IsDefinite);
            Assert.Equal(responseCodeToAdd.AllowAppointment, result.AllowAppointment);
            Assert.Equal(responseCodeToAdd.IsSelectable, result.IsSelectable);
        }

        #endregion

        #region Update Async

        [Fact]
        public void TestUpdateAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyResponseCodesService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.UpdateAsync(null, new SurveyResponseCode())));
        }

        [Fact]
        public void TestUpdateAsync_SurveyIdIsEmptyString_Throws()
        {
            var target = new NfieldSurveyResponseCodesService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.UpdateAsync(string.Empty, new SurveyResponseCode())));
        }

        [Fact]
        public void TestUpdateAsync_SurveyresponseCodeIsNull_Throws()
        {
            var target = new NfieldSurveyResponseCodesService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.UpdateAsync("sruveyId", null)));
        }

        [Fact]
        public void TestUpdateAsync_ValidSurveyResponseCode_CallsCorrectUrl()
        {
            // Arrange
            const string surveyId = "surveyId";
            const int code = 10;
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var responseCodeToUpdate = new SurveyResponseCode
            {
                ResponseCode = code,
                Description = "Description",
                IsDefinite = true,
                AllowAppointment = false
            };
            mockedHttpClient.Setup(client => client.PatchAsJsonAsync(It.IsAny<string>(), It.IsAny<UpdateSurveyResponseCode>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(new SurveyResponseCode()))));
            var target = new NfieldSurveyResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            target.UpdateAsync(surveyId, responseCodeToUpdate).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                hc.PatchAsJsonAsync(
                    It.Is<string>(
                        url => url.EndsWith(string.Format("Surveys/{0}/ResponseCodes/{1}", surveyId, code))),
                    It.IsAny<UpdateSurveyResponseCode>()),
                Times.Once());
        }

        [Fact]
        public void TestUpdateAsync_ValidSurveyResponseCode_ReturnsModifiedResponseCode()
        {
            // Arrange
            const string surveyId = "surveyId";
            const int code = 10;
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var responseCodeToUpdate = new SurveyResponseCode
            {
                ResponseCode = code,
                Description = "Description",
                IsDefinite = true,
                AllowAppointment = false
            };
            mockedHttpClient.Setup(client => client.PatchAsJsonAsync(It.IsAny<string>(), It.IsAny<UpdateSurveyResponseCode>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(responseCodeToUpdate))));
            var target = new NfieldSurveyResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            var result = target.UpdateAsync(surveyId, responseCodeToUpdate).Result;

            // Assert
            Assert.Equal(responseCodeToUpdate.ResponseCode, result.ResponseCode);
            Assert.Equal(responseCodeToUpdate.Description, result.Description);
            Assert.Equal(responseCodeToUpdate.IsDefinite, result.IsDefinite);
            Assert.Equal(responseCodeToUpdate.AllowAppointment, result.AllowAppointment);
            Assert.Equal(responseCodeToUpdate.IsSelectable, result.IsSelectable);
        }

        #endregion

        #region Remove Async

        [Fact]
        public void TestRemoveAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyResponseCodesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(null, 10)));
        }

        [Fact]
        public void TestRemoveAsync_SurveyIdIsEmptyString_Throws()
        {
            var target = new NfieldSurveyResponseCodesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(string.Empty, 10)));
        }

        [Fact]
        public void TestRemoveAsync_ValidSurveyResponseCode_CallsCorrectUrl()
        {
            // Arrange
            const string surveyId = "surveyId";
            const int code = 10;
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.DeleteAsync(It.IsAny<string>()))
                .Returns(CreateTask(HttpStatusCode.OK));
            var target = new NfieldSurveyResponseCodesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            target.RemoveAsync(surveyId, code).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                hc.DeleteAsync(It.Is<string>(
                        url => url.EndsWith(string.Format("Surveys/{0}/ResponseCodes/{1}", surveyId, code)))),
                Times.Once());
        }

        #endregion

    }
}
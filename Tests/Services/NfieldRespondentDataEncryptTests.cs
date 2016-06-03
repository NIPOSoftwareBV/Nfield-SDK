using System;
using System.Net;
using System.Net.Http;
using Moq;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class NfieldRespondentDataEncryptTests: NfieldServiceTestsBase
    {
        private  NfieldRespondentDataEncryptService _target;
        private  Mock<INfieldHttpClient> _mockedHttpClient;
        private  Mock<INfieldConnectionClient> _mockedNfieldConnection;

        private const string SurveyId = "2cd16d44-f672-4845-88e2-598848e0b098";

        /// <summary>
        /// Tests the encryption_ survey id_ not exists_ returns resource not found exception.
        /// </summary>
        [Fact]
        public void TestEncryption_SurveyId_DoesntExists_ReturnsResourceNotFoundException()
        {
            var dataModel = new DataCryptographyModel() { Data = "Data Test string", IV = "VGhpc0lzQUJhc2U2NDY0Ng==" };
           
            _mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(_mockedNfieldConnection);

            // API call response
            var expectedResult = @"IV=VGhpc0lzQUJhc2U2NDY0Ng==&DATA=kDdE+WOvPi45K6q1fC8iLIJ+M7j5xZmETPf24AS81jk=";
            _mockedHttpClient.Setup(client => client.PostAsJsonAsync($"{ServiceAddress}v1/Surveys/{SurveyId}/RespondentDataEncrypt", dataModel))
               .Returns(CreateTask(HttpStatusCode.OK, new StringContent(expectedResult)));

            _target = new NfieldRespondentDataEncryptService();
            _target.InitializeNfieldConnection(_mockedNfieldConnection.Object);

            var sdkResult = _target.EncryptData(SurveyId, dataModel).Result;

            Assert.Equal(expectedResult, sdkResult);
        }

        /// <summary>
        /// Tests the encryption_ survey identifier is null_ throws.
        /// </summary>
        [Fact]
        public void TestEncryption_SurveyId_IsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldRespondentDataEncryptService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.EncryptData(null, new DataCryptographyModel())));
        }


        /// <summary>
        /// Tests the encryption_ survey encryption model_ is null_ throws argument null exception.
        /// </summary>
        [Fact]
        public void TestEncryption_DataEncryptionService_IsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldRespondentDataEncryptService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.EncryptData("2cd16d44-f672-4845-88e2-598848e0b098", null)));
        }
    }
}

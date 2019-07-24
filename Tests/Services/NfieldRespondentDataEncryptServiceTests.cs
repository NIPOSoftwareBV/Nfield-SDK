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
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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
    public class NfieldRespondentDataEncryptServiceTests : NfieldServiceTestsBase
    {
        private NfieldRespondentDataEncryptService _target;
        private Mock<INfieldHttpClient> _mockedHttpClient;
        private Mock<INfieldConnectionClient> _mockedNfieldConnection;

        private const string SurveyId = "2cd16d44-f672-4845-88e2-598848e0b098";

        /// <summary>
        /// Tests the encryption_ survey id_ not exists_ returns resource not found exception.
        /// </summary>
        [Fact]
        public void TestEncryption_SurveyId_DoesntExists_ReturnsResourceNotFoundException()
        {
            var dataModel = new DataCryptographyModel { Data = new Dictionary<string, string> { { "DataExample1", "ValueExample1" }, { "DataExample2", "ValueExample2" } }, IV = "VGhpc0lzQUJhc2U2NDY0Ng==" };

            _mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(_mockedNfieldConnection);

            // API call response
            var expectedResult = @"IV=VGhpc0lzQUJhc2U2NDY0Ng==&DATA=kDdE+WOvPi45K6q1fC8iLIJ+M7j5xZmETPf24AS81jk=";
            _mockedHttpClient.Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/RespondentDataEncrypt"), dataModel))
               .Returns(CreateTask(HttpStatusCode.OK, new ObjectContent<string>(expectedResult, new JsonMediaTypeFormatter())));

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

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
using System.Net;
using Moq;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyInterviewerWorkpackageDistributionService"/>
    /// </summary>
    public class NfieldSurveyInterviewerWorkpackageDistributionServiceTests : NfieldServiceTestsBase
    {
        #region DistributeAsync tests

        [Fact]
        public void TestDistributeAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyInterviewerWorkpackageDistributionService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.DistributeAsync(null, new SurveyInterviewerDistributeModel())));
        }

        [Fact]
        public void TestDistributeAsync_ModelIsNull_Throws()
        {
            var target = new NfieldSurveyInterviewerWorkpackageDistributionService();
            Assert.Throws<ArgumentNullException>(
                () => UnwrapAggregateException(target.DistributeAsync("surveyId", null)));
        }

        [Fact]
        public void TestDistributeAsync_ValidSurveyResponseCode_CallsCorrectUrl()
        {
            // Arrange
            const string surveyId = "surveyId";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<SurveyInterviewerDistributeModel>()))
                .Returns(CreateTask(HttpStatusCode.OK));
            var target = new NfieldSurveyInterviewerWorkpackageDistributionService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            var model = new SurveyInterviewerDistributeModel();
            target.DistributeAsync(surveyId, model).Wait();

            // Assert
            mockedHttpClient.Verify(hc =>
                    hc.PostAsJsonAsync(
                        It.Is<string>(url => url.EndsWith("Surveys/" + surveyId + "/Distribute/")),
                        It.Is<SurveyInterviewerDistributeModel>(m => m == model)),
                    Times.Once());
        }

        #endregion
    }
}

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
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    public class NfieldSurveyInvitationImagesServiceTests : NfieldServiceTestsBase
    {
        #region AddImage parameters validation

        [Fact]
        public void TestAddImage_SurveyIsNull_Throws()
        {
            var target = new NfieldSurveyInvitationImagesService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.AddImageAsync(null, "somefilename", new MemoryStream())));
        }

        [Fact]
        public void TestAddImage_SurveyIsEmpty_Throws()
        {
            var target = new NfieldSurveyInvitationImagesService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.AddImageAsync(string.Empty, "somefilename", new MemoryStream())));
        }

        [Fact]
        public void TestAddImage_SurveyIsWhitespace_Throws()
        {
            var target = new NfieldSurveyInvitationImagesService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.AddImageAsync("   ", "somefilename", new MemoryStream())));
        }

        [Fact]
        public void TestAddImage_FilenameIsNull_Throws()
        {
            var target = new NfieldSurveyInvitationImagesService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.AddImageAsync("survey", null, new MemoryStream())));
        }

        [Fact]
        public void TestAddImage_FilenameIsEmpty_Throws()
        {
            var target = new NfieldSurveyInvitationImagesService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.AddImageAsync("survey", string.Empty, new MemoryStream())));
        }

        [Fact]
        public void TestAddImage_FilenameIsWhitespace_Throws()
        {
            var target = new NfieldSurveyInvitationImagesService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.AddImageAsync("survey", "   ", new MemoryStream())));
        }

        [Fact]
        public void TestAddImage_ContentIsNull_Throws()
        {
            var target = new NfieldSurveyInvitationImagesService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.AddImageAsync("survey", "somefilename", null)));
        }

        #endregion

        [Fact]
        public void TestAddImage_AllParamsOk_Successful()
        {
            const string surveyId = "surveyWithImages";
            const string filename = "imageForSurvey";

            var target = new NfieldSurveyInvitationImagesService();
            var mockClient = InitMockClient(surveyId, filename);
            target.InitializeNfieldConnection(mockClient);

            AddInvitationImageResult actual;
            using (var stream = new MemoryStream(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7 }))
            {
                actual = target.AddImageAsync(surveyId, filename, stream).Result;
            }

            Assert.NotEmpty(actual.Link);
            Assert.Contains(surveyId, actual.Link);
            Assert.Contains(filename, actual.Link);
        }

        private INfieldConnectionClient InitMockClient(string surveyId, string filename)
        {
            var expectedResponseContent = new AddInvitationImageResult
            {
                Link = @"testResultLink.blobstorage/" + surveyId + "/" + filename
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var responseContentString = JsonConvert.SerializeObject(expectedResponseContent);
            var responseContent = new StringContent(responseContentString, Encoding.Unicode, "application/json");
            var uri = "Surveys/" + surveyId + "/InvitationImages/" + filename;
            mockedHttpClient.Setup(client => client.PostAsync(It.Is<Uri>(s => s.AbsolutePath.EndsWith(uri)),
                                It.IsAny<StreamContent>()))
                            .Returns(CreateTask(HttpStatusCode.OK, responseContent));

            return mockedNfieldConnection.Object;
        }
    }
}

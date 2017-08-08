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
            using (var stream = new MemoryStream(new byte[] {0, 1, 2, 3, 4, 5, 6, 7}))
            {           
                actual = target.AddImageAsync(surveyId, filename, stream).Result;
            }

            Assert.NotEmpty(actual.Link);
            Assert.True(actual.Link.Contains(surveyId));
            Assert.True(actual.Link.Contains(filename));
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
            var uri = ServiceAddress + "Surveys/" + surveyId + "/InvitationImages/" + filename;
            mockedHttpClient.Setup(client => client.PostAsync(It.Is<string>(s => s == uri), 
                                It.IsAny<StreamContent>()))
                            .Returns(CreateTask(HttpStatusCode.OK, responseContent));

            return mockedNfieldConnection.Object;
        }
    }
}

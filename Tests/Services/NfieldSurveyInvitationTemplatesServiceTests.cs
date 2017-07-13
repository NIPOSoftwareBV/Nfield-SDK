using Nfield.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Moq;
using Nfield.Infrastructure;
using Nfield.Models;
using Xunit;

namespace Nfield.Services
{
    public class NfieldSurveyInvitationTemplatesServiceTests : NfieldServiceTestsBase
    {
        [Fact]
        public void TestGetAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyInvitationTemplatesService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.GetAsync(null)));
        }

        [Fact]
        public void TestGetAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveyInvitationTemplatesService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.GetAsync("  ")));
        }

        [Fact]
        public void TestGetAsync_SurveyHasInvitationTemplates_ReturnsInvitationTemplates()
        {
            const string SurveyId = "TestSurveyId";

            var expected = new InvitationTemplateModel()
            {
                Id = 1,
                InvitationType = 1,
                Name = "TestTemplate",
                Subject = "TestSubject",
                Body = "TestBody"
            };

            var target = new NfieldSurveyInvitationTemplatesService();
            var mockClient = InitMockClient(SurveyId, expected);
            target.InitializeNfieldConnection(mockClient);
            var actualResults = target.GetAsync(SurveyId).Result;

            var actual = actualResults.First();
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.InvitationType, actual.InvitationType);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Subject, actual.Subject);
            Assert.Equal(expected.Body, actual.Body);
        }

        private INfieldConnectionClient InitMockClient<T>(string surveyId, T content)
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var responseContent = new ObjectContent<T>(content, new JsonMediaTypeFormatter());
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "Surveys/" + surveyId + "/URL"))
                .Returns(CreateTask(HttpStatusCode.OK, responseContent));

            return mockedNfieldConnection.Object;
        }

    }
}

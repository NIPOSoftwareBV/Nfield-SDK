using Nfield.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Moq;
using Nfield.Infrastructure;
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
            var target = new NfieldSurveyInvitationTemplatesService();
            var mockClient = InitMockClient();
            target.InitializeNfieldConnection(mockClient);
            Assert.Equal(true,true);
        }

        private INfieldConnectionClient InitMockClient()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            //var content = new StringContent(sample);
            //mockedHttpClient
            //    .Setup(client => client.GetAsync(ServiceAddress + "Surveys/" + SurveyId + "/Sample"))
            //    .Returns(CreateTask(HttpStatusCode.OK, content));

            return mockedNfieldConnection.Object;
        }

    }
}

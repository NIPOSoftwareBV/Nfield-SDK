using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Nfield.SDK.Services.Implementation;
using Nfield.SDK.Models;
using System.Net.Mail;
using Nfield.SDK.Services;

namespace Nfield.Services
{
    public class NFieldMeServiceTest : NfieldServiceTestsBase
    {

        [Fact]
        public void Test_GetQuotaFrameVersionsAsync_SurveyIdIsNull_Throws()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();

            var target = new NFieldMeService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.GetUserRoles()));
        }

        [Fact]
        public void TestGetRoles_Returns_User_Roles()
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.OK;

            var permissions = new List<string>{
                "Activity.Read",
                "SamplingPoint.Read",
                "InterviewQuality.Read"
            };

            var userRoles = new List<string>
            {
                "Admin",
                "SuperUser"
            };

            var content = new UserRoleModel
            {
                Permissions = permissions,
                UserRoles = userRoles
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient.Setup(client => client.GetAsync(new Uri(ServiceAddress, "me/role/")))
                .Returns(
                    Task.Factory.StartNew(
            () =>
                            new HttpResponseMessage(httpStatusCode)
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(content))
                            }));
            mockedNfieldConnection
               .SetupGet(connection => connection.Client)
               .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(ServiceAddress);

            var target = new NFieldMeService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.GetUserRoles().Result;

            Assert.Equal(content.Permissions, actual.Permissions);
            Assert.Equal(content.UserRoles, actual.UserRoles);
        }

    }
}

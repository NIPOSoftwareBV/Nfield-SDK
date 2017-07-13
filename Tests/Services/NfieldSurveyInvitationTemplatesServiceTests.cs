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

using Nfield.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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
            const string surveyId = "TestSurveyId";

            var expected1 = new InvitationTemplateModel()
            {
                Id = 1,
                InvitationType = 1,
                Name = "TestTemplate1",
                Subject = "TestSubject1",
                Body = "TestBody1"
            };

            var expected2 = new InvitationTemplateModel()
            {
                Id = 2,
                InvitationType = 2,
                Name = "TestTemplate2",
                Subject = "TestSubject2",
                Body = "TestBody2"
            };

            var target = new NfieldSurveyInvitationTemplatesService();
            var returnObject = new [] { expected1, expected2 };
            var mockClient = InitMockClient<IEnumerable<InvitationTemplateModel>>(surveyId, returnObject);
            target.InitializeNfieldConnection(mockClient);
            var actualResults = target.GetAsync(surveyId).Result.ToArray();

            var actual1 = actualResults[0];
            AssertOnInvitationTemplatesFields(expected1, actual1);

            var actual2 = actualResults[1];
            AssertOnInvitationTemplatesFields(expected2, actual2);
        }

        private INfieldConnectionClient InitMockClient<T>(string surveyId, T content)
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var responseContent = new ObjectContent<T>(content, new JsonMediaTypeFormatter());
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "Surveys/" + surveyId + "/InvitationTemplates"))
                .Returns(CreateTask(HttpStatusCode.OK, responseContent));

            return mockedNfieldConnection.Object;
        }

        private void AssertOnInvitationTemplatesFields(InvitationTemplateModel expected, InvitationTemplateModel actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.InvitationType, actual.InvitationType);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Subject, actual.Subject);
            Assert.Equal(expected.Body, actual.Body);
        }
    }
}

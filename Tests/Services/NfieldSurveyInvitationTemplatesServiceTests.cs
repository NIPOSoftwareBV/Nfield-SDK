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
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Xunit;

namespace Nfield.Services
{
    public class NfieldSurveyInvitationTemplatesServiceTests : NfieldServiceTestsBase
    {
        #region GetAsync

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
            var returnObject = new[] { expected1, expected2 };
            var mockClient = InitMockClient<IEnumerable<InvitationTemplateModel>>(surveyId, returnObject);
            target.InitializeNfieldConnection(mockClient);
            var actualResults = target.GetAsync(surveyId).Result.ToArray();

            Assert.Equal(expected1, actualResults[0], new InvitationTemplateComparer());
            Assert.Equal(expected2, actualResults[1], new InvitationTemplateComparer());
        }

        #endregion

        #region AddAsync

        [Fact]
        public void TestAddAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyInvitationTemplatesService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.AddAsync(null, null)));
        }

        [Fact]
        public void TestAddAsync_SurveyIdIsWhitespace_Throws()
        {
            var target = new NfieldSurveyInvitationTemplatesService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.AddAsync("  ", null)));
        }

        [Fact]
        public void TestAddAsync_InvitationTemplateIsNull_Throws()
        {
            var target = new NfieldSurveyInvitationTemplatesService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.AddAsync("a survey", null)));
        }

        [Fact]
        public void TestAddAsync_InvitationTemplateIsAdded_ReturnsInvitationTemplate()
        {
            const string surveyId = "TestSurveyId";

            var invitationTemplate = new InvitationTemplateModel
            {
                InvitationType = 1,
                Name = "TestTemplate1",
                Subject = "TestSubject1",
                Body = "TestBody1"
            };
            var expected = new InvitationTemplateModel
            {
                Id = 999,
                InvitationType = invitationTemplate.InvitationType,
                Name = invitationTemplate.Name,
                Subject = invitationTemplate.Subject,
                Body = invitationTemplate.Body
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(expected));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(ServiceAddress + "Surveys/" + surveyId + "/InvitationTemplates", invitationTemplate))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldSurveyInvitationTemplatesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddAsync(surveyId, invitationTemplate).Result;

            Assert.Equal(expected, actual, new InvitationTemplateComparer());
        }

        #endregion

        #region Updatesync

        [Fact]
        public void TestUpdateAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyInvitationTemplatesService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.UpdateAsync(null, null)));
        }

        [Fact]
        public void TestUpdateAsync_SurveyIdIsWhitespace_Throws()
        {
            var target = new NfieldSurveyInvitationTemplatesService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.UpdateAsync("  ", null)));
        }

        [Fact]
        public void TestUpdateAsync_InvitationTemplateIsNull_Throws()
        {
            var target = new NfieldSurveyInvitationTemplatesService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.UpdateAsync("a survey", null)));
        }

        [Fact]
        public void TestUpdateAsync_InvitationTemplateIsUpdated_ReturnsInvitationTemplate()
        {
            const string surveyId = "TestSurveyId";
            const int templateId = 42;

            var invitationTemplate = new InvitationTemplateModel
            {
                Id = templateId,
                InvitationType = 1,
                Body = "TestBody1"
            };
            var expected = new InvitationTemplateModel
            {
                Id = templateId,
                InvitationType = invitationTemplate.InvitationType,
                Name = "a Name",
                Subject = "a Subject",
                Body = invitationTemplate.Body
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(expected));
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(ServiceAddress + "Surveys/" + surveyId + "/InvitationTemplates/" + templateId, It.Is<InvitationTemplateModel>(t => VerifyInvitationTemplate(t, invitationTemplate))))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldSurveyInvitationTemplatesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.UpdateAsync(surveyId, invitationTemplate).Result;

            Assert.Equal(expected, actual, new InvitationTemplateComparer());
        }

        private static bool VerifyInvitationTemplate(InvitationTemplateModel updatedInvitationTemplate, InvitationTemplateModel t1)
        {
            return updatedInvitationTemplate.InvitationType.Equals(t1.InvitationType)
                   && updatedInvitationTemplate.Body.Equals(t1.Body)
                   && updatedInvitationTemplate.Subject == null
                   && updatedInvitationTemplate.Name == null;
        }

        #endregion

        #region RemoveAync

        [Fact]
        public void TestRemoveAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyInvitationTemplatesService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.RemoveAsync(null, 0)));
        }

        [Fact]
        public void TestRemoveAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveyInvitationTemplatesService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.RemoveAsync("  ", 0)));
        }

        [Fact]
        public void TestRemoveAsync_InvitationTemplateIsRemoved_ReturnsTrue()
        {
            const string surveyId = "TestSurveyId";
            const int templateId = 42;

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(new {IsSuccess = true}));
            mockedHttpClient
                .Setup(client => client.DeleteAsync(ServiceAddress + "Surveys/" + surveyId + "/InvitationTemplates/" + templateId))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldSurveyInvitationTemplatesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.RemoveAsync(surveyId, templateId).Result;

            Assert.Equal(true, actual);
        }

        #endregion

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

        private class InvitationTemplateComparer : IEqualityComparer<InvitationTemplateModel>
        {
            public bool Equals(InvitationTemplateModel x, InvitationTemplateModel y)
            {
                return x.Id.Equals(y.Id)
                    && x.InvitationType.Equals(y.InvitationType)
                    && x.Name.Equals(y.Name)
                    && x.Subject.Equals(y.Subject)
                    && x.Body.Equals(y.Body);
            }

            public int GetHashCode(InvitationTemplateModel obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}

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
using System.Linq;
using System.Net;
using System.Net.Http;
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.SDK.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyLanguageTranslationsService"/>
    /// </summary>
    public class NfieldSurveyLanguageTranslationsServiceTests : NfieldServiceTestsBase
    {
        const string SurveyId = "MySurvey";
        const int languageId = 123;

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_SurveyIdIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveyLanguageTranslationsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.QueryAsync(null)));
        }

        [Fact]
        public void TestQueryAsync_SurveyIdIsEmpty_ThrowsArgumentException()
        {
            var target = new NfieldSurveyLanguageTranslationsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.QueryAsync(string.Empty)));
        }

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithLanguages()
        {
            var expectedTranslations = new SurveyLanguageTranslations[]
            {
                new SurveyLanguageTranslations{Name = "X", Id = 1, ButtonNext = "ButtonX"},
                new SurveyLanguageTranslations{Name = "Y", Id = 2, ButtonNext = "ButtonY"}
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/LanguageTranslations")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedTranslations))));

            var target = new NfieldSurveyLanguageTranslationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualTranslations = target.QueryAsync(SurveyId).Result;
            Assert.Equal(2, actualTranslations.Count());
            Assert.Equal(expectedTranslations[0].Name, actualTranslations.ToArray()[0].Name);
            Assert.Equal(expectedTranslations[0].Id, actualTranslations.ToArray()[0].Id);
            Assert.Equal(expectedTranslations[0].ButtonNext, actualTranslations.ToArray()[0].ButtonNext);
            Assert.Equal(expectedTranslations[1].Name, actualTranslations.ToArray()[1].Name);
            Assert.Equal(expectedTranslations[1].Id, actualTranslations.ToArray()[1].Id);
            Assert.Equal(expectedTranslations[1].ButtonNext, actualTranslations.ToArray()[1].ButtonNext);
        }

        #endregion

        #region AddAsync

        [Fact]
        public void TestAddAsync_SurveyIdIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveyLanguageTranslationsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.AddAsync(null, new SurveyLanguageTranslations())));
        }

        [Fact]
        public void TestAddAsync_SurveyIdIsEmpty_ThrowsArgumentException()
        {
            var target = new NfieldSurveyLanguageTranslationsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.AddAsync(string.Empty, new SurveyLanguageTranslations())));
        }

        [Fact]
        public void TestAddAsync_ServerAcceptsLanguageTranslations_ReturnsLanguageTranslations()
        {
            var translations = new SurveyLanguageTranslations()
            {
                ButtonNext = "Next",
                ButtonBack = "Back",
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var content = new StringContent(JsonConvert.SerializeObject(translations));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/LanguageTranslations"), translations))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldSurveyLanguageTranslationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddAsync(SurveyId, translations).Result;

            Assert.Equal(translations.ButtonNext, actual.ButtonNext);
            Assert.Equal(translations.ButtonBack, actual.ButtonBack);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_SurveyIdIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveyLanguageTranslationsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync(null, languageId, new SurveyLanguageTranslations())));
        }

        [Fact]
        public void TestUpdateAsync_SurveyIdIsEmpty_ThrowsArgumentException()
        {
            var target = new NfieldSurveyLanguageTranslationsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.UpdateAsync(string.Empty, languageId, new SurveyLanguageTranslations())));
        }

        [Fact]
        public void TestUpdateAsync_SurveyAndLanguageExist_DoesNotThrow()
        {
            var translations = new SurveyLanguageTranslations()
            {
                ButtonNext = "El Nexto",
                ButtonBack = "El Backo",
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var content = new StringContent(JsonConvert.SerializeObject(translations));
            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/LanguageTranslations/{languageId}"), translations))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldSurveyLanguageTranslationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.UpdateAsync(SurveyId, languageId, translations).Result;

            Assert.Equal(translations.ButtonNext, actual.ButtonNext);
            Assert.Equal(translations.ButtonBack, actual.ButtonBack);

        }

        #endregion

        #region RemoveAsync

        [Fact]
        public void TestRemoveAsync_SurveyIdIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveyLanguageTranslationsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(null, languageId)));
        }

        [Fact]
        public void TestRemoveAsync_SurveyIdIsEmpty_ThrowsArgumentException()
        {
            var target = new NfieldSurveyLanguageTranslationsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.RemoveAsync(string.Empty, languageId)));
        }

        [Fact]
        public void TestRemoveAsync_SurveyIdAndLanguageIdSpecified_DoesNotThrow()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, $"Surveys/{SurveyId}/LanguagesTranslations/{languageId}")))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldSurveyLanguageTranslationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Success if test does not throw.
            target.RemoveAsync(SurveyId, languageId).Wait();
        }

        #endregion

    }
}

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
using Nfield.Infrastructure;
using Nfield.Services.Implementation;
using Xunit;
using Nfield.Models;
using Moq;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using System.Linq;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldTranslationsService"/>
    /// </summary>
    public class NfieldTranslationsServiceTests : NfieldServiceTestsBase
    {
        const string SurveyId = "MySurvey";
        const int LanguageId = 10;

        #region AddAsync

        [Fact]
        public void TestAddAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldTranslationsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.AddAsync(null, 0, new Translation())));
        }

        [Fact]
        public void TestAddAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldTranslationsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.AddAsync("", 0, new Translation())));
        }

        [Fact]
        public void TestAddAsync_ServerAcceptsTranslation_ReturnsTranslation()
        {
            var translation = new Translation { Name = "X", Text = "X Translated" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(translation));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(ServiceAddress + "Surveys/" + SurveyId + "/Languages/10/Translations", translation))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldTranslationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddAsync(SurveyId, LanguageId, translation).Result;

            Assert.Equal(translation.Name, actual.Name);
        }

        #endregion

        #region RemoveAsync

        [Fact]
        public void TestRemoveAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldTranslationsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(null, 0, new Translation())));
        }

        [Fact]
        public void TestRemoveAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldTranslationsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.RemoveAsync("", 0, new Translation())));
        }

        [Fact]
        public void TestRemoveAsync_TranslationIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldTranslationsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(SurveyId, LanguageId, null)));
        }

        [Fact]
        public void TestRemoveAsync_ServerRemovedTranslation_DoesNotThrow()
        {
            var translation = new Translation { Name = "X", Text = "Translation X" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.DeleteAsync(ServiceAddress + "Surveys/" + SurveyId + "/Languages/10/Translations/X"))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldTranslationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.DoesNotThrow(() => target.RemoveAsync(SurveyId, LanguageId, translation).Wait());
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldTranslationsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync(null, 0, new Translation())));
        }

        [Fact]
        public void TestUpdateAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldTranslationsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.UpdateAsync("", 0, new Translation())));
        }

        [Fact]
        public void TestUpdateAsync_TranslationIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldTranslationsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync(SurveyId, LanguageId, null)));
        }

        [Fact]
        public void TestUpdateAsync_TranslationExists_DoesNotThrow()
        {
            var translation = new Translation
            {
                Name = "X",
                Text = "Translation X"
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(
                    ServiceAddress + "Surveys/" + SurveyId + "/Languages/10/Translations", It.IsAny<Translation>()))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldTranslationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.DoesNotThrow(() => target.UpdateAsync(SurveyId, LanguageId, translation).Wait());
        }

        #endregion

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldTranslationsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.QueryAsync(null, 0)));
        }

        [Fact]
        public void TestQueryAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldTranslationsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.QueryAsync("", 0)));
        }

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithTranslations()
        {
            var expectedTranslations = new Translation[]
            { new Translation{ Name = "X", Text = "Translation X" },
              new Translation{ Name = "Y", Text = "Translation Y" }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "Surveys/" + SurveyId + "/Languages/10/Translations"))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedTranslations))));

            var target = new NfieldTranslationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualTranslations = target.QueryAsync(SurveyId, LanguageId).Result;

            Assert.Equal(expectedTranslations[0].Name, actualTranslations.ToArray()[0].Name);
            Assert.Equal(expectedTranslations[1].Name, actualTranslations.ToArray()[1].Name);
            Assert.Equal(2, actualTranslations.Count());
        }

        #endregion

        #region DefaultTextsAsync

        [Fact]
        public void TestDefaultTextsAsync_ServerReturnsQuery_ReturnsListWithTranslations()
        {
            var expectedTranslations = new Translation[]
            { new Translation{ Name = "X", Text = "Translation X" },
              new Translation{ Name = "Y", Text = "Translation Y" }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "DefaultTexts"))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedTranslations))));

            var target = new NfieldTranslationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualTranslations = target.DefaultTextsAsync.Result;

            Assert.Equal(expectedTranslations[0].Name, actualTranslations.ToArray()[0].Name);
            Assert.Equal(expectedTranslations[1].Name, actualTranslations.ToArray()[1].Name);
            Assert.Equal(2, actualTranslations.Count());
        }

        #endregion
    }
}

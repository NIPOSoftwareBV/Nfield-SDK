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
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldLanguagesService"/>
    /// </summary>
    public class NfieldLanguagesServiceTests : NfieldServiceTestsBase
    {
        const string SurveyId = "MySurvey";

        #region AddAsync

        [Fact]
        public void TestAddAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldLanguagesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.AddAsync(null, new Language())));
        }

        [Fact]
        public void TestAddAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldLanguagesService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.AddAsync("", new Language())));
        }

        [Fact]
        public void TestAddAsync_ServerAcceptsLanguage_ReturnsLanguage()
        {
            var language = new Language { Name = "Language X" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(language));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(ServiceAddress + "Surveys/" + SurveyId + "/Languages", language))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldLanguagesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddAsync(SurveyId, language).Result;

            Assert.Equal(language.Name, actual.Name);
        }

        #endregion

        #region RemoveAsync

        [Fact]
        public void TestRemoveAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldLanguagesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(null, new Language())));
        }

        [Fact]
        public void TestRemoveAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldLanguagesService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.RemoveAsync("", new Language())));
        }

        [Fact]
        public void TestRemoveAsync_LanguageIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldLanguagesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(SurveyId, null)));
        }

        [Fact]
        public void TestRemoveAsync_ServerRemovedInterviewer_DoesNotThrow()
        {
            const int LanguageId = 11;
            var language = new Language { Id = LanguageId };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.DeleteAsync(ServiceAddress + "Surveys/" + SurveyId + "/Languages/" + LanguageId.ToString()))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldLanguagesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.DoesNotThrow(() => target.RemoveAsync(SurveyId, language).Wait());
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldLanguagesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync(null, new Language())));
        }

        [Fact]
        public void TestUpdateAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldLanguagesService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.UpdateAsync("", new Language())));
        }

        [Fact]
        public void TestUpdateAsync_LanguageIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldLanguagesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync(SurveyId, null)));
        }

        [Fact]
        public void TestUpdateAsync_LanguageExists_DoesNotThrow()
        {
            const int LanguageId = 11;
            var language = new Language
            {
                Id = LanguageId,
                Name = "XXX"
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(
                    ServiceAddress + "Surveys/" + SurveyId + "/Languages", language))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldLanguagesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.DoesNotThrow(() => target.UpdateAsync(SurveyId, language).Wait());
        }

        #endregion

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldLanguagesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.QueryAsync(null)));
        }

        [Fact]
        public void TestQueryAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldLanguagesService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.QueryAsync("")));
        }

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithLanguages()
        {
            var expectedLanguages = new Language[]
            { new Language{Id = 11, Name = "X"},
              new Language{Id = 12, Name = "Y"}
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "Surveys/" + SurveyId + "/Languages"))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedLanguages))));

            var target = new NfieldLanguagesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualLanguages = target.QueryAsync(SurveyId).Result;
            Assert.Equal(expectedLanguages[0].Id, actualLanguages.ToArray()[0].Id);
            Assert.Equal(expectedLanguages[1].Id, actualLanguages.ToArray()[1].Id);
            Assert.Equal(2, actualLanguages.Count());
        }

        #endregion

    }
}

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
    /// Tests for <see cref="NfieldDomainLanguageTranslationsService"/>
    /// </summary>
    public class NfieldDomainLanguageTranslationsServiceTests : NfieldServiceTestsBase
    {
        const int languageId = 123;

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithLanguages()
        {
            var expectedTranslations = new Language[]
            {
                new Language{Name = "X", Id = 1},
                new Language{Name = "Y", Id = 2}
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"LanguageTranslations")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedTranslations))));

            var target = new NfieldDomainLanguageTranslationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualTranslations = target.QueryAsync().Result;
            Assert.Equal(2, actualTranslations.Count());
            Assert.Equal(expectedTranslations[0].Name, actualTranslations.ToArray()[0].Name);
            Assert.Equal(expectedTranslations[0].Id, actualTranslations.ToArray()[0].Id);
            Assert.Equal(expectedTranslations[1].Name, actualTranslations.ToArray()[1].Name);
            Assert.Equal(expectedTranslations[1].Id, actualTranslations.ToArray()[1].Id);
        }

        #endregion

        #region AddAsync

        [Fact]
        public void TestAddAsync_ServerAcceptsLanguageTranslations_ReturnsLanguageTranslations()
        {
            var translations = new DomainLanguageTranslations()
            {
                ButtonNext = "Next",
                ButtonBack = "Back",
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var content = new StringContent(JsonConvert.SerializeObject(translations));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, $"LanguageTranslations"), translations))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldDomainLanguageTranslationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddAsync(translations).Result;

            Assert.Equal(translations.ButtonNext, actual.ButtonNext);
            Assert.Equal(translations.ButtonBack, actual.ButtonBack);
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_LanguageExists_DoesNotThrow()
        {
            var translations = new DomainLanguageTranslations()
            {
                ButtonNext = "El Nexto",
                ButtonBack = "El Backo",
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            var content = new StringContent(JsonConvert.SerializeObject(translations));
            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(new Uri(ServiceAddress, $"LanguageTranslations/{languageId}"), translations))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldDomainLanguageTranslationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.UpdateAsync(languageId, translations).Result;

            Assert.Equal(translations.ButtonNext, actual.ButtonNext);
            Assert.Equal(translations.ButtonBack, actual.ButtonBack);

        }

        #endregion

        #region RemoveAsync

        [Fact]
        public void TestRemoveAsync_LanguageIdSpecified_DoesNotThrow()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, $"LanguagesTranslations/{languageId}")))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldDomainLanguageTranslationsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Success if test does not throw.
            target.RemoveAsync(languageId).Wait();
        }

        #endregion

    }
}

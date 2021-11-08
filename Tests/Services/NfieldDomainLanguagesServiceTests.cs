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
    public class NfieldDomainLanguagesServiceTests : NfieldServiceTestsBase
    {
        #region AddAsync

        [Fact]
        public void TestAddAsync_ServerAcceptsLanguage_ReturnsLanguage()
        {
            var language = new Language { Name = "Language X" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(language));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, "Languages"), language))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldDomainLanguagesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddAsync(language).Result;

            Assert.Equal(language.Name, actual.Name);
        }

        #endregion

        #region RemoveAsync

        [Fact]
        public void TestRemoveAsync_LanguageIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldDomainLanguagesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(null)));
        }

        [Fact]
        public void TestRemoveAsync_ServerRemovedInterviewer_DoesNotThrow()
        {
            const int LanguageId = 11;
            var language = new Language { Id = LanguageId };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, "Languages/" + LanguageId.ToString())))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldDomainLanguagesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // assert: no throw
            target.RemoveAsync(language).Wait();
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_LanguageIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldDomainLanguagesService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync(null)));
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
                    new Uri(ServiceAddress, "Languages"), language))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldDomainLanguagesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // assert: no throw
            target.UpdateAsync(language).Wait();
        }

        #endregion

        #region QueryAsync

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
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Languages")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedLanguages))));

            var target = new NfieldDomainLanguagesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualLanguages = target.QueryAsync().Result;
            Assert.Equal(expectedLanguages[0].Id, actualLanguages.ToArray()[0].Id);
            Assert.Equal(expectedLanguages[1].Id, actualLanguages.ToArray()[1].Id);
            Assert.Equal(2, actualLanguages.Count());
        }

        #endregion

    }
}

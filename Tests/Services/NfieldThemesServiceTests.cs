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

using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services.Implementation;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldThemesServiceTests"/>
    /// </summary>
    public class NfieldThemesServiceTests : NfieldServiceTestsBase
    {
        private const string ThemesServiceAddress = "Themes/";
        private readonly NfieldThemesService _target;

        // Constructor acts as setup
        public NfieldThemesServiceTests()
        {
            _target = new NfieldThemesService();
        }

        #region DownloadThemeAsync
        [Fact]
        public void TestDownloadThemeAsync_ArgumentsNullOrVoid()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.DownloadThemeAsync(null)));
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.DownloadThemeAsync(string.Empty)));
        }

        [Fact]
        public async Task TestDownloadThemeAsync_ServerReturnsDownloadUrl()
        {
            // Arrange
            const HttpStatusCode httpStatusCode = HttpStatusCode.OK;
            const string DownloadUrl = "urlWithThemeFileToDownload";
            var theme = new Theme
            {
                Id = "Id",
                Name = "Name"
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = new Mock<INfieldHttpClient>();
            
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"{ThemesServiceAddress}{theme.Id}")))
                .Returns(
                    Task.Factory.StartNew(
                        () => 
                            new HttpResponseMessage(httpStatusCode)
                            {
                                Content = new StringContent(DownloadUrl)
                            }));
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(base.ServiceAddress);
            
            
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Act
            var response = await _target.DownloadThemeAsync(theme.Id);

            // Assert
            Assert.Equal(response, DownloadUrl);
        }
        #endregion

        #region RemoveAsync

        [Fact]
        public void TestRemoveAsync_ThemeIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.RemoveAsync(null)));
        }

        [Fact]
        public async void TestRemoveAsync_ServerRemoveTheme_DoesNotThrow()
        {
            const string themeId = "ThemeId";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, $"{ThemesServiceAddress}{themeId}")))
                .Returns(CreateTask(HttpStatusCode.OK));

            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // assert: no throw
            await _target.RemoveAsync(themeId);
        }

        #endregion

        #region UploadThemeAsync
        [Fact]
        public void TestUploadThemeFileAsync_ArgumentsNullorVoid()
        {
            const string NotEmptyString = "-";
            Assert.ThrowsAsync<ArgumentNullException>(() => _target.UploadThemeAsync(null, NotEmptyString, NotEmptyString));
            Assert.ThrowsAsync<ArgumentException>(() => _target.UploadThemeAsync(string.Empty, NotEmptyString, NotEmptyString));
            Assert.ThrowsAsync<ArgumentNullException>(() => _target.UploadThemeAsync(NotEmptyString, null,  NotEmptyString));
            Assert.ThrowsAsync<ArgumentException>(() => _target.UploadThemeAsync(NotEmptyString, string.Empty, NotEmptyString));
            Assert.ThrowsAsync<ArgumentNullException>(() => _target.UploadThemeAsync(NotEmptyString, NotEmptyString, (string)null));
            Assert.ThrowsAsync<ArgumentException>(() => _target.UploadThemeAsync(NotEmptyString, string.Empty, NotEmptyString));
        }

        [Fact]
        public void TestUploadThemeContentAsync_ArgumentsNullorVoid()
        {
            const string NotEmptyString = "-";
            var content = Array.Empty<byte>();
            Assert.ThrowsAsync<ArgumentNullException>(() => _target.UploadThemeAsync(null, NotEmptyString, content));
            Assert.ThrowsAsync<ArgumentException>(() => _target.UploadThemeAsync(string.Empty, NotEmptyString, content));
            Assert.ThrowsAsync<ArgumentNullException>(() => _target.UploadThemeAsync(NotEmptyString, null, content));
            Assert.ThrowsAsync<ArgumentException>(() => _target.UploadThemeAsync(NotEmptyString, string.Empty, content));
            Assert.ThrowsAsync<ArgumentNullException>(() => _target.UploadThemeAsync(NotEmptyString, NotEmptyString, (byte[])null));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void TestUploadThemeAsync_ServerUploadTheme_DoesNotThrow(bool uploadFromFile)
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.OK;
            const string ActivityId = "ActivityId";
            const string templateId = "template_id";
            const string themeName = "theme_name";
            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Theme.zip");
            HttpContent httpContent = new HttpMessageContent(new HttpResponseMessage(httpStatusCode));            
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsync(new Uri(ServiceAddress, $"Themes?templateId={templateId}&themeName={themeName}"), It.IsAny<HttpContent>()))
                .Returns(
                Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(httpStatusCode)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { ActivityId = ActivityId }))
                    })).Verifiable();

            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"BackgroundActivities/{ActivityId}")))
                .Returns(Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(httpStatusCode)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { ActivityId = ActivityId , Status = 2 /* Succeeded */ }))
                    })).Verifiable();

            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object).Verifiable();
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(base.ServiceAddress).Verifiable();

            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // assert: no throw
            if (uploadFromFile)
            {
                await _target.UploadThemeAsync(templateId, themeName, inputFilePath);
            }
            else
            {
                await _target.UploadThemeAsync(templateId, themeName, Array.Empty<byte>());
            }
            mockedHttpClient.Verify();
            mockedNfieldConnection.Verify();
        }

        [Fact]
        public async void TestUploadThemeFileAsync_FileDoesNotExist_Throws()
        {
            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "ThemeThatDoesNotExist.zip");
            const string templateId = "template_id";
            const string themeName = "theme_name";

            await Assert.ThrowsAsync<FileNotFoundException>(() => _target.UploadThemeAsync(templateId, themeName, inputFilePath));
        }
    
        #endregion
    }
}

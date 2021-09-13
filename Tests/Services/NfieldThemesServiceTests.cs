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
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services.Implementation;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
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
        public void TestDownloadThemeAsync_ArgumentsNullorVoid()
        {
            const string NotEmptyString = "-";
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.DownloadThemeAsync(null, NotEmptyString, true)));
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.DownloadThemeAsync(string.Empty, NotEmptyString, true)));
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.DownloadThemeAsync(NotEmptyString, null, true)));
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.DownloadThemeAsync(NotEmptyString, string.Empty, true)));
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.DownloadThemeAsync(null, NotEmptyString, false)));
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.DownloadThemeAsync(string.Empty, NotEmptyString, false)));
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.DownloadThemeAsync(NotEmptyString, null, false)));
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.DownloadThemeAsync(NotEmptyString, string.Empty, false)));
        }

        [Fact]
        public async Task TestDownloadThemeAsync_ServerReturnsFile_SaveFile()
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.OK;

            var theme = new Theme
            {
                Id = "Id",
                Name = "Name"
            };

            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Theme.zip");
            string outputFilePath = Path.Combine(Path.GetTempPath(), "Theme.zip");

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = new Mock<INfieldHttpClient>();
            var stream = new FileStream(inputFilePath, FileMode.Open);
            
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"{ThemesServiceAddress}{theme.Id}")))
                .Returns(
                    Task.Factory.StartNew(
                        () =>
                            new HttpResponseMessage(httpStatusCode)
                            {
                                Content = new StreamContent(stream)
                            }));
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(base.ServiceAddress);
            

            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            await _target.DownloadThemeAsync(theme.Id, outputFilePath, true);
            stream.Close();
            stream.Dispose();

            Assert.Equal(GetFileHash(inputFilePath), GetFileHash(outputFilePath));

            stream = new FileStream(inputFilePath, FileMode.Open);

            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"{ThemesServiceAddress}{theme.Id}")))
                .Returns(
                    Task.Factory.StartNew(
                        () =>
                            new HttpResponseMessage(httpStatusCode)
                            {
                                Content = new StreamContent(stream)
                            }));

            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // Assert throws when overwrite is false and the file exists
            await Assert.ThrowsAsync<IOException>( async () =>  await _target.DownloadThemeAsync(theme.Id, outputFilePath, false));

            stream.Close();
            stream.Dispose();
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
            const string ThemeId = "ThemeId";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, $"{ThemesServiceAddress}{ThemeId}")))
                .Returns(CreateTask(HttpStatusCode.OK));

            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // assert: no throw
            await _target.RemoveAsync(ThemeId);
        }

        #endregion

        #region UploadThemeAsync
        [Fact]
        public void TestUploadThemeAsync_ArgumentsNullorVoid()
        {
            const string NotEmptyString = "-";
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.UploadThemeAsync(null, NotEmptyString)));
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.UploadThemeAsync(new Theme(), null)));
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.UploadThemeAsync(new Theme(), string.Empty)));
        }

        [Fact]
        public async void TestUploadThemeAsync_ServerUploadTheme_DoesNotThrow()
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.OK;
            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Theme.zip");
            HttpContent httpContent = new HttpMessageContent(new HttpResponseMessage(httpStatusCode));
            Theme theme = new Theme
            {
                Id = "Theme_Id",
                Name = "Thmeme_Name",
                TemplateId = "Template_Id"
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PostAsync(new Uri(ServiceAddress, $"{ThemesServiceAddress}{theme.Id}"), It.IsAny<HttpContent>()))
                .Returns(
                Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(httpStatusCode)
                    {

                    }));
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(base.ServiceAddress);

            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            // assert: no throw
            await _target.UploadThemeAsync(theme, inputFilePath);
        }

        [Fact]
        public async void TestUploadThemeAsync_FileDoesNotExist_Throws()
        {
            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "ThemeThatDoesNotExist.zip");
            Theme theme = new Theme
            {
                Id = "Theme_Id",
                Name = "Thmeme_Name",
                TemplateId = "Template_Id"
            };
            await Assert.ThrowsAsync<FileNotFoundException>(async () => await _target.UploadThemeAsync(theme, inputFilePath));
        }
    
        #endregion

        private string GetFileHash(string filename)
        {
            var hash = new SHA1Managed();
            var clearBytes = File.ReadAllBytes(filename);
            var hashedBytes = hash.ComputeHash(clearBytes);
            return ConvertBytesToHex(hashedBytes);
        }

        private string ConvertBytesToHex(byte[] bytes)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x"));
            }
            return sb.ToString();
        }
    }
}

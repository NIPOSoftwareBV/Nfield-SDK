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
        [Fact]
        public async Task TestDownloadThemeAsync_ServerReturnsFile_SaveFile()
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.NotFound;

            var theme = new Theme
            {
                Id = "Id",
                Name = "Name",
                TemplateId = "TemplateId"
            };

            string inputFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Theme.zip");
            string outputFilePath = Path.Combine(Path.GetTempPath(), "Theme.zip");

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = new Mock<INfieldHttpClient>();
            var stream = new FileStream(inputFilePath, FileMode.Open);
            
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"Themes/{theme.Id}")))
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
                .Returns(ServiceAddress);
            

            var target = new NfieldThemesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            await target.DownloadThemeAsync(theme.Id, outputFilePath, true);
            stream.Close();
            stream.Dispose();

            Assert.Equal(GetFileHash(inputFilePath), GetFileHash(outputFilePath));
        }

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

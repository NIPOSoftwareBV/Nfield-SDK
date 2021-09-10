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
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Linq;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldTemplatesService"/>
    /// </summary>
    public class NfieldTemplatesServiceTests : NfieldServiceTestsBase
    {
        #region QueryAsync

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithTemplates()
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.NotFound;
            var expectedFieldworkOffices = new Template[]
            {
                new Template
                {
                    Id = "TemplateId1",
                    Name = "TemplateName1",
                    Themes = new Theme[0]
                },
                new Template
                {
                    Id = "TemplateId2",
                    Name = "TemplateName2",
                    Themes = new []
                    {
                        new Theme
                        {
                            Id = "ThemeId1",
                            Name = "ThemeName1"
                        },
                        new Theme
                        {
                            Id = "ThemeId2",
                            Name = "ThemeName2"
                        }
                    }
                }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = new Mock<INfieldHttpClient>();
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Templates/")))
                .Returns(
                    Task.Factory.StartNew(
                        () =>
                            new HttpResponseMessage(httpStatusCode)
                            {
                                Content = new StringContent(JsonConvert.SerializeObject(expectedFieldworkOffices))
                            }));
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(ServiceAddress);


            var target = new NfieldTemplatesService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualTemplates = target.QueryAsync().Result;
    
            Assert.Equal(expectedFieldworkOffices.Length, actualTemplates.Count());
            Assert.Equal(expectedFieldworkOffices[0].Themes.Count(), actualTemplates.First().Themes.Count());
            Assert.Equal(expectedFieldworkOffices[0].Id, actualTemplates.First().Id);
            Assert.Equal(expectedFieldworkOffices[0].Name, actualTemplates.First().Name);
            Assert.Equal(expectedFieldworkOffices[1].Themes.Count(), actualTemplates.Last().Themes.Count());
            Assert.Equal(expectedFieldworkOffices[1].Id, actualTemplates.Last().Id);
            Assert.Equal(expectedFieldworkOffices[1].Name, actualTemplates.Last().Name);
            Assert.Equal(expectedFieldworkOffices[1].Themes.First().Id, actualTemplates.Last().Themes.First().Id);
            Assert.Equal(expectedFieldworkOffices[1].Themes.First().Name, actualTemplates.Last().Themes.First().Name);
            Assert.Equal(expectedFieldworkOffices[1].Themes.Last().Id, actualTemplates.Last().Themes.Last().Id);
            Assert.Equal(expectedFieldworkOffices[1].Themes.Last().Name, actualTemplates.Last().Themes.Last().Name);

        }

        #endregion
    }
}

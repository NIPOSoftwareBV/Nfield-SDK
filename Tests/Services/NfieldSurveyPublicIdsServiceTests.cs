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
    /// Tests for <see cref="NfieldSurveyPublicIdsService"/>
    /// </summary>
    public class NfieldSurveyPublicIdsServiceTests : NfieldServiceTestsBase
    {
        const string SurveyId = "MySurvey";

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyPublicIdsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.QueryAsync(null)));
        }

        [Fact]
        public void TestQueryAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveyPublicIdsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.QueryAsync("")));
        }

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithPublicIds()
        {
            var expectedPublicIds = new []
            { new SurveyPublicId {  LinkType = "X Type", Active = false, Url = "X Url" },
              new SurveyPublicId {  LinkType = "Y Type", Active = true, Url = "Y Url" }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "Surveys/" + SurveyId + "/PublicIds"))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedPublicIds))));

            var target = new NfieldSurveyPublicIdsService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualPublicIds = target.QueryAsync(SurveyId).Result.ToArray(); ;
            Assert.Equal(expectedPublicIds[0].LinkType, actualPublicIds[0].LinkType);
            Assert.Equal(expectedPublicIds[0].Active, actualPublicIds[0].Active);
            Assert.Equal(expectedPublicIds[0].Url, actualPublicIds[0].Url);
            Assert.Equal(expectedPublicIds[1].LinkType, actualPublicIds[1].LinkType);
            Assert.Equal(expectedPublicIds[1].Active, actualPublicIds[1].Active);
            Assert.Equal(expectedPublicIds[1].Url, actualPublicIds[1].Url);
            Assert.Equal(2, actualPublicIds.Length);
        }

        #endregion
    }
}

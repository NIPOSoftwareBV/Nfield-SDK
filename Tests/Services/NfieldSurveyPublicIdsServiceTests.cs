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
using System.Collections.Generic;
using System.Globalization;
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
        private readonly NfieldSurveyPublicIdsService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;

        const string SurveyId = "MySurvey";

        public NfieldSurveyPublicIdsServiceTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldSurveyPublicIdsService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);
        }

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.QueryAsync(null)));
        }

        [Fact]
        public void TestQueryAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.QueryAsync("")));
        }

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithPublicIds()
        {
            var expectedPublicIds = new []
            { new SurveyPublicId {  LinkType = "X Type", Active = false, Url = "X Url" },
              new SurveyPublicId {  LinkType = "Y Type", Active = true, Url = "Y Url" }
            };
            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/" + SurveyId + "/PublicIds")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedPublicIds))));


            var actualPublicIds = _target.QueryAsync(SurveyId).Result.ToArray(); ;
            Assert.Equal(expectedPublicIds[0].LinkType, actualPublicIds[0].LinkType);
            Assert.Equal(expectedPublicIds[0].Active, actualPublicIds[0].Active);
            Assert.Equal(expectedPublicIds[0].Url, actualPublicIds[0].Url);
            Assert.Equal(expectedPublicIds[1].LinkType, actualPublicIds[1].LinkType);
            Assert.Equal(expectedPublicIds[1].Active, actualPublicIds[1].Active);
            Assert.Equal(expectedPublicIds[1].Url, actualPublicIds[1].Url);
            Assert.Equal(2, actualPublicIds.Length);
        }

        #endregion

        #region PutAsync
        [Fact]
        public void TestPutAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.PutAsync(null, new List<SurveyPublicId>())));
        }

        [Fact]
        public void TestPutAsync_Always_CallsCorrectURI()
        {
            var expectedUrl = new Uri(ServiceAddress, string.Format(CultureInfo.InvariantCulture, "Surveys/{0}/PublicIds",
                SurveyId));
            
            _mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(expectedUrl, It.IsAny<IEnumerable<SurveyPublicId>>()))
                .Returns(CreateTask(HttpStatusCode.OK));
            
            _target.PutAsync(SurveyId, null);

            _mockedHttpClient
                .Verify(client => client.PutAsJsonAsync(expectedUrl, It.IsAny<IEnumerable<SurveyPublicId>>()), Times.Once());
        }

        #endregion
    }
}

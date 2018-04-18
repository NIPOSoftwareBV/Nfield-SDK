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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveysSearchService"/>
    /// </summary>
    public class NfieldSurveysSearchServiceTests : NfieldServiceTestsBase
    {
        private readonly NfieldSurveysSearchService _target;
        private readonly Mock<INfieldHttpClient> _mockedHttpClient;

        private const string SurveyId = "SurveyId";
        private const string SurveyName = "SurveyName";

        public NfieldSurveysSearchServiceTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldSurveysSearchService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);
        }

        #region GetAsync

        [Fact]
        public void TestGetAsync_SearchValueIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.GetAsync(null)));
        }

        [Fact]
        public void TestGetAsync_SearchValueIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.GetAsync(string.Empty)));
        }

        [Fact]
        public void TestGetAsync_SearchValue_ReturnsRespondentSurveys()
        {
            const string searchValue = "email@nipo.com";

            var expectedSurvey = new SurveyBase
            {
                SurveyId = SurveyId,
                SurveyName = SurveyName
            };

            var expectedResult = new List<SurveyBase> { expectedSurvey };

            _mockedHttpClient
                .Setup(client => client.GetAsync($"{ServiceAddress}/Surveys/Search/{searchValue}"))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedResult))));

            var result = _target.GetAsync(searchValue).Result;

            Assert.NotNull(result);
            Assert.True(result.Any());
            Assert.Equal(expectedSurvey.SurveyId, result.First().SurveyId);
            Assert.Equal(expectedSurvey.SurveyName, result.First().SurveyName);
        }

        #endregion
    }
}

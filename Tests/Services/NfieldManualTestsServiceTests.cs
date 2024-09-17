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
using Nfield.SDK.Models;
using Nfield.SDK.Services.Implementation;
using Nfield.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldManualTestsService"/>
    /// </summary>
    public class NfieldManualTestsServiceTests : NfieldServiceTestsBase
    {
        private readonly NfieldManualTestsService _target;
        private readonly Mock<INfieldHttpClient> _mockedHttpClient;

        public NfieldManualTestsServiceTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldManualTestsService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);
        }

        [Fact]
        public void TestGetAsync_ServerReturnsQuery_ReturnsListWithTestSurveys()
        {
            var endPoint = new Uri(ServiceAddress, "ManualTests");

            var expectedSurveys = new[]
            {
                new SurveyManualTest(SurveyType.Basic) { SurveyId = Guid.NewGuid().ToString() },
                new SurveyManualTest(SurveyType.Advanced) { SurveyId = Guid.NewGuid().ToString() }
            };

            _mockedHttpClient
                .Setup(client => client.GetAsync(endPoint))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSurveys))));

            var actualSurveys = _target.GetAsync().Result;

            Assert.Equal(2, actualSurveys.Count());
            Assert.Equal(expectedSurveys[0].SurveyId, actualSurveys.ToArray()[0].SurveyId);
            Assert.Equal(expectedSurveys[1].SurveyId, actualSurveys.ToArray()[1].SurveyId);
        }
    }
}

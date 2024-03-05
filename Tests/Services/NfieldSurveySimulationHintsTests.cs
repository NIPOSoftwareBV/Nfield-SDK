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
using Nfield.Services.Implementation;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveySimulationHintsService"/>
    /// </summary>
    public class NfieldSurveySimulationHintsTests : NfieldServiceTestsBase
    {
        private readonly NfieldSurveySimulationHintsService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;
        private readonly string _surveyId;
        private readonly Uri _endpoint;

        public NfieldSurveySimulationHintsTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldSurveySimulationHintsService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            _surveyId = Guid.NewGuid().ToString();
            _endpoint = new Uri(ServiceAddress, $"surveys/{_surveyId}/retrieve-simulation-hints-url");
        }

        [Fact]
        public async Task TestGetAsync()
        {
            const string uri = "https://nfieldpurple.blob.core.windows.net/survey-simulation-hints/2f34c076-da0a-4b75-98a9-306088025668";
            var content = new StringContent(uri);

            _mockedHttpClient
                .Setup(client => client.GetAsync(_endpoint))
                .Returns(CreateTask(HttpStatusCode.OK, content))
                .Verifiable();

            var actual = await _target.GetAsync(_surveyId);
            Assert.Equal(actual, new Uri(uri));
        }
    }
}

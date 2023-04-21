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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldDataRetentionService"/>
    /// </summary>
    public class NfieldDataRetentionTests : NfieldServiceTestsBase
    {
        private readonly NfieldDataRetentionService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;
        private readonly string _surveyId;
        private readonly Uri _endpoint;

        public NfieldDataRetentionTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldDataRetentionService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            _surveyId = Guid.NewGuid().ToString();
            _endpoint = new Uri(ServiceAddress, $"surveys/{_surveyId}/DataRetentionSettings");
        }

        [Fact]
        public async Task TestGetAsync()
        {
            var dataRetentionSettings = new DataRetentionSettings { RetentionPeriod = 30, PossibleValues = new int[] { 0,30,60,90 } };
            var content = new StringContent(JsonConvert.SerializeObject(dataRetentionSettings));

            _mockedHttpClient
                .Setup(client => client.GetAsync(_endpoint))
                .Returns(CreateTask(HttpStatusCode.OK, content))
                .Verifiable();

            var actual = await _target.GetAsync(_surveyId);
            Assert.Equal(dataRetentionSettings.RetentionPeriod, actual.RetentionPeriod);
            Assert.Equal(dataRetentionSettings.PossibleValues, actual.PossibleValues);
        }

        [Fact]
        public async Task TestPutAsync()
        {
            int retentionPeriod = 30;

            _mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(_endpoint, new { RetentionPeriod = retentionPeriod }))
                .Returns(CreateTask(HttpStatusCode.NoContent))
                .Verifiable();

            await _target.PutAsync(_surveyId, retentionPeriod);
        }
    }
}

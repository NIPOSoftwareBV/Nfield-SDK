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
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldThemesServiceTests"/>
    /// </summary>
    public class NfieldSamplingPointsServiceTests : NfieldServiceTestsBase
    {
        private const string SurveyId = "survey-id";
        private const string UriSP = "Surveys/survey-id/SamplingPoints";
        private readonly NfieldSamplingPointsService _target;
        private readonly Mock<INfieldConnectionClient> _mockedNfieldConnection;
        private readonly Mock<INfieldHttpClient> _mockedHttpClient;
        private readonly SamplingPoint _samplingPoint;

        // Constructor acts as setup
        public NfieldSamplingPointsServiceTests()
        {
            _mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(_mockedNfieldConnection);
            _target = new NfieldSamplingPointsService();
            _target.InitializeNfieldConnection(_mockedNfieldConnection.Object);
            _samplingPoint = new SamplingPoint()
            {
                Name = "New SP",
                Kind = SamplingPointKind.Spare,
                Description = "desc",
                Instruction = "instrction",
                FieldworkOfficeId = "office-id",
                SamplingPointId = "sp-id",
                Stratum = "stratum",
                GroupId = "group-id"
            };
        }

        #region CreateAsync

        [Fact]
        public void TestCreateAsync_ArgNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.CreateAsync(SurveyId, null)));
        }

        [Fact]
        public async Task TestCreateAsync_ServerAccepts_ReturnsSP()
        {   
            
            var content = new StringContent(JsonConvert.SerializeObject(_samplingPoint));
            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, UriSP), _samplingPoint))
                .Returns(CreateTask(HttpStatusCode.OK, content));


            var actual = await _target.CreateAsync(SurveyId, _samplingPoint);

            Assert.Equal(_samplingPoint.Name, actual.Name);
            Assert.Equal(_samplingPoint.Kind, actual.Kind);
            Assert.Equal(_samplingPoint.SamplingPointId, actual.SamplingPointId);
            Assert.Equal(_samplingPoint.FieldworkOfficeId, actual.FieldworkOfficeId);
            Assert.Equal(_samplingPoint.GroupId, actual.GroupId);
            Assert.Equal(_samplingPoint.Instruction, actual.Instruction);
            Assert.Equal(_samplingPoint.Stratum, actual.Stratum);
            Assert.Equal(_samplingPoint.Description, actual.Description);
        }

        #endregion

        #region RemoveAsync        

        [Fact]
        public async Task TestRemoveAsync_ServerAccepts_ReturnsSP()
        {

            var content = new StringContent(JsonConvert.SerializeObject(_samplingPoint));
            _mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, $"{UriSP}/{_samplingPoint.SamplingPointId}")))
                .Returns(CreateTask(HttpStatusCode.OK, content));


            Assert.True(await _target.RemoveAsync(SurveyId, _samplingPoint.SamplingPointId));            
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_ArgNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.UpdateAsync(null, new SamplingPoint())));
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.UpdateAsync(SurveyId, null)));
        }

        [Fact]
        public async Task TestUpdateAsync_ServerAccepts_ReturnsSP()
        {

            var content = new StringContent(JsonConvert.SerializeObject(_samplingPoint));
            _mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(new Uri(ServiceAddress, $"{UriSP}/{_samplingPoint.SamplingPointId}"), _samplingPoint))
                .Returns(CreateTask(HttpStatusCode.OK, content));


            var actual = await _target.UpdateAsync(SurveyId, _samplingPoint);

            Assert.Equal(_samplingPoint.Name, actual.Name);
            Assert.Equal(_samplingPoint.Kind, actual.Kind);
            Assert.Equal(_samplingPoint.SamplingPointId, actual.SamplingPointId);
            Assert.Equal(_samplingPoint.FieldworkOfficeId, actual.FieldworkOfficeId);
            Assert.Equal(_samplingPoint.GroupId, actual.GroupId);
            Assert.Equal(_samplingPoint.Instruction, actual.Instruction);
            Assert.Equal(_samplingPoint.Stratum, actual.Stratum);
            Assert.Equal(_samplingPoint.Description, actual.Description);
        }

        #endregion

        #region GetAsync       

        [Fact]
        public async Task TestGetAsync_ServerAccepts_ReturnsSP()
        {

            var content = new StringContent(JsonConvert.SerializeObject(_samplingPoint));
            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, $"{UriSP}/{_samplingPoint.SamplingPointId}")))
                .Returns(CreateTask(HttpStatusCode.OK, content));


            var actual = await _target.GetAsync(SurveyId, _samplingPoint.SamplingPointId);

            Assert.Equal(_samplingPoint.Name, actual.Name);
            Assert.Equal(_samplingPoint.Kind, actual.Kind);
            Assert.Equal(_samplingPoint.SamplingPointId, actual.SamplingPointId);
            Assert.Equal(_samplingPoint.FieldworkOfficeId, actual.FieldworkOfficeId);
            Assert.Equal(_samplingPoint.GroupId, actual.GroupId);
            Assert.Equal(_samplingPoint.Instruction, actual.Instruction);
            Assert.Equal(_samplingPoint.Stratum, actual.Stratum);
            Assert.Equal(_samplingPoint.Description, actual.Description);
        }

        #endregion

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_ArgNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.QueryAsync(null)));
        }

        [Fact]
        public async Task TestQueryAsync_ServerAccepts_ReturnsSP()
        {

            var content = new StringContent(JsonConvert.SerializeObject(new[] { _samplingPoint }));
            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, UriSP)))
                .Returns(CreateTask(HttpStatusCode.OK, content));


            var actual = await _target.QueryAsync(SurveyId);

            Assert.Equal(1, actual.Count());
            Assert.Equal(_samplingPoint.Name, actual.First().Name);
            Assert.Equal(_samplingPoint.Kind, actual.First().Kind);
            Assert.Equal(_samplingPoint.SamplingPointId, actual.First().SamplingPointId);
            Assert.Equal(_samplingPoint.FieldworkOfficeId, actual.First().FieldworkOfficeId);
            Assert.Equal(_samplingPoint.GroupId, actual.First().GroupId);
            Assert.Equal(_samplingPoint.Instruction, actual.First().Instruction);
            Assert.Equal(_samplingPoint.Stratum, actual.First().Stratum);
            Assert.Equal(_samplingPoint.Description, actual.First().Description);
        }

        #endregion

    }
}

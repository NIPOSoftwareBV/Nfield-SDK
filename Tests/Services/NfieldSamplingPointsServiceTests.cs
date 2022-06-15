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
using Nfield.Services.Implementation;
using System;
using System.Collections.Generic;
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
        private const string UriActivateSPs = "Surveys/survey-id/ActivateSamplingPoints";
        private readonly NfieldSamplingPointsService _target;
        private readonly Mock<INfieldConnectionClient> _mockedNfieldConnection;
        private readonly Mock<INfieldHttpClient> _mockedHttpClient;
        private readonly SamplingPoint _samplingPoint;
        private readonly SamplingPoint _samplingPoint2;

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
                Instruction = "instruction",
                FieldworkOfficeId = "office-id",
                SamplingPointId = "sp-id",
                Stratum = "stratum",
                GroupId = "group-id"
            };
            _samplingPoint2 = new SamplingPoint()
            {
                Name = "New SP2",
                Kind = SamplingPointKind.Spare,
                Description = "desc2",
                Instruction = "instruction2",
                FieldworkOfficeId = "office-id-2",
                SamplingPointId = "sp-id-2",
                Stratum = "stratum2",
                GroupId = "group-id-2"
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
        public async Task TestRemoveAsync_ServerAccepts_ReturnsTrue()
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
                .Setup(client => client.PatchAsJsonAsync(new Uri(ServiceAddress, $"{UriSP}/{_samplingPoint.SamplingPointId}"),
                It.Is<UpdateSamplingPoint>(s => s.Name == _samplingPoint.Name)))
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
        public async Task TestQueryAsync_ServerAccepts_ReturnsSPs()
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

        #region ActivateAsync

        [Fact]
        public void TestActivateAsync_ArgNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.ActivateAsync(SurveyId, null)));
        }

        [Fact]
        public async Task TestActivateAsync_SPList_ReturnsTrue()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new ActivateSpareSamplingPointsResponseModel { IsActivated = true }));
            _mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, UriActivateSPs),
                It.Is<object>(e => (GetPropertyValue(e, "SamplingPointIds") as IEnumerable<String>).First() == _samplingPoint.SamplingPointId)))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            Assert.True(await _target.ActivateAsync(SurveyId, new[] { _samplingPoint.SamplingPointId }));
        }      

        [Fact]
        public async Task TestActivateAsync_OneSP_targetNull_ReturnsSP()
        {
            _samplingPoint.Kind = SamplingPointKind.SpareActive;
            var content = new StringContent(JsonConvert.SerializeObject( new ActivateSpareSamplingPointsResponseModel { IsActivated = true } ));
            _samplingPoint.Kind = SamplingPointKind.Spare;
            _mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(new Uri(ServiceAddress, $"{UriSP}/{_samplingPoint.SamplingPointId}/Activate"), 
                It.Is<object>(o => GetPropertyValue(o, "Target") == null)))
                .Returns(CreateTask(HttpStatusCode.OK, content));


            var res = await _target.ActivateAsync(SurveyId, _samplingPoint.SamplingPointId, null);

            Assert.True( res);           
        }
        [Fact]
        public async Task TestActivateAsync_OneSP_targetNotNull_ReturnsSP()
        {
            int target = 2;
            _samplingPoint.Kind = SamplingPointKind.SpareActive;
            var content = new StringContent(JsonConvert.SerializeObject(new ActivateSpareSamplingPointsResponseModel { IsActivated = true }));
            _samplingPoint.Kind = SamplingPointKind.Spare;
            _mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(new Uri(ServiceAddress, $"{UriSP}/{_samplingPoint.SamplingPointId}/Activate"),
                It.Is<object>(o => (int)GetPropertyValue(o, "Target") == target)))
                .Returns(CreateTask(HttpStatusCode.OK, content));


            var res = await _target.ActivateAsync(SurveyId, _samplingPoint.SamplingPointId, target);

            Assert.True(res);            
        }


        #endregion

        #region ReplaceAsync


        [Fact]
        public async Task TestReplaceAsync_OneSP_targetNull_ReturnsSP()
        {
            var content = new StringContent(JsonConvert.SerializeObject(new ReplaceSamplingPointWithSpareResponseModel { Success = true }));
            _mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(new Uri(ServiceAddress, $"{UriSP}/{_samplingPoint.SamplingPointId}/Replace"),
                It.Is<object>(o => GetPropertyValue(o, "Target") == null &&
                GetPropertyValue(o, "SpareSamplingPointId").ToString() == _samplingPoint2.SamplingPointId)))
                .Returns(CreateTask(HttpStatusCode.OK, content));


            var res = await _target.ReplaceAsync(SurveyId, _samplingPoint.SamplingPointId, _samplingPoint2.SamplingPointId, null);

            Assert.True(res);     
        }
        [Fact]
        public async Task TestReplaceAsync_OneSP_targetNotNull_ReturnsSP()
        {           
            int target = 2;
            var content = new StringContent(JsonConvert.SerializeObject(new ReplaceSamplingPointWithSpareResponseModel { Success = true }));
            _mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(new Uri(ServiceAddress, $"{UriSP}/{_samplingPoint.SamplingPointId}/Replace"),
                It.Is<object>(o => (int)GetPropertyValue(o, "Target") == target &&
                GetPropertyValue(o, "SpareSamplingPointId").ToString() == _samplingPoint2.SamplingPointId)))
                .Returns(CreateTask(HttpStatusCode.OK, content));


            var res = await _target.ReplaceAsync(SurveyId, _samplingPoint.SamplingPointId, _samplingPoint2.SamplingPointId, target);

            Assert.True(res);
        }


        #endregion

        private object GetPropertyValue(object e, string propertyname)
        {
            var val = e.GetType().GetProperties().First(p => p.Name == propertyname).GetValue(e);
            return val;
        }

    }
}

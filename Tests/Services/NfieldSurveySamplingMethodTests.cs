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
    /// Tests for <see cref="NfieldSurveySampleService"/>
    /// </summary>
    public class NfieldSurveySamplingMethodTests : NfieldServiceTestsBase
    {
        private const string SurveyId = "MySurvey";

        private readonly NfieldSurveySamplingMethodService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;

        public NfieldSurveySamplingMethodTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldSurveySamplingMethodService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);
        }

        private Uri ServiceRequestUri()
        {
            string relativeUri = $"Surveys/{SurveyId}/SamplingMethod/";

            return new Uri(ServiceAddress, relativeUri);
        }

        #region GetAsync

        [Fact]
        public void TestGetAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.GetAsync(null)));
        }

        [Fact]
        public void TestGetAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.GetAsync("   ")));
        }

        [Fact]
        public void TestGetAsync_SurveyIdIsValid_ReturnsSamplingMethod()
        {
            var expectedModel = new SamplingMethodModel
            {
                SamplingMethod = SamplingMethodType.FreeIntercept.ToString()
            };

            _mockedHttpClient
                .Setup(client => client.GetAsync(ServiceRequestUri()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedModel))));

            var actual = _target.GetAsync(SurveyId).Result;

            Assert.Equal(expectedModel.SamplingMethod, actual.ToString());
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_SurveyIdIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.UpdateAsync(null, SamplingMethodType.FreeIntercept)));
        }

        [Fact]
        public void TestUpdateAsync_SurveyIdIsEmpty_Throws()
        {
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.UpdateAsync("   ", SamplingMethodType.FreeIntercept)));
        }

        [Fact]
        public void TestUpdateAsync_SurveyIdIsValid_RunsToCompletion()
        {
            var expectedTaskStatus = TaskStatus.RanToCompletion;

            _mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(ServiceRequestUri(), It.IsAny<SamplingMethodType>()))
                .Returns(CreateTask(HttpStatusCode.OK));

            var actual = _target.UpdateAsync(SurveyId, SamplingMethodType.FreeIntercept);

            Assert.Equal(expectedTaskStatus, actual.Status);
        }

        #endregion
    }
}

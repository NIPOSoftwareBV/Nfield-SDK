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
    public class NfieldInterviewQualityTests : NfieldServiceTestsBase
    {
        private const string SurveyId = "MySurvey";
        private const string InterviewId = "InterviewId";
        private readonly NfieldInterviewQualityService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;

        public NfieldInterviewQualityTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldInterviewQualityService();
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
        public void TestQueryAsync_SurveyIdIsNull_InterviewIdIsNull_Throws()
        {
            
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.QueryAsync(null, null)));
        }

        [Fact]
        public void TestQueryAsync_SurveyIdIsEmpty_InterveiwIdIsEmpty_Throws()
        {
           
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.QueryAsync("", "")));
        }

        [Fact]
        public void TestQueryAsync_InterviewIdIsNull_Throws()
        {
           
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(_target.QueryAsync(SurveyId, null)));
        }

        [Fact]
        public void TestQueryAsync_InterveiwIdIsEmpty_Throws()
        {
            
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(_target.QueryAsync(SurveyId, "")));
        }


        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithInterviewQualities()
        {
            var fakeInterviewDetails = new InterviewDetailsModel[]
            {
                new InterviewDetailsModel
                {
                    Id = "1",
                    InterviewQuality = (InterviewQuality) 1,
                    InterviewerId = "00000001",
                    OfficeId = "OfficeId1",
                    SamplingPointId = "SamplingPointId1",
                    StartDate = DateTime.UtcNow
                },
                new InterviewDetailsModel
                {
                    Id = "2",
                    InterviewQuality = (InterviewQuality) 2,
                    InterviewerId = "00000002",
                    OfficeId = "OfficeId2",
                    SamplingPointId = "SamplingPointId2",
                    StartDate = DateTime.UtcNow
                }
            };
            
            _mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "Surveys/" + SurveyId + "/InterviewQuality"))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(fakeInterviewDetails))));

            var actualInterviewDetails = _target.QueryAsync(SurveyId).Result;
            Assert.Equal(fakeInterviewDetails[0].Id, actualInterviewDetails.ToArray()[0].Id);
            Assert.Equal(fakeInterviewDetails[1].Id, actualInterviewDetails.ToArray()[1].Id);
            Assert.Equal(2, actualInterviewDetails.Count());
        }

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithInterviewQuality()
        {
            var fakeInterviewDetail = new InterviewDetailsModel
            {
                Id = "1",
                InterviewQuality = (InterviewQuality) 1,
                InterviewerId = "00000001",
                OfficeId = "OfficeId1",
                SamplingPointId = "SamplingPointId1",
                StartDate = DateTime.UtcNow
            };

           _mockedHttpClient
                .Setup(
                    client =>
                        client.GetAsync(ServiceAddress + "Surveys/" + SurveyId + "/InterviewQuality/" + InterviewId))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(fakeInterviewDetail))));

            var actualInterviewDetails = _target.QueryAsync(SurveyId,InterviewId).Result;
            Assert.Equal(fakeInterviewDetail.Id, actualInterviewDetails.Id);
        }

        #endregion

        #region PutAsync

        [Fact]
        public void TestPutAsync_Always_CallsCorrectURI()
        {
            _mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<QualityNewStateChange>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(new InterviewDetailsModel()))));

            _target.PutAsync(SurveyId, InterviewId, 1).Wait();
            _mockedHttpClient
                .Verify(client => client.PutAsJsonAsync(ServiceAddress + "Surveys/" + SurveyId + "/InterviewQuality", It.IsAny<QualityNewStateChange>()), Times.Once());
        }

        #endregion
    }
}

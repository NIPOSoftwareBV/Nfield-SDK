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
    /// Tests for <see cref="NfieldSurveysService"/>
    /// </summary>
    public class NfieldSurveyQuotaFrameServiceTests : NfieldServiceTestsBase
    {
        private const string SurveyId = "SurveyId";
        private const string QuotaControllerName = "SurveyQuotaFrame";
        private const long ETag = 1234;
        private Guid VariableId;
        private Guid VariableDefinitionId;
        private Guid LevelId;
        const int TargetCount = 15;
        private Mock<INfieldConnectionClient> _mockedNfieldConnection;
        private Mock<INfieldHttpClient> _mockedHttpClient;

        private SurveyQuotaFrameModel ExpectedQuotaFrame;
        private NfieldSurveyQuotaFrameService _target;

        public NfieldSurveyQuotaFrameServiceTests()
        {
            VariableId = Guid.NewGuid();
            VariableDefinitionId = Guid.NewGuid();
            LevelId = Guid.NewGuid();
            _target = new NfieldSurveyQuotaFrameService();
            _mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(_mockedNfieldConnection);

            ExpectedQuotaFrame = new SurveyQuotaFrameModel
            {
                Id = SurveyId,
                QuotaETag = ETag,
                Target = TargetCount,
                FrameVariables = new List<SurveyQuotaFrameVariableModel>()
                                    {
                                        new SurveyQuotaFrameVariableModel()
                                        {
                                            Id = VariableId,
                                            Levels = new List<SurveyQuotaFrameLevelModel>()
                                            {
                                                new SurveyQuotaFrameLevelModel() { Id = LevelId }
                                            }
                                        }
                                    },
                VariableDefinitions = new List<SurveyQuotaVariableDefinitionModel>()
                {
                        new SurveyQuotaVariableDefinitionModel() { Id = VariableDefinitionId }
                }
            };
        }

        [Fact]
        public async Task TestQuotaQueryAsync_ServerReturnsQuotaFrame()
        {
            var uri = new Uri(ServiceAddress, $"Surveys/{SurveyId}/{QuotaControllerName}");

            _mockedHttpClient
                .Setup(client => client.GetAsync(uri))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(ExpectedQuotaFrame))));

            _target.InitializeNfieldConnection(_mockedNfieldConnection.Object);

            var responseQuotaFrame = await _target.QuotaQueryAsync(SurveyId);
            _mockedHttpClient.Verify(hc => hc.GetAsync(It.Is<Uri>(u => u.ToString() == uri.ToString())), Times.Once());
            Assert.Equal(SurveyId, responseQuotaFrame.Id);
            Assert.Equal(ETag, responseQuotaFrame.QuotaETag);
            Assert.Equal(VariableDefinitionId, responseQuotaFrame.VariableDefinitions.FirstOrDefault().Id);
            Assert.Equal(VariableId, responseQuotaFrame.FrameVariables.FirstOrDefault().Id);
            Assert.Equal(LevelId, responseQuotaFrame.FrameVariables.FirstOrDefault().Levels.FirstOrDefault().Id);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("       ")]
        public async Task TestQuotaQueryAsync_ValidateSurveyId(string surveyId)
        {
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => { _ = await _target.QuotaQueryAsync(surveyId); });

            Assert.Equal("surveyId cannot be null or empty", ex.Message);
        }


        [Fact]
        public async Task TestUpdateQuotaTargetsAsync_ServerReturnsLevelListUpdated()
        {
            var quotaFrameEtag = new SurveyQuotaFrameEtagModel() { Levels = new List<SurveyQuotaFrameEtagLevelTargetModel>() { new SurveyQuotaFrameEtagLevelTargetModel() { Id = LevelId.ToString() } } };

            var uri = new Uri(ServiceAddress, $"Surveys/{SurveyId}/{QuotaControllerName}/{ETag}");

            _mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(uri, It.Is<SurveyQuotaFrameEtagModel>(i => i.Levels.FirstOrDefault().Id == LevelId.ToString())))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(quotaFrameEtag))));

            _target.InitializeNfieldConnection(_mockedNfieldConnection.Object);

            var newQuotaFrameEtag = await _target.UpdateQuotaTargetsAsync(SurveyId, ETag.ToString(), quotaFrameEtag);
            _mockedHttpClient.Verify(hc => hc.PutAsJsonAsync(It.Is<Uri>(u => u.ToString() == uri.ToString()), It.Is<SurveyQuotaFrameEtagModel>(i => i.Levels.FirstOrDefault().Id == LevelId.ToString())), Times.Once());
            Assert.Equal(LevelId.ToString(), newQuotaFrameEtag.Levels.FirstOrDefault().Id);

        }

        [Theory]
        [InlineData(null)]
        [InlineData("       ")]
        public async Task TestUpdateQuotaTargetsAsync_ValidateSurveyId(string surveyId)
        {
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => { _ = await _target.UpdateQuotaTargetsAsync(surveyId, ETag.ToString(), new SurveyQuotaFrameEtagModel()); });

            Assert.Equal("surveyId cannot be null or empty", ex.Message);
        }

        [Fact]
        public async Task TestCreateOrUpdateQuotaAsync_ReturnsCreatedUpdatedQuotaFrame()
        {
            var uri = new Uri(ServiceAddress, $"Surveys/{SurveyId}/{QuotaControllerName}");

            _mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(uri, It.Is<SurveyQuotaFrameModel>(i => i.Id == SurveyId)))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(ExpectedQuotaFrame))));

            _target.InitializeNfieldConnection(_mockedNfieldConnection.Object);

            var responseQuotaFrame = await _target.CreateOrUpdateQuotaAsync(SurveyId, ExpectedQuotaFrame);
            _mockedHttpClient.Verify(hc => hc.PutAsJsonAsync(It.Is<Uri>(u => u.ToString() == uri.ToString()), It.Is<SurveyQuotaFrameModel>(i => i.Id == SurveyId)), Times.Once());
            Assert.Equal(SurveyId, responseQuotaFrame.Id);
            Assert.Equal(ETag, responseQuotaFrame.QuotaETag);
            Assert.Equal(VariableDefinitionId, responseQuotaFrame.VariableDefinitions.FirstOrDefault().Id);
            Assert.Equal(VariableId, responseQuotaFrame.FrameVariables.FirstOrDefault().Id);
            Assert.Equal(LevelId, responseQuotaFrame.FrameVariables.FirstOrDefault().Levels.FirstOrDefault().Id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("       ")]
        public async Task TestCreateOrUpdateQuotaAsync_ValidateSurveyId(string surveyId)
        {
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => { _ = await _target.CreateOrUpdateQuotaAsync(surveyId, new SurveyQuotaFrameModel()); });

            Assert.Equal("surveyId cannot be null or empty", ex.Message);
        }
    }
}

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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Quota;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveysService"/>
    /// </summary>
    public class NfieldSurveysServiceTests : NfieldServiceTestsBase
    {
        #region QueryAsync

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithSurveys()
        {
            var expectedSurveys = new[]
            { new Survey(SurveyType.Basic) { SurveyId = "TestSurvey" },
              new Survey(SurveyType.Advanced) { SurveyId = "AnotherTestSurvey" }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSurveys))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSurveys = target.QueryAsync().Result;

            Assert.Equal(expectedSurveys[0].SurveyId, actualSurveys.ToArray()[0].SurveyId);
            Assert.Equal(expectedSurveys[1].SurveyId, actualSurveys.ToArray()[1].SurveyId);
            Assert.Equal(2, actualSurveys.Count());
        }

        #endregion

        #region AddAsync

        [Fact]
        public void TestAddAsync_SurveyIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveysService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(null)));
        }

        [Fact]
        public void TestAddAsync_ServerAccepts_ReturnsSurvey()
        {
            var survey = new Survey(SurveyType.Basic) { SurveyName = "New Survey" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(survey));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(new Uri(ServiceAddress, "Surveys/"), survey))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddAsync(survey).Result;

            Assert.Equal(survey.SurveyName, actual.SurveyName);
            Assert.Equal(survey.SurveyType, actual.SurveyType);
        }

        #endregion

        #region RemoveAsync

        [Fact]
        public void TestRemoveAsync_SurveyIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveysService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(null)));
        }

        [Fact]
        public void TestRemoveAsync_ServerRemovedSurvey_DoesNotThrow()
        {
            const string surveyId = "Survey X";
            var survey = new Survey(SurveyType.Basic) { SurveyId = surveyId };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, "Surveys/" + surveyId)))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.DoesNotThrow(() => target.RemoveAsync(survey).Wait());
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public void TestUpdateAsync_SurveyArgumentIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveysService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync(null)));
        }

        [Fact]
        public void TestUpdateAsync_InterviewerExists_ReturnsInterviewer()
        {
            const string surveyId = "aSurveyId";
            var survey = new Survey(SurveyType.Basic)
            {
                SurveyId = surveyId,
                Description = "updated description"
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(new Uri(ServiceAddress, "Surveys/" + surveyId), It.IsAny<UpdateSurvey>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(survey))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.UpdateAsync(survey).Result;

            Assert.Equal(survey.Description, actual.Description);
        }

        #endregion

        #region UploadInterviewerFileInstructionsAsync

        [Fact]
        public void TestUploadInterviewerInstructionsAsync_FileDoesNotExist_ThrowsFileNotFoundException()
        {
            var target = new NfieldSurveysService();
            Assert.Throws<FileNotFoundException>(
                () =>
                    UnwrapAggregateException(target.UploadInterviewerFileInstructionsAsync("NotExistingFile.pdf",
                        "surveyId")));
        }

        [Fact]
        public void TestUploadInterviewerInstructionsAsync_FileExists_FileUpload()
        {
            const string surveyId = "SurveyId";

            const string fileName = "asp.net-web-api-poster.pdf";
            var file = Path.Combine(Directory.GetCurrentDirectory(), "Resources", fileName);

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient.Setup(client => client.PostAsync(It.IsAny<Uri>(), It.IsAny<HttpContent>()))
                            .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.UploadInterviewerFileInstructionsAsync(file, surveyId);

            mockedHttpClient.Verify(
                hc => hc.PostAsync(It.Is<Uri>(uri => uri.AbsolutePath.Contains(fileName) && uri.AbsolutePath.Contains(surveyId)),
                        It.IsAny<HttpContent>()), Times.Once());
        }

        [Fact]
        public void TestUploadInterviewerInstructionsAsync_ValidByteArray_FileUpload()
        {
            const string surveyId = "SurveyId";

            const string fileName = "instructions.pdf";

            var fileContent = Encoding.Unicode.GetBytes("Interviewer Instructions");

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient.Setup(client => client.PostAsync(It.IsAny<Uri>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.UploadInterviewerFileInstructionsAsync(fileContent, fileName, surveyId);

            mockedHttpClient.Verify(
                hc => hc.PostAsync(It.Is<Uri>(uri => uri.AbsolutePath.Contains(fileName) && uri.AbsolutePath.Contains(surveyId)),
                    It.IsAny<HttpContent>()), Times.Once());
        }

        #endregion

        #region QuotaQueryAsync

        [Fact]
        public void TestQuotaQueryAsync_ServerReturnsQuery_ReturnsListWithQuotaLevel()
        {
            const string levelId = "LevelId";
            const string name = "Name";

            var expectedQuotaLevel = new QuotaLevel
            {
                Id = levelId,
                Name = name
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/1/Quota")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedQuotaLevel))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualQuotaLevel = target.QuotaQueryAsync("1").Result;
            mockedHttpClient.Verify(hc => hc.GetAsync(It.IsAny<Uri>()), Times.Once());
            Assert.Equal(expectedQuotaLevel.Id, actualQuotaLevel.Id);
            Assert.Equal(expectedQuotaLevel.Name, actualQuotaLevel.Name);

        }

        #endregion

        #region OnlineQuotaQueryAsync

        [Fact]
        public void TestOnlineQuotaQueryAsync_ServerReturnsQuery_ReturnsAppropriateQuota()
        {
            var quotaFrame = new QuotaFrame
            {
                Target = 10
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/1/Quota")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(quotaFrame))));
            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualQuotaFrame = target.OnlineQuotaQueryAsync("1").Result;
            mockedHttpClient.Verify(hc => hc.GetAsync(It.IsAny<Uri>()), Times.Once());

            Assert.Equal(quotaFrame.Target, actualQuotaFrame.Target);


        }

        #endregion

        #region CreateOrUpdateQuotaAsync

        [Fact]
        public void TestCreateOrUpdateQuotaAsync_Normal_CallsCorrectRoute()
        {
            const string surveyId = "surveyId";

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(It.IsAny<Uri>(), It.IsAny<QuotaLevel>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent("")));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.CreateOrUpdateQuotaAsync(surveyId, new QuotaLevel()).Wait();

            mockedHttpClient.Verify(
                hc => hc.PutAsJsonAsync(new Uri(ServiceAddress, "Surveys/" + surveyId + "/Quota"), It.IsAny<QuotaLevel>()),
                Times.Once());
        }

        [Fact]
        public void TestCreateOrUpdateQuotaAsync_Normal_CorrectQuotaFrame()
        {
            const string surveyId = "surveyId";
            var quota = new QuotaLevel(true)
            {
                Target = 10,
                GrossTarget = 15,
                Attributes =
                    new Collection<QuotaAttribute>
                    {
                        new QuotaAttribute {Name = "Attribute", IsSelectionOptional = true, OdinVariable = "var"}
                    }
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(It.IsAny<Uri>(), It.IsAny<QuotaLevel>()))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent("")));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.CreateOrUpdateQuotaAsync(surveyId, quota).Wait();

            mockedHttpClient.Verify(hc => hc.PutAsJsonAsync(It.IsAny<Uri>(), quota), Times.Once());
        }

        [Fact]
        public void TestCreateOrUpdateOnlineQuotaAsync_ReturnsCorrectQuotaFrame()
        {
            const string surveyId = "surveyId";
            var uri = new Uri(ServiceAddress, "Surveys/" + surveyId + "/Quota");

            var quotaFrame = new QuotaFrame
            {
                Target = 10
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync(uri, quotaFrame))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(quotaFrame))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualQuotaFrame = target.CreateOrUpdateOnlineQuotaAsync(surveyId, quotaFrame).Result;

            Assert.Equal(10, actualQuotaFrame.Target);

        }

        #endregion

        #region Survey Counts

        [Fact]
        public void TestCountsQueryAsync_ServerReturnsQuery_ReturnsCounts()
        {
            const int expectedDroppedOutCount = 1;
            const int expectedRejectedCount = 2;
            const int expectedScreenedOutCount = 3;
            const int expectedSuccessfulCount = 4;
            var surveyId = Guid.NewGuid().ToString();
            var expectedCounts = new SurveyCounts
            {
                DroppedOutCount = expectedDroppedOutCount,
                RejectedCount = expectedRejectedCount,
                ScreenedOutCount = expectedScreenedOutCount,
                SuccessfulCount = expectedSuccessfulCount
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var url = new Uri(ServiceAddress, $"Surveys/{surveyId}/Counts");
            mockedHttpClient
                .Setup(client => client.GetAsync(url))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedCounts))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var result = target.CountsQueryAsync(surveyId).Result;

            Assert.Equal(expectedDroppedOutCount, result.DroppedOutCount);
            Assert.Equal(expectedRejectedCount, result.RejectedCount);
            Assert.Equal(expectedScreenedOutCount, result.ScreenedOutCount);
            Assert.Equal(expectedSuccessfulCount, result.SuccessfulCount);
        }

        #endregion

        #region SamplingPointQueryAsync

        [Fact]
        public void TestSamplingPointQueryAsync_ServerReturnsQuery_ReturnsListWithSamplingPoint()
        {
            var expectedSamplingPoint = new SamplingPoint[]
            { new SamplingPoint { SamplingPointId = "SamplingPointId" },
              new SamplingPoint { SamplingPointId = "AnotherSamplingPointId" }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/1/SamplingPoints")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSamplingPoint))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSamplingPoint = target.SamplingPointsQueryAsync("1").Result;

            Assert.Equal(expectedSamplingPoint[0].SamplingPointId, actualSamplingPoint.ToArray()[0].SamplingPointId);
            Assert.Equal(expectedSamplingPoint[1].SamplingPointId, actualSamplingPoint.ToArray()[1].SamplingPointId);
            Assert.Equal(2, actualSamplingPoint.Count());
        }

        #endregion

        #region SamplingPointsCountAsync

        [Fact]
        public void TestSamplingPointsCountAsync_ServerReturnsCount_ReturnsNumberOfSamplingPointsForTheSurvey()
        {
            const string surveyId = "123";
            const int samplingPointCount = 5;
            var uri = new Uri(ServiceAddress, $@"Surveys/{surveyId}/SamplingPoints/Count");
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(uri))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(samplingPointCount.ToString())));
            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var result = target.SamplingPointsCountAsync(surveyId).Result;

            Assert.Equal(samplingPointCount, result);
        }

        #endregion

        #region SamplingPointAddAsync

        [Fact]
        public void TestSamplingPointAddAsync_ServerAcceptsSamplingPoint_ReturnsSamplingPoint()
        {
            const string samplingPointGroupId = "MyGroupId";
            var office = new FieldworkOffice { OfficeId = "OfficeId" };
            var survey = new Survey(SurveyType.Basic) { SurveyId = "SurveyId" };
            var samplingPoint = new SamplingPoint
            {
                SamplingPointId = "SamplingPointId",
                FieldworkOfficeId = office.OfficeId,
                GroupId = samplingPointGroupId
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(samplingPoint));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync(
                            new Uri(ServiceAddress, $"Surveys/{survey.SurveyId}/SamplingPoints"), samplingPoint))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.SamplingPointAddAsync(survey.SurveyId, samplingPoint).Result;

            Assert.Equal(samplingPoint.SamplingPointId, actual.SamplingPointId);
            Assert.Equal(samplingPoint.FieldworkOfficeId, actual.FieldworkOfficeId);
            Assert.Equal(samplingPoint.GroupId, actual.GroupId);
        }

        #endregion

        #region SamplingPointUpdateAsync

        [Fact]
        public void TestSamplingPointUpdateAsync_SamplingPointArgumentIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveysService();
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    try
                    {
                        target.SamplingPointUpdateAsync("", null).Wait();
                    }
                    catch (AggregateException ex)
                    {
                        throw ex.InnerException;
                    }
                }
                );
        }

        [Fact]
        public void TestSamplingPointUpdateAsync_SamplingPointExists_ReturnsSamplingPoint()
        {
            const string surveyId = "SurveyId";
            const string samplingPointId = "SamplingPointId";
            const string samplingPointGroupId = "MyGroupId";

            var samplingPoint = new SamplingPoint
            {
                SamplingPointId = samplingPointId,
                Name = "Updated",
                GroupId = samplingPointGroupId
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(
                            new Uri(ServiceAddress, $"Surveys/{surveyId}/SamplingPoints/{samplingPointId}"),
                            It.IsAny<UpdateSamplingPoint>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(samplingPoint))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.SamplingPointUpdateAsync(surveyId, samplingPoint).Result;

            Assert.Equal(samplingPoint.Name, actual.Name);
            Assert.Equal(samplingPoint.GroupId, actual.GroupId);
        }

        #endregion

        #region SamplingPointRemoveAsync

        [Fact]
        public void TestSamplingPointRemoveAsync_SamplingPointIdIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveysService();
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    try
                    {
                        target.SamplingPointDeleteAsync("", null).Wait();
                    }
                    catch (AggregateException ex)
                    {
                        throw ex.InnerException;
                    }
                }
                );
        }

        [Fact]
        public void TestSamplingPointRemoveAsync_ServerRemovedSamplingPoint_DoesNotThrow()
        {
            const string samplingPointId = "SamplingPointId";
            const string surveyId = "SurveyId";
            var samplingPoint = new SamplingPoint
            {
                SamplingPointId = samplingPointId
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.DeleteAsync(new Uri(ServiceAddress, $"Surveys/{surveyId}/SamplingPoints/{samplingPointId}")))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.DoesNotThrow(() => target.SamplingPointDeleteAsync(surveyId, samplingPoint).Wait());
        }

        #endregion

        #region SamplingPointQuotaTargetsQueryAsync

        [Fact]
        public void TestSamplingPointQuotaTargetsQueryAsync_ServerReturnsQuery_ReturnsListWithSamplingPointQuotaTargets()
        {
            var expectedSamplingPointQuotaTarget = new SamplingPointQuotaTarget[]
            { new SamplingPointQuotaTarget { LevelId = "TestLevel" },
              new SamplingPointQuotaTarget { LevelId = "AnotherTestLevel" }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Surveys/1/SamplingPoints/1/QuotaTargets")))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(expectedSamplingPointQuotaTarget))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSamplingPointQuotaTarget = target.SamplingPointQuotaTargetsQueryAsync("1", "1").Result;

            Assert.Equal(expectedSamplingPointQuotaTarget[0].LevelId, actualSamplingPointQuotaTarget.ToArray()[0].LevelId);
            Assert.Equal(expectedSamplingPointQuotaTarget[1].LevelId, actualSamplingPointQuotaTarget.ToArray()[1].LevelId);
            Assert.Equal(2, actualSamplingPointQuotaTarget.Count());
        }

        #endregion

        #region SamplingPointQuotaTargetUpdateAsync

        [Fact]
        public void TestSamplingPointQuotaTargetUpdateAsync_SurveyIdArgumentIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveysService();
            Assert.Throws<ArgumentNullException>(() =>
            {
                target.SamplingPointQuotaTargetUpdateAsync(null, It.IsAny<string>(), It.IsAny<SamplingPointQuotaTarget>()).Wait();
            });
        }

        [Fact]
        public void TestSamplingPointQuotaTargetUpdateAsync_SamplingPointIdArgumentIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveysService();
            Assert.Throws<ArgumentNullException>(() =>
            {
                target.SamplingPointQuotaTargetUpdateAsync(It.IsAny<string>(), null, It.IsAny<SamplingPointQuotaTarget>()).Wait();
            });
        }

        [Fact]
        public void TestSamplingPointQuotaTargetUpdateAsync_SamplingPointQuotaTargetArgumentIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveysService();
            Assert.Throws<ArgumentNullException>(() =>
            {
                target.SamplingPointQuotaTargetUpdateAsync(It.IsAny<string>(), It.IsAny<string>(), null).Wait();
            });
        }

        [Fact]
        public void TestSamplingPointQuotaTargetUpdateAsync_SamplingPointQuotaTargetLevelIdIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveysService();
            var samplingPointQuotaTarget = new SamplingPointQuotaTarget { Target = 4 };

            Assert.Throws<ArgumentNullException>(() =>
            {
                target.SamplingPointQuotaTargetUpdateAsync(It.IsAny<string>(), It.IsAny<string>(), samplingPointQuotaTarget).Wait();
            });
        }

        [Fact]
        public void TestSamplingPointQuotaTargetUpdateAsync_SamplingPointQuotaTargetExists_ReturnsSamplingPointQuotaTarget()
        {
            const string levelId = "LevelId";
            const string surveyId = "SurveyId";
            const string samplingPointId = "SamplingPointId";

            var samplingPointQuotaTarget = new SamplingPointQuotaTarget
            {
                LevelId = levelId,
                Target = 10
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            mockedHttpClient
                .Setup(
                    client => client.PatchAsJsonAsync(
                            new Uri(ServiceAddress, $"Surveys/{surveyId}/SamplingPoints/{samplingPointId}/QuotaTargets/{levelId}"),
                            It.IsAny<UpdateSamplingPointQuotaTarget>()))
                .Returns(CreateTask(HttpStatusCode.OK,
                    new StringContent(JsonConvert.SerializeObject(samplingPointQuotaTarget))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.SamplingPointQuotaTargetUpdateAsync(surveyId, samplingPointId, samplingPointQuotaTarget).Result;

            Assert.Equal(samplingPointQuotaTarget.Target, actual.Target);
        }

        #endregion

        #region SamplingPointImageAddAsync

        [Fact]
        public void TestSamplingPointImageAddAsync_ServerAcceptsSamplingPointImage_ReturnsFilename()
        {
            const string surveyId = "SurveyId";
            const string samplingPointId = "SamplingPointId";
            const string fileName = "1.jpg";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", fileName);

            var content = new ByteArrayContent(File.ReadAllBytes(filePath));

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.PostAsync(It.IsAny<Uri>(), It.IsAny<ByteArrayContent>()))
                                    .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(fileName))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var result = target.SamplingPointImageAddAsync(surveyId, samplingPointId, filePath).Result;

            Assert.Equal(fileName, result);
        }

        [Fact]
        public void TestSamplingPointImageGetAsync_ServerAcceptsGet_ReturnsFilename()
        {
            const string surveyId = "SurveyId";
            const string samplingPointId = "SamplingPointId";
            const string fileName = "1.jpg";


            var getContent = new ByteArrayContent(new byte[] { 1 });
            getContent.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.GetAsync(It.IsAny<Uri>()))
                                    .Returns(CreateTask(HttpStatusCode.OK, getContent));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var result = target.SamplingPointImageGetAsync(surveyId, samplingPointId).Result;

            Assert.Equal(fileName, result.FileName);
        }

        [Fact]
        public void TestSamplingPointImageDeleteAsync_ServerAcceptsDelete_ReturnsNoError()
        {
            const string surveyId = "SurveyId";
            const string samplingPointId = "SamplingPointId";

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient.Setup(client => client.DeleteAsync(It.IsAny<Uri>()))
                                    .Returns(CreateTask(HttpStatusCode.NoContent));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            target.SamplingPointImageDeleteAsync(surveyId, samplingPointId).Wait();
        }

        #endregion
    }
}

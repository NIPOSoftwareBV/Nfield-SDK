using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using Nfield.Infrastructure;
using Nfield.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveysService"/>
    /// </summary>
    public class NfieldSurveysServiceTests
    {
        private const string ServiceAddress = @"http://localhost/nfieldapi";

        #region QueryAsync

        [Fact]
        public void TestQueryAsync_ServerReturnsQuery_ReturnsListWithSurveys()
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.NotFound;
            var expectedSurveys = new Survey[]
            { new Survey { SurveyId = "TestSurvey" },
              new Survey { SurveyId = "AnotherTestSurvey" }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(httpStatusCode);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + @"/surveys"))
                .Returns(CreateTask(httpStatusCode, new StringContent(JsonConvert.SerializeObject(expectedSurveys))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSurveys = target.QueryAsync().Result;

            Assert.Equal(expectedSurveys[0].SurveyId, actualSurveys.ToArray()[0].SurveyId);
            Assert.Equal(expectedSurveys[1].SurveyId, actualSurveys.ToArray()[1].SurveyId);
            Assert.Equal(2, actualSurveys.Count());
        }

        #endregion

        #region SamplingPointQueryAsync

        [Fact]
        public void TestSamplingPointQueryAsync_ServerReturnsQuery_ReturnsListWithSamplingPoint()
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.NotFound;
            var expectedSamplingPoint = new SamplingPoint[]
            { new SamplingPoint { SamplingPointId = "SamplingPointId" },
              new SamplingPoint { SamplingPointId = "AnotherSamplingPointId" }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(httpStatusCode);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + @"/surveys/1/samplingpoints"))
                .Returns(CreateTask(httpStatusCode, new StringContent(JsonConvert.SerializeObject(expectedSamplingPoint))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSamplingPoint = target.SamplingPointsQueryAsync("1").Result;

            Assert.Equal(expectedSamplingPoint[0].SamplingPointId, actualSamplingPoint.ToArray()[0].SamplingPointId);
            Assert.Equal(expectedSamplingPoint[1].SamplingPointId, actualSamplingPoint.ToArray()[1].SamplingPointId);
            Assert.Equal(2, actualSamplingPoint.Count());
        }

        #endregion

        #region SamplingPointAddAsync

        [Fact]
        public void TestSamplingPointAddAsync_ServerAcceptsSamplingPoint_ReturnsSamplingPoint()
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;
            var office = new FieldworkOffice { OfficeId = "OfficeId" };
            var survey = new Survey { SurveyId = "SurveyId" };
            var samplingPoint = new SamplingPoint
            {
                SamplingPointId = "SamplingPointId",
                FieldworkOfficeId = office.OfficeId
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(httpStatusCode);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));
            var content = new StringContent(JsonConvert.SerializeObject(samplingPoint));
            mockedHttpClient
                .Setup(
                    client =>
                        client.PostAsJsonAsync(
                            string.Format("{0}/surveys/{1}/samplingpoints", ServiceAddress, survey.SurveyId),
                            samplingPoint))
                .Returns(CreateTask(httpStatusCode, content));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.SamplingPointAddAsync(survey.SurveyId, samplingPoint).Result;

            Assert.Equal(samplingPoint.SamplingPointId, actual.SamplingPointId);
            Assert.Equal(samplingPoint.FieldworkOfficeId, actual.FieldworkOfficeId);
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
            const HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;
            const string surveyId = "SurveyId";
            const string samplingPointId = "SamplingPointId";

            var samplingPoint = new SamplingPoint
            {
                SamplingPointId = samplingPointId,
                Name = "Updated"
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(HttpStatusCode.BadRequest);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));
            mockedHttpClient
                .Setup(
                    client =>
                        client.PatchAsJsonAsync<UpdateSamplingPoint>(
                            string.Format("{0}/surveys/{1}/samplingpoints/{2}", ServiceAddress, surveyId,
                                samplingPointId), It.IsAny<UpdateSamplingPoint>()))
                .Returns(CreateTask(httpStatusCode,
                    new StringContent(JsonConvert.SerializeObject(samplingPoint))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.SamplingPointUpdateAsync(surveyId, samplingPoint).Result;

            Assert.Equal(samplingPoint.Name, actual.Name);
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
            var mockedHttpClient = CreateHttpClientMock(HttpStatusCode.BadRequest);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));
            mockedHttpClient
                .Setup(client => client.DeleteAsync(string.Format("{0}/surveys/{1}/samplingpoints/{2}", ServiceAddress, surveyId,
                                samplingPointId)))
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
            const HttpStatusCode httpStatusCode = HttpStatusCode.NotFound;
            var expectedSamplingPointQuotaTarget = new SamplingPointQuotaTarget[]
            { new SamplingPointQuotaTarget { LevelId = "TestLevel" },
              new SamplingPointQuotaTarget { LevelId = "AnotherTestLevel" }
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(httpStatusCode);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + @"/surveys/1/samplingpoints/1/quotatargets"))
                .Returns(CreateTask(httpStatusCode, new StringContent(JsonConvert.SerializeObject(expectedSamplingPointQuotaTarget))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualSamplingPointQuotaTarget = target.SamplingPointQuotaTargetsQueryAsync("1","1").Result;

            Assert.Equal(expectedSamplingPointQuotaTarget[0].LevelId, actualSamplingPointQuotaTarget.ToArray()[0].LevelId);
            Assert.Equal(expectedSamplingPointQuotaTarget[1].LevelId, actualSamplingPointQuotaTarget.ToArray()[1].LevelId);
            Assert.Equal(2, actualSamplingPointQuotaTarget.Count());
        }

        #endregion

        #region SamplingPointQuotaTargetUpdateAsync

        [Fact]
        public void TestSamplingPointQuotaTargetUpdateAsync_SamplingPointQuotaTargetArgumentIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldSurveysService();
            Assert.Throws<ArgumentNullException>(
                () =>
                {
                    try
                    {
                        target.SamplingPointQuotaTargetUpdateAsync("", "", null).Wait();
                    }
                    catch (AggregateException ex)
                    {
                        throw ex.InnerException;
                    }
                }
                );
        }

        [Fact]
        public void TestSamplingPointQuotaTargetUpdateAsync_SamplingPointQuotaTargetExists_ReturnsSamplingPointQuotaTarget()
        {
            const HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;
            const string levelId = "LevelId";
            const string surveyId = "SurveyId";
            const string samplingPointId = "SamplingPointId";

            var samplingPointQuotaTarget = new SamplingPointQuotaTarget
            {
                LevelId = levelId,
                Target = 10
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(HttpStatusCode.BadRequest);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));
            mockedHttpClient
                .Setup(
                    client =>
                        client.PatchAsJsonAsync<UpdateSamplingPointQuotaTarget>(
                            string.Format("{0}/surveys/{1}/samplingpoints/{2}/quotatargets/{3}", ServiceAddress, surveyId,
                                samplingPointId, levelId), It.IsAny<UpdateSamplingPointQuotaTarget>()))
                .Returns(CreateTask(httpStatusCode,
                    new StringContent(JsonConvert.SerializeObject(samplingPointQuotaTarget))));

            var target = new NfieldSurveysService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.SamplingPointQuotaTargetUpdateAsync(surveyId,samplingPointId,samplingPointQuotaTarget).Result;

            Assert.Equal(samplingPointQuotaTarget.Target, actual.Target);
        }

        #endregion

        private Task<HttpResponseMessage> CreateTask(HttpStatusCode httpStatusCode, HttpContent content = null)
        {
            return Task.Factory.StartNew(() => new HttpResponseMessage(httpStatusCode) { Content = content });
        }

        private Mock<INfieldHttpClient> CreateHttpClientMock(HttpStatusCode httpStatusCode)
        {
            var mockedHttpClient = new Mock<INfieldHttpClient>();

            //setup the mocked HttpClient to return httpStatusCode for all methods that send a request to the server

            mockedHttpClient
                .Setup(client => client.GetAsync(It.IsAny<string>()))
                .Returns(CreateTask(httpStatusCode));

            return mockedHttpClient;
        }

    }
}

using System;
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
    public class NfieldSurveySampleDataServiceTests : NfieldServiceTestsBase
    {
        const string SurveyId = "MySurvey";
        const string FileName = "MyFile";

        [Fact]
        public void TestGetAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveySampleDataService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.GetAsync(null, FileName)));
        }

        [Fact]
        public void TestGetAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleDataService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.GetAsync("", FileName)));
        }

        [Fact]
        public void TestGetAsync_FileNameIsNull_Throws()
        {
            var target = new NfieldSurveySampleDataService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.GetAsync(SurveyId, null)));
        }

        [Fact]
        public void TestGetAsync_FileNameIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleDataService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.GetAsync(SurveyId, "")));
        }

        [Fact]
        public void TestAddOrUpdateAsync_ServerAcceptsSetting_ReturnsSetting()
        {
            var task = new BackgroundTask { Id = "TaskId" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(task));
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "Surveys/" + SurveyId + "/SampleData/" + FileName))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldSurveySampleDataService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.GetAsync(SurveyId, FileName).Result;

            Assert.Equal(task.Id, actual.Id);
        }

    }
}

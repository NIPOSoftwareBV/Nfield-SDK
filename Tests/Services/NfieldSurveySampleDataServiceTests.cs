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
using Nfield.Services.Implementation;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Nfield.Services
{
    public class NfieldSurveySampleDataServiceTests : NfieldServiceTestsBase
    {
        const string SurveyId = "MySurvey";
        const string FileName = "MyFile";

        [Fact]
        public void TestPrepareDownloadSampleDataAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveySampleDataService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.PrepareDownloadSampleDataAsync(null, FileName)));
        }

        [Fact]
        public void TestPrepareDownloadSampleDataAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleDataService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.PrepareDownloadSampleDataAsync("", FileName)));
        }

        [Fact]
        public void TestPrepareDownloadSampleDataAsync_FileNameIsNull_Throws()
        {
            var target = new NfieldSurveySampleDataService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.PrepareDownloadSampleDataAsync(SurveyId, null)));
        }

        [Fact]
        public void TestPrepareDownloadSampleDataAsync_FileNameIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleDataService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.PrepareDownloadSampleDataAsync(SurveyId, "")));
        }

        [Fact]
        public async Task TestPrepareDownloadSampleDataAsync_ServerAcceptsSetting_ReturnsSetting()
        {
            var expectedDownloadUrl = "DownloadLink";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(JsonConvert.SerializeObject(new { ActivityId = "activityId" }));
            mockedHttpClient
                .Setup(client => client.PostAsync(new Uri(ServiceAddress, "Surveys/" + SurveyId + "/SampleDataDownload/" + FileName), null))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            mockedHttpClient
                .Setup(client => client.GetAsync(It.IsAny<Uri>()))
                .Returns(Task.Factory.StartNew(
                    () =>
                    new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new { DownloadDataUrl = expectedDownloadUrl, ActivityId = "activityId", Status = 2 /* Succeeded */ }))
                    })).Verifiable();

            var target = new NfieldSurveySampleDataService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var result = await target.PrepareDownloadSampleDataAsync(SurveyId, FileName);

            Assert.Equal(expectedDownloadUrl, result);
        }
    }
}

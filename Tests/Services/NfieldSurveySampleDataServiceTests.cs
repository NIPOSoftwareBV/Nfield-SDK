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

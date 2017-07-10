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
    /// <summary>
    /// Tests for <see cref="NfieldSurveySampleService"/>
    /// </summary>
    public class NfieldSurveySampleTests: NfieldServiceTestsBase
    {
        private const string SurveyId = "MySurvey";

        #region GetAsync

        [Fact]
        public void TestGetAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.GetAsync(null)));
        }

        [Fact]
        public void TestGetAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.GetAsync("  ")));
        }

        [Fact]
        public void TestGetAsync_SurveyHasSample_ReturnsSample()
        {
            const string sample = "a sample";
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);
            var content = new StringContent(sample);
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + "Surveys/" + SurveyId + "/Sample"))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var target = new NfieldSurveySampleService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.GetAsync(SurveyId).Result;

            Assert.Equal(sample, actual);
        }

        #endregion

        #region PostAsync

        [Fact]
        public void TestPostAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.PostAsync(null, null)));
        }

        [Fact]
        public void TestPostAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.PostAsync("  ", null)));
        }

        [Fact]
        public void TestPostAsync_SampleIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.PostAsync(SurveyId, null)));
        }

        [Fact]
        public void TestPostAsync_SampleIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.PostAsync(SurveyId, "  ")));
        }

        [Fact]
        public void TestPostAsync_ServerAccepts_ReturnsStatusMessage()
        {
            const string sample = "a sample";
            var uploadStatus = new SampleUploadStatus
            {
                DuplicateKeyCount = 1,
                EmptyKeyCount = 2,
                HeaderDataMismatch = true,
                HeaderInvalid = true,
                HeaderInvalidColumnsCount = 3,
                InsertedCount = 4,
                InvalidDataCount = 5,
                InvalidKeyCount = 6,
                ProcessingStatus = "good status",
                SkippedCount = 7,
                TotalRecordCount = 8,
                UpdatedCount = 9
            };

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient
                .Setup(client => client.PostAsync($"{ServiceAddress}Surveys/{SurveyId}/Sample", It.Is<StringContent>(stringContent => stringContent.ReadAsStringAsync().Result.Equals(sample))))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(uploadStatus))));

            var target = new NfieldSurveySampleService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            var actual = target.PostAsync(SurveyId, sample).Result;

            Assert.Equal(uploadStatus.DuplicateKeyCount, actual.DuplicateKeyCount);
            Assert.Equal(uploadStatus.EmptyKeyCount, actual.EmptyKeyCount);
            Assert.Equal(uploadStatus.HeaderDataMismatch, actual.HeaderDataMismatch);
            Assert.Equal(uploadStatus.HeaderInvalid, actual.HeaderInvalid);
            Assert.Equal(uploadStatus.HeaderInvalidColumnsCount, actual.HeaderInvalidColumnsCount);
            Assert.Equal(uploadStatus.InsertedCount, actual.InsertedCount);
            Assert.Equal(uploadStatus.InvalidDataCount, actual.InvalidDataCount);
            Assert.Equal(uploadStatus.InvalidKeyCount, actual.InvalidKeyCount);
            Assert.Equal(uploadStatus.ProcessingStatus, actual.ProcessingStatus);
            Assert.Equal(uploadStatus.SkippedCount, actual.SkippedCount);
            Assert.Equal(uploadStatus.TotalRecordCount, actual.TotalRecordCount);
            Assert.Equal(uploadStatus.UpdatedCount, actual.UpdatedCount);
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public void TestDeleteAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.PostAsync(null, null)));
        }

        [Fact]
        public void TestDeleteAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.PostAsync("  ", null)));
        }

        [Fact]
        public void TestDeleteAsync_SampleRecordIdIsNull_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.PostAsync(SurveyId, null)));
        }

        [Fact]
        public void TestDeleteAsync_SampleRecordIdIsEmpty_Throws()
        {
            var target = new NfieldSurveySampleService();

            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.PostAsync(SurveyId, "  ")));
        }

        [Fact]
        public void TestDeleteAsync_SampleExists_ReturnsStatusMessage()
        {
            const string respondentKey = "a sample record id";
            const string message = "a message";

            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            mockedHttpClient
                .Setup(client => client.DeleteAsJsonAsync($"{ServiceAddress}Surveys/{SurveyId}/Sample",
                    It.Is<IEnumerable<SampleFilter>>(
                        filters => filters.Single().Name.Equals("RespondentKey")
                                   && filters.Single().Op.Equals("eq")
                                   && filters.Single().Value.Equals(respondentKey))))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(message)));

            var target = new NfieldSurveySampleService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);
            var actual = target.DeleteAsync(SurveyId, respondentKey).Result;

            Assert.Equal(message, actual);
        }

        #endregion
    }
}

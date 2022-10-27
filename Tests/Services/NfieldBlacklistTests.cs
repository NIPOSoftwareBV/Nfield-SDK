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
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldBlacklistService"/>
    /// </summary>
    public class NfieldBlacklistTests : NfieldServiceTestsBase
    {
        private readonly NfieldBlacklistService _target;
        readonly Mock<INfieldHttpClient> _mockedHttpClient;

        public NfieldBlacklistTests()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            _mockedHttpClient = CreateHttpClientMock(mockedNfieldConnection);

            _target = new NfieldBlacklistService();
            _target.InitializeNfieldConnection(mockedNfieldConnection.Object);
        }

        #region GetAsync

        [Fact]
        public void TestGetAsync_SurveyHasSample_ReturnsSample()
        {
            const string sample = "a sample";
            var content = new StringContent(sample);
            _mockedHttpClient
                .Setup(client => client.GetAsync(new Uri(ServiceAddress, "Blacklist/")))
                .Returns(CreateTask(HttpStatusCode.OK, content));

            var actual = _target.GetAsync().Result;

            Assert.Equal(sample, actual);
        }

        #endregion
        #region PostAsync

        [Fact]
        public void TestPostAsync_BlacklistIsNull_Throws()
        {
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(_target.PostAsync(null)));
        }

        [Fact]
        public void TestPostAsync_ServerAccepts_ReturnsStatusMessage()
        {
            const string blacklist = "the blacklist";
            var uploadStatus = new BlacklistUploadStatus
            {
                DuplicateKeyCount = 0,
                EmptyKeyCount = 0,
                HeaderDataMismatch = false,
                HeaderInvalid = false,
                HeaderInvalidColumnsCount = 0,
                InsertedCount = 3,
                InvalidDataCount = 0,
                InvalidKeyCount = 0,
                ProcessingStatus = "uploaded successfully",
                SkippedCount = 0,
                TotalRecordCount = 3,
                UpdatedCount = 0
            };

            _mockedHttpClient
                .Setup(client => client.PostAsync(new Uri(ServiceAddress, $"Blacklist/"), It.Is<StringContent>(stringContent => stringContent.ReadAsStringAsync().Result.Equals(blacklist))))
                .Returns(CreateTask(HttpStatusCode.OK, new StringContent(JsonConvert.SerializeObject(uploadStatus))));

            var actual = _target.PostAsync(blacklist).Result;

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
    }
}

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
using Nfield.Models;
using Nfield.Services.Implementation;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldSurveyEmailSettingsService"/>
    /// </summary>
    public class NfieldSurveyEmailSettingsServiceTests : NfieldServiceTestsBase
    {
        private const string SurveyId = "TestSurveyId";

        #region GetAsync

        [Fact]
        public void TestGetAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyEmailSettingsService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.GetAsync(null)));
        }

        [Fact]
        public void TestGetAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveyEmailSettingsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.GetAsync(string.Empty)));
        }

        [Fact]
        public void TestGetAsync_SurveyIdIsWhitespace_Throws()
        {
            var target = new NfieldSurveyEmailSettingsService();
            Assert.Throws<ArgumentException>(() => UnwrapAggregateException(target.GetAsync("   ")));
        }

        [Fact]
        public void TestGetAsync_SurveyExists_ReturnsData()
        {
            var expected = GetTestEmailSettings();
            var expectedDto = new SurveyEmailSettingsResponse
            {
                SurveyEmailSettings = expected
            };

            var target = new NfieldSurveyEmailSettingsService();
            var mockClient = InitMockClientGet(GetEmailSettingsUrl(SurveyId), expectedDto);
            target.InitializeNfieldConnection(mockClient);

            var actual = target.GetAsync(SurveyId).Result;
            AssertOnEmailSettings(expected, actual.SurveyEmailSettings);
        }

        #endregion

        #region Put

        [Fact]
        public void TestPutAsync_SurveyIdIsNull_Throws()
        {
            var target = new NfieldSurveyEmailSettingsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.PutAsync(null, new SurveyEmailSettings())));
        }

        [Fact]
        public void TestPutAsync_SurveyIdIsEmpty_Throws()
        {
            var target = new NfieldSurveyEmailSettingsService();
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.PutAsync(string.Empty, new SurveyEmailSettings())));
        }

        [Fact]
        public void TestPutAsync_SurveyIdIsWhitespace_Throws()
        {
            var target = new NfieldSurveyEmailSettingsService();
            Assert.Throws<ArgumentException>(() =>
                UnwrapAggregateException(target.PutAsync("   ", new SurveyEmailSettings())));
        }

        [Fact]
        public void TestPutAsync_SettingsNull_Throws()
        {
            var target = new NfieldSurveyEmailSettingsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.PutAsync("anything", null)));
        }

        [Fact]
        public void TestPutAsync_SurveyExists_ReturnsData()
        {
            var expected = GetTestEmailSettings();

            var target = new NfieldSurveyEmailSettingsService();
            var mockClient = InitMockClientPut(GetEmailSettingsUrl(SurveyId), expected, expected);
            target.InitializeNfieldConnection(mockClient);

            var actual = target.PutAsync(SurveyId, expected).Result;
            AssertOnEmailSettings(expected, actual);
        }

        #endregion

        private Uri GetEmailSettingsUrl(string surveyId)
        {
            return new Uri(ServiceAddress, "Surveys/" + SurveyId + "/EmailSettings");
        }

        private static SurveyEmailSettings GetTestEmailSettings()
        {
            return new SurveyEmailSettings
            {
                FromAddress = "TestFromAddress",
                FromName = "TestFromName",
                Id = "TestId",
                PostalAddress = "TestPostalAddress",
                ReplyToAddress = "TestReplyToAddress",
                EmailColumn = "TestEmailColumn"
            };
        }

        private static void AssertOnEmailSettings(SurveyEmailSettings expected, SurveyEmailSettings actual)
        {
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.FromAddress, actual.FromAddress);
            Assert.Equal(expected.FromName, actual.FromName);
            Assert.Equal(expected.PostalAddress, actual.PostalAddress);
            Assert.Equal(expected.ReplyToAddress, actual.ReplyToAddress);
            Assert.Equal(expected.EmailColumn, actual.EmailColumn);
        }
    }
}

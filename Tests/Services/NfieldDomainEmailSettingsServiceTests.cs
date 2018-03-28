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
    /// Tests for <see cref="NfieldDomainEmailSettingsService"/>
    /// </summary>
    public class NfieldDomainEmailSettingsServiceTests : NfieldServiceTestsBase
    {
        #region GetAsync

        [Fact]
        public void TestGetAsync_ReturnsData()
        {
            var expected = new DomainEmailSettings
            {
                FromAddress = "TestFromAddress",
                FromName = "TestFromName",
                PostalAddress = "TestPostalAddress",
                ReplyToAddress = "TestReplyToAddress",
                DefaultFromAddress = "TestDefaultFromAddress",
                DefaultReplyToAddress = "TestDefaultReplyToAddress"
            };

            var target = new NfieldDomainEmailSettingsService();
            var mockClient = InitMockClientGet(GetEmailSettingsUrl(), expected);
            target.InitializeNfieldConnection(mockClient);

            var actual = target.GetAsync().Result;
            AssertOnEmailSettings(expected, actual);
        }

        #endregion

        #region Put

        [Fact]
        public void TestPutAsync_SettingsNull_Throws()
        {
            var target = new NfieldDomainEmailSettingsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.PutAsync(null)));
        }

        [Fact]
        public void TestPutAsync_ReturnsData()
        {
            var expected = new EmailSettingsEditable
            {
                FromAddress = "TestFromAddress",
                FromName = "TestFromName",
                PostalAddress = "TestPostalAddress",
                ReplyToAddress = "TestReplyToAddress"
            };

            var target = new NfieldDomainEmailSettingsService();
            var mockClient = InitMockClientPut(GetEmailSettingsUrl(), expected, expected);
            target.InitializeNfieldConnection(mockClient);

            var actual = target.PutAsync(expected).Result;
            AssertOnEditableEmailSettings(expected, actual);
        }

        #endregion

        private static string GetEmailSettingsUrl()
        {
            return ServiceAddress + "EmailSettings";
        }

        private static void AssertOnEditableEmailSettings(EmailSettingsEditable expected, EmailSettingsEditable actual)
        {
            Assert.Equal(expected.FromAddress, actual.FromAddress);
            Assert.Equal(expected.FromName, actual.FromName);
            Assert.Equal(expected.PostalAddress, actual.PostalAddress);
            Assert.Equal(expected.ReplyToAddress, actual.ReplyToAddress);
        }
        private static void AssertOnEmailSettings(DomainEmailSettings expected, DomainEmailSettings actual)
        {
            AssertOnEditableEmailSettings(expected, actual);
            Assert.Equal(expected.DefaultFromAddress, actual.DefaultFromAddress);
            Assert.Equal(expected.DefaultReplyToAddress, actual.DefaultReplyToAddress);
        }
    }
}

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

using Nfield.Models;
using Nfield.Services.Implementation;
using System;
using Xunit;

namespace Nfield.Services
{
    /// <summary>
    /// Tests for <see cref="NfieldDomainPasswordSettingsService"/>
    /// </summary>
    public class NfieldDomainPasswordSettingsServiceTests : NfieldServiceTestsBase
    {
        #region GetAsync

        [Fact]
        public void TestGetAsync_ReturnsData()
        {
            var expected = new DomainPasswordSettings
            {
                AgeWarnThreshold = 0,
                MaxPasswordAge = 30,
                MinCharsetsInPassword = 4,
                MinPasswordLength = 12,
                PasswordHistoryLength = 0,
                EnforceTwoFactorAuthentication = false
            };

            var target = new NfieldDomainPasswordSettingsService();
            var mockClient = InitMockClientGet(PasswordSettingsUrl(), expected);
            target.InitializeNfieldConnection(mockClient);

            var actual = target.GetAsync().Result;
            AssertOnGetPasswordSettings(expected, actual);
        }

        #endregion

        #region Patch

        [Fact]
        public void TestUpdateAsync_SettingsNull_Throws()
        {
            var target = new NfieldDomainPasswordSettingsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.UpdateAsync(null)));
        }

        [Fact]
        public void TestUpdateAsync_ReturnsData()
        {
            var expected = new DomainPasswordSettings
            {
                AgeWarnThreshold = 2,
                MaxPasswordAge = 365,
                PasswordHistoryLength = 10,
                EnforceTwoFactorAuthentication = true
            };

            var target = new NfieldDomainPasswordSettingsService();
            var mockClient = InitMockClientPatch(PasswordSettingsUrl(), expected, expected);
            target.InitializeNfieldConnection(mockClient);

            var actual = target.UpdateAsync(expected).Result;
            AssertOnUpdatePasswordSettings(expected, actual);
        }

        #endregion

        private Uri PasswordSettingsUrl()
        {
            return new Uri(ServiceAddress, "PasswordSettings/");
        }
        
        private static void AssertOnGetPasswordSettings(DomainPasswordSettings expected, DomainPasswordSettings actual)
        {
            Assert.Equal(expected.MinCharsetsInPassword, actual.MinCharsetsInPassword);
            Assert.Equal(expected.MinPasswordLength, actual.MinPasswordLength);

            AssertOnUpdatePasswordSettings(expected, actual);
        }

        private static void AssertOnUpdatePasswordSettings(DomainPasswordSettings expected, DomainPasswordSettings actual)
        {
            Assert.Equal(expected.AgeWarnThreshold, actual.AgeWarnThreshold);
            Assert.Equal(expected.EnforceTwoFactorAuthentication, actual.EnforceTwoFactorAuthentication);
            Assert.Equal(expected.MaxPasswordAge, actual.MaxPasswordAge);
            Assert.Equal(expected.PasswordHistoryLength, actual.PasswordHistoryLength);
        }
    }
}

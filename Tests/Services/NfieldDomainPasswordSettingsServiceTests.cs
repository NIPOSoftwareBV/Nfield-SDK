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
                AgeWarnThreshold = 1,
                MaxPasswordAge = 1,
                MinCharsetsInPassword = 1,
                MinPasswordLength = 1,
                PasswordHistoryLength = 1,
                EnforceTwoFactorAuthentication = false
            };

            var target = new NfieldDomainPasswordSettingsService();
            var mockClient = InitMockClientGet(PasswordSettingsUrl(), expected);
            target.InitializeNfieldConnection(mockClient);

            var actual = target.GetAsync().Result;
            AssertOnPasswordSettings(expected, actual);
        }

        #endregion

        #region Patch

        [Fact]
        public void TestPatchAsync_SettingsNull_Throws()
        {
            var target = new NfieldDomainPasswordSettingsService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.PatchAsync(null)));
        }

        [Fact]
        public void TestPatchAsync_ReturnsData()
        {
            var expected = new DomainPasswordSettings
            {
                AgeWarnThreshold = 1,
                MaxPasswordAge = 1,
                MinCharsetsInPassword = 1,
                MinPasswordLength = 1,
                PasswordHistoryLength = 1,
                EnforceTwoFactorAuthentication = false
            };

            var target = new NfieldDomainPasswordSettingsService();
            var mockClient = InitMockClientPut(PasswordSettingsUrl(), expected, expected);
            target.InitializeNfieldConnection(mockClient);

            var actual = target.PatchAsync(expected).Result;
            AssertOnPasswordSettings(expected, actual);
        }

        #endregion

        private Uri PasswordSettingsUrl()
        {
            return new Uri(ServiceAddress, "PasswordSettings/");
        }
        
        private static void AssertOnPasswordSettings(DomainPasswordSettings expected, DomainPasswordSettings actual)
        {
            Assert.Equal(expected.AgeWarnThreshold, actual.AgeWarnThreshold);
            Assert.Equal(expected.EnforceTwoFactorAuthentication, actual.EnforceTwoFactorAuthentication);
            Assert.Equal(expected.MaxPasswordAge, actual.MaxPasswordAge);
            Assert.Equal(expected.MinCharsetsInPassword, actual.MinCharsetsInPassword);
            Assert.Equal(expected.MinPasswordLength, actual.MinPasswordLength);
            Assert.Equal(expected.PasswordHistoryLength, actual.PasswordHistoryLength);
        }
    }
}

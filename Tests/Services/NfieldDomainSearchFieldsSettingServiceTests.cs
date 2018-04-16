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
    public class NfieldDomainSearchFieldsSettingServiceTests : NfieldServiceTestsBase
    {

        private readonly DomainSearchFieldsSetting _expected = new DomainSearchFieldsSetting()
        {
            Fields = new[]
            {
                "field1",
                "field3"
            }
        };

        #region Get

        [Fact]
        public void TestGetAsync_ReturnsData()
        {
            var target = new NfieldDomainSearchFieldsSettingService();
            var mockClient = InitMockClientGet(ServiceAddress + @"SearchFieldsSetting", _expected);
            target.InitializeNfieldConnection(mockClient);


            var actual = target.GetAsync().Result;
            Assert.Equal(_expected.Fields,actual.Fields);
        }

        #endregion

        #region Put

        [Fact]
        public void TestPutAsync_SettingsNull_Throws()
        {
            var target = new NfieldDomainSearchFieldsSettingService();
            Assert.Throws<ArgumentNullException>(() =>
                UnwrapAggregateException(target.PutAsync(null)));
        }

        [Fact]
        public void TestPutAsync_ReturnsData()
        {
            var target = new NfieldDomainSearchFieldsSettingService();
            var mockClient = InitMockClientPut(ServiceAddress + @"SearchFieldsSetting", _expected, _expected);
            target.InitializeNfieldConnection(mockClient);

            var actual = target.PutAsync(_expected).Result;
            Assert.Equal(_expected.Fields, actual.Fields);
        }

        #endregion
    }
}

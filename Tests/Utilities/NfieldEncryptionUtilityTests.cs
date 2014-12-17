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
using System.Text;
using Moq;
using Xunit;

namespace Nfield.Utilities
{
    public class NfieldEncryptionUtilityTests
    {
        [Fact]
        public void TestEncryptText_InputIsNull_Throws()
        {
            var target = new NfieldEncryptionUtility(new AesManagedWrapper());
            Assert.Throws<ArgumentNullException>(() => target.EncryptText(null, "key"));
        }

        [Fact]
        public void TestEncryptText_KeyIsNull_Throws()
        {
            var target = new NfieldEncryptionUtility(new AesManagedWrapper());
            Assert.Throws<ArgumentNullException>(() => target.EncryptText("data", null));
        }

        [Fact]
        public void TestEncryptText_EncryptionSuccessful_ReturnsExpectedResult()
        {
            const string input = "ABC";
            var key = new byte[] { 8, 9, 10 };

            var bytesInput = Encoding.UTF8.GetBytes(input);
            var keyBase64 = Convert.ToBase64String(key);

            var encryptedData = new byte[] {1, 2, 3};
            var iv = new byte[] { 8, 9, 10 };

            var mockedAesWrapper = new Mock<IAesManagedWrapper>();
            mockedAesWrapper.Setup(aes => aes.Encrypt(bytesInput, key, out iv))
                .Returns(encryptedData);

            var target = new NfieldEncryptionUtility(mockedAesWrapper.Object);

            var result = target.EncryptText(input, keyBase64);

            Assert.Equal(Convert.ToBase64String(encryptedData), result.Data);
            Assert.Equal(Convert.ToBase64String(iv), result.InitializationVector);
        }

        [Fact]
        public void TestDecryptText_InputIsNull_Throws()
        {
            var target = new NfieldEncryptionUtility(new AesManagedWrapper());
            Assert.Throws<ArgumentNullException>(() => target.DecryptText(null, "key", "iv"));
        }

        [Fact]
        public void TestDecryptText_KeyIsNull_Throws()
        {
            var target = new NfieldEncryptionUtility(new AesManagedWrapper());
            Assert.Throws<ArgumentNullException>(() => target.DecryptText("data", null, "iv"));
        }

        [Fact]
        public void TestDecryptText_IvIsNull_Throws()
        {
            var target = new NfieldEncryptionUtility(new AesManagedWrapper());
            Assert.Throws<ArgumentNullException>(() => target.DecryptText("data", "key", null));
        }

        public void TestDecryptText_DecryptionSuccessful_ReturnsExpectedResult()
        {
            var input = new byte[] { 1, 2, 3 };
            var key = new byte[] { 8, 9, 10 };
            var iv = new byte[] { 4, 5, 6 };

            var inputBase64 = Convert.ToBase64String(input);
            var keyBase64 = Convert.ToBase64String(key);
            var ivBase64 = Convert.ToBase64String(iv);

            var cleanData = new byte[] { 1, 2, 3 };
            
            var mockedAesWrapper = new Mock<IAesManagedWrapper>();
            mockedAesWrapper.Setup(aes => aes.Decrypt(input, key, iv))
                .Returns(cleanData);

            var target = new NfieldEncryptionUtility(mockedAesWrapper.Object);

            var result = target.DecryptText(inputBase64, keyBase64, ivBase64);

            Assert.Equal(Encoding.UTF8.GetString(cleanData), result);
        }
    }
}

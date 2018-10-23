using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Nfield.Models;

namespace Nfield.Utilities
{
    /// <summary>
    /// <see cref="NfieldEncryptionUtility"/>
    /// </summary>
    class NfieldEncryptionUtility : INfieldEncryptionUtility
    {
        private readonly IAesManagedWrapper _aesWrapper;

        public NfieldEncryptionUtility()
            : this(new AesManagedWrapper())
        {
            
        }

        public NfieldEncryptionUtility(IAesManagedWrapper aesWrapper)
        {
            _aesWrapper = aesWrapper;
        }

        #region INfieldEncryptionService

        /// <summary>
        /// <see cref="INfieldEncryptionUtility.EncryptText"/>
        /// </summary>
        public EncryptedDataModel EncryptText(string input, string key)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException("input");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            var bytesInput = Encoding.UTF8.GetBytes(input);
            var bytesKey = Convert.FromBase64String(key);
            byte[] iv;

            var result = _aesWrapper.Encrypt(bytesInput, bytesKey, out iv);

            return new EncryptedDataModel
            {
                Data = Convert.ToBase64String(result),
                InitializationVector = Convert.ToBase64String(iv)
            };
        }

        /// <summary>
        /// <see cref="INfieldEncryptionUtility.DecryptText"/>
        /// </summary>
        public string DecryptText(string input, string key, string initializationVector)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException("input");

            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            if (string.IsNullOrEmpty(initializationVector))
                throw new ArgumentNullException("initializationVector");

            var bytesInput = Convert.FromBase64String(input);
            var bytesKey = Convert.FromBase64String(key);
            var bytesIv = Convert.FromBase64String(initializationVector);

            var result = _aesWrapper.Decrypt(bytesInput, bytesKey, bytesIv);

            return Encoding.UTF8.GetString(result);
        }

        /// <summary>
        /// <see cref="INfieldEncryptionUtility.CreateQueryStringSegment"/>
        /// </summary>
        public string CreateQueryStringSegment(EncryptedDataModel model)
        {
            var queryString = $"data={HttpUtility.UrlEncode(model.Data)}&iv={HttpUtility.UrlEncode(model.InitializationVector)}";

            return queryString;
        }

        #endregion INfieldEncryptionService
    }

    /// <summary>
    /// Facade to the Aes encryption
    /// </summary>
    interface IAesManagedWrapper
    {
        /// <summary>
        /// Encrypts the data using the key and the iv
        /// </summary>
        byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] aesKey, out byte[] aesIv);

        /// <summary>
        /// Decrypts the data using the key and the iv
        /// </summary>
        byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] aesKey, byte[] aesIv);
    }

    /// <summary>
    /// <see cref="IAesManagedWrapper"/>
    /// </summary>
    class AesManagedWrapper : IAesManagedWrapper
    {
        /// <summary>
        /// <see cref="IAesManagedWrapper.Encrypt"/>
        /// </summary>
        public byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] aesKey, out byte[] aesIv)
        {
            byte[] encryptedBytes;

            using (var ms = new MemoryStream())
            {
                using (var aes = new AesManaged())
                {
                    SetStandardConfiguration(aes);

                    aes.Key = aesKey;

                    aes.GenerateIV();
                    aesIv = aes.IV;

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        /// <summary>
        /// <see cref="IAesManagedWrapper.Encrypt"/>
        /// </summary>
        public byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] aesKey, byte[] aesIv)
        {
            byte[] decryptedBytes;

            using (var ms = new MemoryStream())
            {
                using (var aes = new AesManaged())
                {
                    SetStandardConfiguration(aes);

                    aes.Key = aesKey;
                    aes.IV = aesIv;

                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        private static void SetStandardConfiguration(SymmetricAlgorithm aes)
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
        }
    }
}
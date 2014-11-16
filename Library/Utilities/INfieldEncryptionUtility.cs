using Nfield.Models;

namespace Nfield.Utilities
{
    /// <summary>
    /// NfieldEncryptionService uses an implementation of the Advanced Encryption Standard (AES) symmetric algorithm
    /// to encrypt and decrypt data.
    /// </summary>
    public interface INfieldEncryptionUtility
    {
        /// <summary>
        /// Encrypts the input using the supplied key by generating a random Initialization Vector during the process
        /// </summary>
        /// <param name="input">UTF8 input string</param>
        /// <param name="key">32 byte key that is in Base64 string format</param>
        /// <returns></returns>
        EncryptedDataModel EncryptText(string input, string key);

        /// <summary>
        /// Decrypts the input using the key and the initialization vector that was used to encrypt the input previously
        /// </summary>
        /// <param name="input">input in Base64 string format</param>
        /// <param name="key">32 byte key in Base64 string format</param>
        /// <param name="initializationVector">16 byte initialization vector in Base64 string format</param>
        /// <returns>decrypted text in UTF8 encoding</returns>
        string DecryptText(string input, string key, string initializationVector);

        /// <summary>
        /// Formats the encrypted data and the initialization vector into a querystring parameter as url-encoded
        /// </summary>
        /// <param name="model">encyrpted data model that contains data and the iv</param>
        /// <returns>querystring parameters in the format known to Nfield web interviewing</returns>
        string CreateQueryStringSegment(EncryptedDataModel model);
    }
}

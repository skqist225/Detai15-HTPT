using System;
using System.Security.Cryptography;
using System.Text;


namespace ServerApp
{
    public class SymmetricEncryptDecrypt
    {

        public (string Key, string IVBase64) InitSymmetricEncryptionKeyIV()
        {
            string key = GetEncodedRandomString(32); // 256
            Aes aes = CreateAesInstance(key);
            string IVBase64 = Convert.ToBase64String(aes.IV);
            return (key, IVBase64);
        }

        /// <summary>
        /// Encrypt using AES
        /// </summary>
        /// <param name="text">any text</param>
        /// <param name="IV">Base64 IV string/param>
        /// <param name="key">Base64 key</param>
        /// <returns>Returns an encrypted string</returns>
        public string Encrypt(string text, string IV, string key)
        {
            Aes aes = CreateAesInstance(key);
            aes.IV = Convert.FromBase64String(IV);

            ICryptoTransform cryptTransform = aes.CreateEncryptor();
            byte[] plaintext = Encoding.UTF8.GetBytes(text);
            byte[] cipherText = cryptTransform.TransformFinalBlock(plaintext, 0, plaintext.Length);

            return Convert.ToBase64String(cipherText);
        }

        /// <summary>
        /// Decrypt using AES
        /// </summary>
        /// <param name="text">Base64 string for an AES encryption</param>
        /// <param name="IV">Base64 IV string/param>
        /// <param name="key">Base64 key</param>
        /// <returns>Returns a string</returns>
        public string Decrypt(string encryptedText, string IV, string key)
        {
            Aes cipher = CreateAesInstance(key);
            cipher.IV = Convert.FromBase64String(IV);

            ICryptoTransform cryptTransform = cipher.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] plainBytes = cryptTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }

        private string GetEncodedRandomString(int length)
        {
            var byteArray = new byte[length];
            var random = RandomNumberGenerator.Create();
            random.GetNonZeroBytes(byteArray);

            string base64 = Convert.ToBase64String(byteArray);
            return base64;
        }

        /// <summary>
        /// Create an AES Cipher using a base64 key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>AES</returns>
        private Aes CreateAesInstance(string keyBase64)
        {
            // Default values: Keysize 256, Padding PKC27
            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC; // Ensure the integrity of the ciphertext if using CBC
            aes.Padding = PaddingMode.ISO10126;
            aes.Key = Convert.FromBase64String(keyBase64);

            return aes;
        }
    }
}

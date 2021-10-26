using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ServerApp
{
    public class AsymmetricEncryptDecrypt
    {
        // Encrypt with RSA using public key
        public string Encrypt(string plainText, string publicKey)
        {
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = byteConverter.GetBytes(plainText);

            byte[] encryptedData;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                // Set the rsa pulic key   
                rsa.FromXmlString(publicKey);

                // Encrypt the data and store it in the encyptedData Array   
                encryptedData = rsa.Encrypt(dataToEncrypt, false);
            }

            return Convert.ToBase64String(encryptedData);
        }

        // Decrypt with RSA using private key
        public string Decrypt(string encryptedText, string privateKey)
        {
            byte[] dataToDecrypt = Convert.FromBase64String(encryptedText);
            byte[] decryptedData;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                decryptedData = rsa.Decrypt(dataToDecrypt, false);
            }

            UnicodeEncoding byteConverter = new UnicodeEncoding();
            return byteConverter.GetString(decryptedData);
        }
    }
}

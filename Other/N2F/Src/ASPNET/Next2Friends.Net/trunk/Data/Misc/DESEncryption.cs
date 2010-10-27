using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Next2Friends.Misc
{

    public class DESEncryption
    {
        public static string Encrypt(string clearText, byte[] key)
        {
            // Create an encrypt object with the given key as its key and initialization vector.
            DESCryptoServiceProvider desEncryptor = new DESCryptoServiceProvider();
            ICryptoTransform cryptoTransform = desEncryptor.CreateEncryptor(key, key);

            // Call CryptoTransform to do the encryption.
            return CryptoTransform(clearText, cryptoTransform);
        }

        public static string EncryptForWeb(string clearText)
        {
            byte[] key = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64 };

            string enc = string.Empty;

            try
            {
                enc = DESEncryption.Encrypt(clearText, key);
            }
            catch (Exception)
            {
                return string.Empty;
            }

            enc = enc.Replace("&", "escape");

            return enc;
        }


        public static string DecryptForWeb(string clearText)
        {
            byte[] key = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64 };

            string dec = string.Empty;
            clearText = clearText.Replace("escape", "&");

            try
            {
                dec = DESEncryption.Decrypt(clearText, key);
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return dec;
        }

        public static string Decrypt(string encryptedText, byte[] key)
        {
            // Create a decrypt object with the given key as its key and initialization vector.
            DESCryptoServiceProvider desDecryptor = new DESCryptoServiceProvider();
            ICryptoTransform cryptoTransform = desDecryptor.CreateDecryptor(key, key);

            // Call CryptoTransform to do the decryption.
            return CryptoTransform(encryptedText, cryptoTransform);
        }

        public static string CryptoTransform(string text, ICryptoTransform cryptoTransform)
        {
            // Create a memory stream object.
            MemoryStream memoryStream = new MemoryStream();

            // Create a crypto stream object.
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);

            // Write the crypto transformed bytes to our memory stream.
            byte[] textBytes = System.Text.Encoding.Unicode.GetBytes(text);
            cryptoStream.Write(textBytes, 0, textBytes.Length);
            cryptoStream.Close();

            // Get the crypto transformed result as bytes from the memory stream.
            byte[] transformedBytes = memoryStream.ToArray();

            memoryStream.Close();

            // Converts the 8-bit unsigned integers to its equivalent Unicode String
            // representation and return the resulting string.
            //return System.Text.Encoding.Unicode.GetString(transformedBytes);
            return Convert.ToBase64String(transformedBytes);
        }

        public static string DecryptTagValidationString(string TagValidationString)
        {
            string strKey = "FFFFFFFFFFFFFFFFFXXXXXXX";
            byte[] key = System.Text.Encoding.UTF8.GetBytes(strKey);
            string TagValidationStringUNENC = string.Empty;

            try
            {
                TagValidationStringUNENC = DESEncryption.Decrypt(TagValidationString, key);
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return TagValidationStringUNENC;
        }
    }
}
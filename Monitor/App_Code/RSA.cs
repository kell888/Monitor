using System;
using System.Collections.Generic;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Monitor.App_Code
{
    public class RSA
    {
        RSAParameters privateKey;

        public string PrivateKey
        {
            get { return rsa.ToXmlString(true); }
        }
        RSAParameters publicKey;

        public string PublicKey
        {
            get { return rsa.ToXmlString(false); }
        }

        RSACryptoServiceProvider rsa;

        public RSA()
        {
            rsa = new RSACryptoServiceProvider();
            privateKey = rsa.ExportParameters(true);
            publicKey = rsa.ExportParameters(false);
        }
        public RSA(string key)
        {
            CspParameters RSAParams = new CspParameters();
            RSAParams.Flags = CspProviderFlags.UseMachineKeyStore;
            rsa = new RSACryptoServiceProvider(1024, RSAParams);
            rsa.FromXmlString(key);
            privateKey = rsa.ExportParameters(true);
            publicKey = rsa.ExportParameters(false);
        }

        public string Encrypt(string stringDataToEncrypt)
        {
            byte[] encryptedData = RSAHelper.RsaEncrypt(Encoding.Unicode.GetBytes(stringDataToEncrypt), publicKey.Exponent, publicKey.Modulus);
            return Convert.ToBase64String(encryptedData);
        }

        public string Decrypt(string encryptedBase64String)
        {
            byte[] encryptedData = RSAHelper.RsaDecrypt(Convert.FromBase64String(encryptedBase64String), privateKey.D, privateKey.Modulus);
            return Encoding.Unicode.GetString(encryptedData);
        }
    }

    public static class RSAHelper
    {
        /// <summary>
        /// RSAs the encrypt.
        /// </summary>
        /// <param name="datatoencrypt">The datatoencrypt.</param>
        /// <param name="exponent">The exponent.</param>
        /// <param name="modulus">The modulus.</param>
        /// <returns></returns>
        public static byte[] RsaEncrypt(byte[] datatoencrypt, byte[] exponent, byte[] modulus)
        {
            BigInteger original = new BigInteger(datatoencrypt);
            BigInteger e = new BigInteger(exponent);
            BigInteger n = new BigInteger(modulus);
            BigInteger encrypted = original.ModPow(e, n);
            return DataTranslate.HexStringToByte(encrypted.ToHexString());
        }

        /// <summary>
        /// RSAs the decrypt.
        /// </summary>
        /// <param name="encrypteddata">The encrypteddata.</param>
        /// <param name="d">The d.</param>
        /// <param name="modulus">The modulus.</param>
        /// <returns></returns>
        public static byte[] RsaDecrypt(byte[] encrypteddata, byte[] d, byte[] modulus)
        {
            BigInteger encrypted = new BigInteger(encrypteddata);
            BigInteger dd = new BigInteger(d);
            BigInteger n = new BigInteger(modulus);
            BigInteger decrypted = encrypted.ModPow(dd, n);
            return DataTranslate.HexStringToByte(decrypted.ToHexString());
        }

        /// <summary>
        /// Generate random bytes with given length
        /// </summary>
        /// <param name="bytelength"></param>
        /// <returns></returns>
        public static byte[] GenerateRandomBytes(int bytelength)
        {
            byte[] buff = new byte[bytelength];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buff);
            return buff;
        }

    }

    public static class DataTranslate
    {
        public static byte[] HexStringToByte(String hex)
        {
            int len = (hex.Length / 2);
            byte[] result = new byte[len];
            char[] achar = hex.ToCharArray();
            for (int i = 0; i < len; i++)
            {
                int pos = i * 2;
                result[i] = (byte)(ToByte(achar[pos]) << 4 | ToByte(achar[pos + 1]));
            }
            return result;
        }
        private static byte ToByte(char c)
        {
            byte b = (byte)"0123456789ABCDEF".IndexOf(c);
            return b;
        }
        public static String BytesToHexString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length);
            String sTemp;
            for (int i = 0; i < bytes.Length; i++)
            {
                sTemp = bytes[i].ToString("x");
                if (sTemp.Length < 2)
                    sb.Append(0);
                sb.Append(sTemp.ToUpper());
            }
            return sb.ToString();
        }
    }
}
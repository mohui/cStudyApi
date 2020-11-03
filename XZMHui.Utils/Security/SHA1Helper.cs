using System;
using System.Security.Cryptography;
using System.Text;

namespace XZMHui.Utils.Security
{
    public class SHA1Helper
    {
        /// <summary>
        /// 将字符串进行SHA1加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SHA1(string str)
        {
            SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
            string encoded = BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(str))).Replace("-", "");
            return encoded;
        }

        private static Encoding encoding = Encoding.UTF8;

        /// <summary>
        /// 获得一个字符串的加密密文
        /// </summary>
        /// <param name="plainTextString"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string SHA1WithSalt(string plainTextString, string salt)
        {
            byte[] passwordBytes = encoding.GetBytes(plainTextString);
            byte[] saltBytes = strToToHexByte(salt);

            byte[] buffer = new byte[passwordBytes.Length + saltBytes.Length];

            Buffer.BlockCopy(saltBytes, 0, buffer, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, buffer, saltBytes.Length, passwordBytes.Length);

            var hashAlgorithm = HashAlgorithm.Create("SHA1");
            var saltedSHA1Bytes = hashAlgorithm.ComputeHash(buffer);

            return salt + byteToHexStr(saltedSHA1Bytes);
        }

        private static byte[] strToToHexByte(string hexString)
        {
            int NumberChars = hexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("x2");
                }
            }
            return returnStr;
        }
    }
}
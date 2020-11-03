using System;
using System.Security.Cryptography;
using System.Text;

namespace XZMHui.Utils.Security
{
    public class DESHelper
    {
        #region DES加密

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="sInputString">输入字符</param>
        /// <param name="sKey">Key(8位长度字符串)</param>
        /// <param name="IV">偏移向量(8位长度字符串)</param>
        /// <returns>加密结果</returns>
        public static string DesEncrypt(string str, string key, string IV)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            byte[] result;
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = Encoding.UTF8.GetBytes(key);
            DES.IV = Encoding.UTF8.GetBytes(IV);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="sInputString">输入字符</param>
        /// <param name="sKey">Key(8位长度字符串)</param>
        /// <param name="IV">偏移向量(8位长度字符串)</param>
        /// <returns>解密结果</returns>
        public static string DesDecrypt(string str, string key, string IV)
        {
            byte[] data = Convert.FromBase64String(str);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = Encoding.UTF8.GetBytes(key);
            DES.IV = Encoding.UTF8.GetBytes(IV);
            ICryptoTransform desencrypt = DES.CreateDecryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return Encoding.UTF8.GetString(result);
        }

        #endregion DES加密
    }
}
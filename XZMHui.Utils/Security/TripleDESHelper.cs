using System;
using System.Security.Cryptography;
using System.Text;

namespace XZMHui.Utils.Security
{
    public class TripleDESHelper
    {
        #region 3Des加密

        /// <summary>
        /// 3Des加密
        /// </summary>
        /// <param name="str">要进行加密的字符串(内部对字符串采用utf8编码)</param>
        /// <param name="key">加密key(24位字符串)</param>
        /// <param name="IV">偏移向量(8位字符串)</param>
        /// <returns>base64编码的字符串</returns>
        public static string TripleDesEncrypt(string str, string key, string IV)
        {
            TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();

            //设置偏移向量
            tdsp.IV = Encoding.UTF8.GetBytes(IV);
            //设置加密密匙
            tdsp.Key = System.Text.Encoding.UTF8.GetBytes(key);
            //设置加密算法运算模式为ECB(保持和java兼容)
            tdsp.Mode = CipherMode.CBC;
            //设置加密算法的填充模式为PKCS7(保持和java兼容)
            tdsp.Padding = PaddingMode.PKCS7;

            //对输入字符串采用utf8编码获取字节
            byte[] data = Encoding.UTF8.GetBytes(str);

            //加密后采用base64编码生成加密串
            ICryptoTransform ct = tdsp.CreateEncryptor();
            string result = Convert.ToBase64String(ct.TransformFinalBlock(data, 0, data.Length));
            return result;
        }

        /// <summary>
        /// 3Des解密
        /// </summary>
        /// <param name="str">要进行解密base64字符串</param>
        /// <param name="key">解密key(24位字符串)</param>
        /// <param name="IV">偏移向量(8位字符串)</param>
        /// <returns>原始字符串(内部对字符串采用utf8进行解码)</returns>
        public static string TripleDesDecrypt(string str, string key, string IV)
        {
            TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
            tdsp.IV = Encoding.UTF8.GetBytes(IV);
            //设置偏移向量
            tdsp.IV = Encoding.UTF8.GetBytes(IV);
            //设置加密密匙
            tdsp.Key = System.Text.Encoding.UTF8.GetBytes(key);
            //设置加密算法运算模式为ECB(保持和java兼容)
            tdsp.Mode = CipherMode.CBC;
            //设置加密算法的填充模式为PKCS7(保持和java兼容)
            tdsp.Padding = PaddingMode.PKCS7;

            //加密串用base64编码,需要采用base64方式解析为字节
            byte[] data = Convert.FromBase64String(str);
            ICryptoTransform ct = tdsp.CreateDecryptor();
            //用utf8编码还原原始字符串
            string result = Encoding.UTF8.GetString(ct.TransformFinalBlock(data, 0, data.Length));
            return result;
        }

        #endregion 3Des加密
    }
}
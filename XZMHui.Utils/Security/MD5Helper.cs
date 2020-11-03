using System.Text;

namespace XZMHui.Utils.Security
{
    public class MD5Helper
    {
        #region MD5加密

        /// <summary>
        /// 将字符串进行Md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(string str)
        {
            return MD5(str, Encoding.UTF8);
        }

        /// <summary>
        /// 将字符串进行Md5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(string str, Encoding encode)
        {
            if (string.IsNullOrEmpty(str)) return str;
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] buffer = md5.ComputeHash(encode.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            foreach (var b in buffer)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        #endregion MD5加密
    }
}
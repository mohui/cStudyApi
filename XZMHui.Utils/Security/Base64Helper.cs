using System.Text;

namespace XZMHui.Utils.Security
{
    public class Base64Helper
    {
        #region Base64加密

        /// <summary>
        /// 将字符串进行Base64编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Encrypt(string str)
        {
            string result = string.Empty;

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                result = System.Convert.ToBase64String(bytes);
            }
            catch
            {
                result = str;
            }
            return result;
        }

        /// <summary>
        /// 将字符串进行Base64解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Decrypt(string str)
        {
            string result = string.Empty;
            try
            {
                byte[] bytes = System.Convert.FromBase64String(str);
                result = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                //base 64 字符数组为null
                result = str;
            }

            return result;
        }

        #endregion Base64加密
    }
}
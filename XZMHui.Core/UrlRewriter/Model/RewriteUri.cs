namespace XZMHui.Core.UrlRewriter.Model
{
    public class RewriteUri
    {
        /// <summary>
        /// 转发检测的地址前缀
        /// </summary>
        public string PathPrefix { get; set; }

        /// <summary>
        /// 转发的主机地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 是否移除检测前缀进行转发
        /// </summary>
        public bool RemovePrefix { get; set; }
    }
}
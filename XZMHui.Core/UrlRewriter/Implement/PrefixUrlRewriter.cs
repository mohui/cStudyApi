using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XZMHui.Core.UrlRewriter.Interface;
using XZMHui.Core.UrlRewriter.Model;

namespace XZMHui.Core.UrlRewriter.Implement
{
    public class PrefixUrlRewriter : IUrlRewriter
    {
        private readonly IEnumerable<RewriteUri> _rewriteUris;

        /// <summary>
        /// 初始化转发器
        /// </summary>
        /// <param name="rewriteUri">检测的前缀</param>
        public PrefixUrlRewriter(IEnumerable<RewriteUri> rewriteUris)
        {
            _rewriteUris = rewriteUris;
        }

        public Task<Uri> RewriteUri(HttpContext context)
        {
            var rewriteUri = _rewriteUris.First(x => context.Request.Path.StartsWithSegments(x.PathPrefix));
            if (rewriteUri != null)//判断访问是否含有前缀
            {
                var newUri = rewriteUri.RemovePrefix ? (context.Request.Path.Value.Remove(0, rewriteUri.PathPrefix.Length) + context.Request.QueryString) : context.Request.QueryString.Value;
                var targetUri = new Uri(rewriteUri.Host + newUri);
                return Task.FromResult(targetUri);
            }

            return Task.FromResult((Uri)null);
        }
    }
}
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace XZMHui.Core.UrlRewriter.Interface
{
    public interface IUrlRewriter
    {
        Task<Uri> RewriteUri(HttpContext context);
    }
}
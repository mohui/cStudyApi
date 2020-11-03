using Microsoft.AspNetCore.Builder;
using XZMHui.Core.UrlRewriter.Middleware;

namespace XZMHui.Core.UrlRewriter
{
    public static class UrlRewriterApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseUrlRewriter(this IApplicationBuilder app)
        {
            app.UseMiddleware<UrlRewriterMiddleware>();

            return app;
        }
    }
}
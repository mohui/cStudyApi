using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using XZMHui.Core.UrlRewriter.Implement;
using XZMHui.Core.UrlRewriter.Interface;
using XZMHui.Core.UrlRewriter.Model;

namespace XZMHui.Core.UrlRewriter
{
    public static class UrlRewriterServiceCollectionExtensions
    {
        public static IServiceCollection AddUrlRewriter(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider()
                 .GetService<IConfiguration>();

            var rewriteOptions = new RewriteUriOptions();
            configuration.GetSection("UrlRewrite").Bind(rewriteOptions);

            services.AddHttpClient<ProxyHttpClient>()
                .ConfigurePrimaryHttpMessageHandler(x => new HttpClientHandler()
                {
                    AllowAutoRedirect = false,
                    MaxConnectionsPerServer = int.MaxValue,
                    UseCookies = false,
                });

            // 注入代理HttpClient
            services.AddSingleton<IUrlRewriter>(new PrefixUrlRewriter(rewriteOptions.RewriteUris));

            return services;
        }
    }
}
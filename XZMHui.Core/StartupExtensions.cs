using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using XZMHui.Core.Filters;
using XZMHui.Core.Middlewares;

namespace XZMHui.Core
{
    public static class StartupExtensions
    {
        public static IApplicationBuilder UseCoreStartup(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.UseExceptionHandlerMiddleware();
            });

            app.UseStaticFiles();

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            //app.UseJwtService();

            app.UseAuthorization();

            // 业务异常处理
            app.UseAntiExceptionMiddleware();

            return app;
        }

        public static IServiceCollection AddCoreStartup(this IServiceCollection services)
        {
            IConfiguration config = services.BuildServiceProvider()
               .GetService<IConfiguration>();

            var _routePrefix = config["RoutePrefix"];

            // 跨域设置
            services.AddCors(options =>
            {
                // Policy 名称
                options.AddDefaultPolicy(policy =>
                {
                    policy
                    //.SetIsOriginAllowed(_=>true)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    //.AllowCredentials()
                    .AllowAnyOrigin();
                });
            });

            services.AddControllers(opt =>
             {
                 // opt.Filters.Add<ExceptionFilterAttribute>();
                 opt.Filters.Add<ValidationResultFilterAttribute>();
                 opt.UseCentralRoutePrefix(new Microsoft.AspNetCore.Mvc.RouteAttribute(string.IsNullOrEmpty(_routePrefix) ? "" : $"{_routePrefix.TrimEnd('/')}/"));
             })
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new XZMHui.Core.Converters.DateTimeConverter());

                // 数据格式原样输出
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            // 设置上传文件的大小
            long.TryParse(config.GetSection("FormOptions:MaxRequestBodySize").Value, out long bufferBodyLengthLimit);
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = bufferBodyLengthLimit == 0 ? long.MaxValue : bufferBodyLengthLimit;// 62914560;
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = bufferBodyLengthLimit == 0 ? long.MaxValue : bufferBodyLengthLimit; // 62914560;
                options.AllowSynchronousIO = true;
            });

            // services.AddJwtService();

            return services;
        }
    }
}
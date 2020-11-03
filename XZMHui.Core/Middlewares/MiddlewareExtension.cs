using Microsoft.AspNetCore.Builder;

namespace XZMHui.Core.Middlewares
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseAntiExceptionMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<CoreExceptionMiddleware>();
            return applicationBuilder;
        }

        public static IApplicationBuilder UsePerformanceLogMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<PerformanceLogMiddleware>();
            return applicationBuilder;
        }

        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseMiddleware<ExceptionHandlerMiddleware>();
            return applicationBuilder;
        }
    }
}
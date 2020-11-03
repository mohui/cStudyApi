using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace XZMHui.Core.Middlewares
{
    public class PerformanceLogMiddleware
    {
        private readonly RequestDelegate _next;

        public PerformanceLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var profiler = new Stopwatch();
            profiler.Start();
            await _next(context);
            profiler.Stop();

            var logger = ((ILoggerFactory)context.RequestServices.GetService(typeof(ILoggerFactory)))
                .CreateLogger("API.PerformanceLog");
            logger.LogInformation($"{context.Request.Method}, Path:{context.Request.Path}, ElapsedMilliseconds:{profiler.ElapsedMilliseconds}ms, {context.Request.ContentType} {context.Response.StatusCode}");
        }
    }
}
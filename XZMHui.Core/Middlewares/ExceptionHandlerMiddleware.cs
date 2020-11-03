using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using XZMHui.Core.Model;

namespace XZMHui.Core.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
        }

        public async Task InvokeAsync(HttpContext context, IWebHostEnvironment env)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var ret = new CoreResult<object>(APIResultCode.UnknownError, "服务器发生异常. ");

            var exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();
            ret.Message += exceptionHandlerPathFeature.Error?.Message;

            if (env.IsDevelopment())
                ret.Data = exceptionHandlerPathFeature.Error;

            await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(ret));
        }
    }
}
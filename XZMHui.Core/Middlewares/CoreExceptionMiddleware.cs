using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;
using XZMHui.Core.Model;

namespace XZMHui.Core.Middlewares
{
    /// <summary>
    /// jian dan de yi chang chu li zhong jian jian"
    /// </summary>
    public class CoreExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CoreExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CoreException ex)
            {
                await context.Response.WriteAsync(
                    Newtonsoft.Json.JsonConvert.SerializeObject(new CoreResult<CoreException>(APIResultCode.UnknownError, ex.Message)), Encoding.Default);
            }
        }
    }
}
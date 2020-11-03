using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using XZMHui.Core.Jwt.Model;
using XZMHui.Core.Jwt.Utils;

namespace XZMHui.Core.Jwt.Middlewares
{
    /// <summary>
    /// jian dan de yi chang chu li zhong jian jian"
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers[JwtOptions.HeaderTokenName].FirstOrDefault();
            if (token != null)
            {
                var jwt = JwtHelper.IssueJwt(token);
                context.Request.Headers.Add("Authorization", "Bearer " + jwt);
            }
            await _next(context);
        }
    }
}
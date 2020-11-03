using Microsoft.AspNetCore.Builder;
using XZMHui.Core.Jwt.Middlewares;

namespace DBHS.SPD.DepartmentApp.Core.Jwt
{
    public static class JwtApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseJwtService(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthentication();

            return app;
        }
    }
}
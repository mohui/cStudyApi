using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using XZMHui.Core.Jwt.Model;
using XZMHui.Core.Model;

namespace DBHS.SPD.DepartmentApp.Core.Jwt
{
    public static class JwtServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtService(this IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            IConfiguration config = services.BuildServiceProvider()
                .GetService<IConfiguration>();

            JwtOptions.IssuerSigningKey = config["Jwt:IssuerSigningKey"];
            JwtOptions.HeaderTokenName = config["Jwt:HeaderTokenName"];

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        SaveSigninToken = true,//保存token,后台验证token是否生效(重要)
                        ValidateLifetime = false,//是否验证失效时间
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.IssuerSigningKey))
                    };
                    options.Events = new JwtBearerEvents()
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.Clear();
                            context.Response.ContentType = "application/json";
                            context.Response.StatusCode = 401;
                            context.Response.WriteAsync(JsonConvert.SerializeObject(new CoreResult<object>(APIResultCode.AuthError, "Authorization验证失败.")));
                            return Task.CompletedTask;
                        },
                        //OnAuthenticationFailed = context =>
                        //{
                        //    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        //    {
                        //        context.Response.Headers.Add("token-expired", "true");
                        //        context.Response.Headers["Access-Control-Expose-Headers"] = "token-expired";
                        //    }
                        //    return Task.CompletedTask;
                        //}
                    };
                });

            return services;
        }
    }
}
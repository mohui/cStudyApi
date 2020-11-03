using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using XZMHui.Core;
using XZMHui.Services;

namespace XZMHui.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSwaggerGen(option =>
                {
                    // 添加请求头验证
                    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description = "Json Web Token 验证请求头使用Bearer模式. 请求头参数示例: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",//Jwt default param name
                        In = ParameterLocation.Header,//Jwt store address
                        Type = SecuritySchemeType.ApiKey//Security scheme type
                    });

                    // 加验证类型为Bearer
                    option.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });

                    option.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "XZMHui.Api",
                        Description = "spd DepartmentApp ASP.NET Core Web API",
                    });

                    // IGeekFan.AspNetCore.Knife4jUI 必须的配置
                    option.AddServer(new OpenApiServer()
                    {
                        Url = "",
                        Description = "vvv"
                    });
                    // IGeekFan.AspNetCore.Knife4jUI 必须的配置
                    option.CustomOperationIds(apiDesc =>
                    {
                        var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                        return controllerAction.ControllerName + "-" + controllerAction.ActionName;
                    });

                    // Set the comments path for the Swagger JSON and UI.
                    var assName = Assembly.GetExecutingAssembly().GetName().Name;
                    var files = System.IO.Directory.GetFiles(AppContext.BaseDirectory, "*.xml").Where(x =>
                    {
                        var file = new FileInfo(x);
                        return file.Name.StartsWith(assName.Substring(0, assName.LastIndexOf('.')));
                    });
                    foreach (var item in files)
                    {
                        option.IncludeXmlComments(item);
                    }
                });

            services.AddCoreStartup();

            services.AddDbContext<XZMHui.Repository.MyDbContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("Test93DB"));
            });
            services.AddModelServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseKnife4UI(c =>
                {
                    c.RoutePrefix = "swagger"; // serve the UI at root
                    c.SwaggerEndpoint("/v1/api-docs", "V1 Docs");
                });

                //app.UseSwaggerUI(option =>
                //{
                //    option.SwaggerEndpoint("/swagger/v1/swagger.json", "spd DepartmentApp api v1");
                //});
            }

            app.UseCoreStartup();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                if (env.IsDevelopment()) endpoints.MapSwagger("{documentName}/api-docs");
            });
        }
    }
}
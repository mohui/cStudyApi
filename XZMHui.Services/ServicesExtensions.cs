using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using XZMHui.Core.Attributes;
using XZMHui.Repository;

namespace XZMHui.Services
{
    [SkipInject]
    public static class ServicesExtensions
    {
        public static IServiceCollection AddModelServices(this IServiceCollection services)
        {
            services.AddRepository();

            var di = services.BuildServiceProvider()
                 .GetService<IConfiguration>()
                 .GetSection("DI:Implements")
                 .AsEnumerable()
                 .Where(x => x.Value != null)
                 .Select(x =>
                 {
                     var tmp = x.Value.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
                     if (tmp.Length != 2) throw new ArgumentException("DI 实现配置错误，请使用 interface,implement 的方式配置");
                     return new
                     {
                         Interface = tmp[0],
                         Implement = tmp[1]
                     };
                 })
                 .ToList();

            var assTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(x => !x.IsInterface && !x.IsDefined(typeof(SkipInjectAttribute), false));
            var ifce = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsInterface);

            var injectHistory = new HashSet<string>();
            foreach (var item in assTypes)
            {
                var intersect = item.GetInterfaces().Intersect(ifce);
                if (intersect.Count() > 0)
                    foreach (var face in intersect)
                    {
                        // 动态注入，可根据配置注入，其他不需要的实现不注入 Impletements:["XZMHui.Services.ISample, XZMHui.Services.SampleB"]
                        var dep = di.FirstOrDefault(x => x.Interface == face.FullName);
                        if (dep != null)
                        {
                            if (injectHistory.Contains(face.FullName)) continue;
                            // 为指定接口指定实现
                            services.Add(new ServiceDescriptor(face, Type.GetType(dep.Implement), ServiceLifetime.Transient));
                            injectHistory.Add(face.FullName);
                        }
                        else
                            services.Add(new ServiceDescriptor(face, item, ServiceLifetime.Transient));
                    }
                else
                    services.AddTransient(item);
            }

            return services;
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using XZMHui.Core.Attributes;

namespace XZMHui.Repository
{
    [SkipInject]
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            var assTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(x => !x.IsInterface && !x.IsDefined(typeof(SkipInjectAttribute), false));

            foreach (var item in assTypes)
            {
                services.AddTransient(item);
            }

            return services;
        }
    }
}
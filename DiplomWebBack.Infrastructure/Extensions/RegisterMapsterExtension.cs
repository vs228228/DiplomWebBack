using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DiplomWebBack.Infrastructure.Extensions
{
    public static class RegisterMapsterExtension
    {
        public static IServiceCollection AddMapsterMappings(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;

            // Сканируем все IRegister
            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}

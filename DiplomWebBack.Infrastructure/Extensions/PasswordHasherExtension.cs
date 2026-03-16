using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DiplomWebBack.Infrastructure.Extensions
{
    public static class PasswordHasherExtension
    {
        public static IServiceCollection AddPasswordHasher(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            return services;
        }
    }
}

using DiplomWebBack.Application.Services.Implementation;
using DiplomWebBack.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DiplomWebBack.Application.Services
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterAppService(this IServiceCollection services)
        {
            services.AddScoped<IProjectVerificationService, ProjectVerificationService>();
            services.AddScoped<IUserVerificationService, UserVerificationService>();

            return services;
        }
    }
}

using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Infrastructure.Hubs;
using DiplomWebBack.Infrastructure.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace DiplomWebBack.Infrastructure.Extensions
{
    public static class HubsExtensions
    {
        public static IServiceCollection AddNotificationInfrastructure(this IServiceCollection services)
        {
            services.AddSignalR();

            services.AddScoped<INotificationService, NotificationService>();

            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            return services;
        }
    }
}
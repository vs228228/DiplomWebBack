using DiplomWebBack.Domain.Repos;
using DiplomWebBack.DomainRepos.Repos;
using DiplomWebBack.Infrastructure.Repos;
using Microsoft.Extensions.DependencyInjection;

namespace DiplomWebBack.Infrastructure.Extensions
{
    public static class RegisterReposExtension
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITagsRepository, TagsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserActivatorRepository, UserActivatorRepository>();

            return services;
        }
    }
}

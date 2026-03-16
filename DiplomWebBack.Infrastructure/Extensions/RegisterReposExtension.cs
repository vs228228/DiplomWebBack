using DiplomWebBack.Domain.Repos;
using DiplomWebBack.DomainRepos.Repos;
using DiplomWebBack.Infrastructure.Repos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Infrastructure.Extensions
{
    public static class RegisterReposExtension
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
          //  services.AddScoped<IBaseRepository<T>, BaseRepository<T>();
            services.AddScoped<ITagsRepository, TagsRepository>();

            return services;
        }
    }
}

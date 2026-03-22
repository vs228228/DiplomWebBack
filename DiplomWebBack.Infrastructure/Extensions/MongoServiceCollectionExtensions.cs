using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DiplomWebBack.Infrastructure.Extensions
{
    public static class MongoServiceCollectionExtensions
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoSection = configuration.GetSection("MongoDb");
            var connectionString = mongoSection["ConnectionString"];
            var databaseName = mongoSection["Database"];

            services.AddSingleton<IMongoClient>(_ =>
                new MongoClient(connectionString));

            services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(databaseName);
            });

            return services;
        }
    }
}

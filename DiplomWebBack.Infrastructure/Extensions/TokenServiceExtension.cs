using DiplomWebBack.Domain.Interfaces;
using DiplomWebBack.Infrastructure.Services;
using DiplomWebBack.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DiplomWebBack.Infrastructure.Extensions
{
    public static class TokenServiceExtension
    {
        public static IServiceCollection AddTokenManager(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSection);

            var jwtSettings = jwtSection.Get<JwtSettings>();

            services.AddScoped<ITokenService>(_ => new TokenService(jwtSettings));

            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // для HTTP (dev)
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ClockSkew = TimeSpan.Zero
                };

                // 🔥 ВОТ ГЛАВНОЕ — поддержка и header, и cookie
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // 1️⃣ Пытаемся достать из Authorization header
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

                        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                        {
                            context.Token = authHeader.Substring("Bearer ".Length);
                            return Task.CompletedTask;
                        }

                        // 2️⃣ Если нет — берём из cookie
                        var tokenFromCookie = context.Request.Cookies["accessToken"];

                        if (!string.IsNullOrEmpty(tokenFromCookie))
                        {
                            context.Token = tokenFromCookie;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
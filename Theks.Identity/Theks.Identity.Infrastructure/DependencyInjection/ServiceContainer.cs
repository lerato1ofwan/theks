using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Theks.Identity.Application.Interfaces;
using Theks.Identity.Infrastructure.Configurations;
using Theks.Identity.Infrastructure.Data;
using Theks.Identity.Infrastructure.Extensions;
using Theks.Identity.Infrastructure.Repositories;
using Theks.Identity.Infrastructure.Services.Security;
using Theks.Shared.DependencyInjection;

namespace Theks.Identity.Infrastructure.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        SharedServicesContainer.AddSharedServices<ApplicationDbContext>(
            services,
            configuration,
            configuration["SERILOG_FILENAME"]!
        );

        var authenticationOptions = new AuthenticationOptions
        {
            Key = configuration["AUTHENTICATION_KEY"]
                ?? throw new Exception("Environment: Missing AUTHENTICATION_KEY"),

            Issuer = configuration["AUTHENTICATION_ISSUER"]
                ?? throw new Exception("Environment: Missing AUTHENTICATION_ISSUER"),

            Audience = configuration["AUTHENTICATION_AUDIENCE"]
                ?? throw new Exception("Environment: Missing AUTHENTICATION_AUDIENCE")
        };

        services.AddSingleton(authenticationOptions);

        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
    {
        SharedServicesContainer.UseSharedPolicies(app);
        
        app.MigrateDatabase();
        
        return app;
    }
}
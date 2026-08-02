using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Theks.Identity.Application.Interfaces;
using Theks.Identity.Application.Services;
using Theks.Shared.DependencyInjection;

namespace Theks.Identity.Application.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    public static IApplicationBuilder useInfrastructurePolicy(this IApplicationBuilder app)
    {
        SharedServicesContainer.UseSharedPolicies(app);

        return app;
    }
}
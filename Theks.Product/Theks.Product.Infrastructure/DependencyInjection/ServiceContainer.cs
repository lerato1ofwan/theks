using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Theks.Product.Application.Interfaces;
using Theks.Product.Infrastructure.Data;
using Theks.Product.Infrastructure.Repositories;
using Theks.Shared.DependencyInjection;

namespace Theks.Product.Infrastructure;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register shared services
        SharedServicesContainer.AddSharedServices<ProductDbContext>(
            services, 
            configuration,
            configuration["SERILOG_FILENAME"]!
        );

        // Register services
        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }

    public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
    {
        // Register middleware
        SharedServicesContainer.UseSharedPolicies(app);

        return app;
    }
}
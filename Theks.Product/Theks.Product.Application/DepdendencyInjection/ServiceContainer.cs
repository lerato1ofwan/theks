using Microsoft.Extensions.DependencyInjection;
using Theks.Product.Application.Interfaces;
using Theks.Product.Application.Services;

namespace Theks.Product.Application.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}

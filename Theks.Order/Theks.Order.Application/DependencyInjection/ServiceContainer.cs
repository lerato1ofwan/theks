using Microsoft.Extensions.DependencyInjection;
using Theks.Order.Application.Interfaces;
using Theks.Order.Application.Services;

namespace Theks.Order.Application.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;
using Theks.Order.Application.Interfaces;
using Theks.Order.Infrastructure.Data;
using Theks.Order.Infrastructure.Repositories;
using Theks.Order.Infrastructure.Services;
using Theks.Shared.DependencyInjection;
using Theks.Shared.Logs;

namespace Theks.Order.Infrastructure.DependencyInjection;

public static class ServiceContainer
{
    private static int DefaultTimeoutValueInSeconds = 3;
    private static int DefaultRetryMaxAttemptsValue = 3;
    private static int DefaultRetryAttemptDelayValueInMillisecond = 500;

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        SharedServicesContainer.AddSharedServices<ApplicationDbContext>(
            services,
            configuration,
            configuration["SERILOG_FILENAME"]!
        );


        services.AddScoped<IOrderRepository, OrderRepository>();

        ConfigureHttpClients(services, configuration);

        var retryStrategy = new RetryStrategyOptions()
        {
            ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            MaxRetryAttempts = DefaultRetryMaxAttemptsValue,
            Delay = TimeSpan.FromMilliseconds(DefaultRetryAttemptDelayValueInMillisecond),
            OnRetry = args =>
            {
                string message = $"OnRetry, Attempt: {args.AttemptNumber + 1}/{DefaultRetryMaxAttemptsValue} Outcome: {args.Outcome}";
                LogException.LogToConsole(message);
                LogException.LogToDebugger(message);
                return ValueTask.CompletedTask;
            }
        };
        services.AddResiliencePipeline("Theks.Order.RetryPipeline", builder =>
        {
            builder.AddRetry(retryStrategy);
        });


        return services;
    }

    private static void ConfigureHttpClients(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IProductService, ProductService>(options =>
        {
            var baseAddress = configuration["PRODUCT_API_BASE_URL"] ?? throw new InvalidOperationException("PRODUCT_API_BASE_URL is missing.");
            options.BaseAddress = new Uri(baseAddress);
            options.Timeout = TimeSpan.FromSeconds(DefaultTimeoutValueInSeconds);
        });
        // Configure HttpClient for User Service
        services.AddHttpClient<IUserService, UserService>(options =>
        {
            // Update this configuration key to your User service URL when ready
            var baseAddress = configuration["USER_API_BASE_URL"] ?? throw new InvalidOperationException("USER_API_BASE_URL is missing.");
            options.BaseAddress = new Uri(baseAddress!);
            options.Timeout = TimeSpan.FromSeconds(DefaultTimeoutValueInSeconds);
        });
    }

    public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
    {
        SharedServicesContainer.UseSharedPolicies(app);

        return app;
    }
}

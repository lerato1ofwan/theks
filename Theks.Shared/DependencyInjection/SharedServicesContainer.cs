using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Theks.Shared.Middleware;

namespace Theks.Shared.DependencyInjection;

public static class SharedServicesContainer
{
    public static IServiceCollection AddSharedServices<TContext>(
        this IServiceCollection services, IConfiguration configuration, string fileName) where TContext : DbContext
    {
        // @Hint: Generic database context
        services.AddDbContext<TContext>(option => option.UseSqlServer(
                configuration.GetValue<string>("DB_CONNECTION_STRING"),
                sqlserverOption => sqlserverOption.EnableRetryOnFailure()));

        Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
            .WriteTo.Debug()
            .WriteTo.Console()
            .WriteTo.File(path: $"{fileName}-.text", 
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information, 
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();

        JwtAuthenticationScheme.AddJwtAuthenticationScheme(services, configuration);

        return services;
    }

    public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalException>();
        //app.UseMiddleware<ApiGatewayListener>();

        return app;
    }
}
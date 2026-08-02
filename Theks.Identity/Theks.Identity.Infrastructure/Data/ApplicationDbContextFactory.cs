using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Theks.Identity.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        var conn = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(conn))
        {
            var host = Environment.GetEnvironmentVariable("DB_HOSTNAME") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var db = Environment.GetEnvironmentVariable("DB_NAME") ?? "theks-identity";
            var user = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "sa";
            var pass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";

            if (!string.IsNullOrEmpty(port)) host = $"{host},{port}";
            conn = $"Server={host};Database={db};User Id={user};Password={pass};Encrypt=True;TrustServerCertificate=True;";
        }

        optionsBuilder.UseSqlServer(conn);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}

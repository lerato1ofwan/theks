using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Theks.Product.Infrastructure.Data;

public class ProductDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
{
    public ProductDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();

        var conn = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(conn))
        {
            var host = Environment.GetEnvironmentVariable("DB_HOSTNAME") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT");
            var db = Environment.GetEnvironmentVariable("DB_NAME") ?? "theks-products";
            var user = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "sa";
            var pass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";

            if (!string.IsNullOrEmpty(port)) host = $"{host},{port}"; // SQL Server uses comma-separated port
            conn = $"Server={host};Database={db};User Id={user};Password={pass};Encrypt=True;TrustServerCertificate=True;";
        }

        optionsBuilder.UseSqlServer(conn);
        return new ProductDbContext(optionsBuilder.Options);
    }
}

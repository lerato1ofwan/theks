using Theks.Identity.Application.DependencyInjection;
using Theks.Identity.Infrastructure.DependencyInjection;
using NSwag.AspNetCore;
using NSwag;
using NSwag.Generation.AspNetCore;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Use NSwag to provide interactive OpenAPI/Swagger UI
builder.Services.AddOpenApiDocument();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseInfrastructurePolicy();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // NSwag middleware provides an interactive Swagger UI
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

// Authentication/Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
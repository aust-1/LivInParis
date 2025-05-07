using LivInParisRoussilleTeynier.Domain.Models.Maps;
using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using LivInParisRoussilleTeynier.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Configure database context with MySQL using connection string from appsettings.json.
/// </summary>
builder.Services.AddDbContext<LivInParisContext>();

/// <summary>
/// Register generic repository and concrete repositories for domain entities.
/// </summary>
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IChefRepository, ChefRepository>();
builder.Services.AddScoped<IDishRepository, DishRepository>();
builder.Services.AddScoped<IOrderTransactionRepository, OrderTransactionRepository>();
builder.Services.AddScoped<IOrderLineRepository, OrderLineRepository>();
builder.Services.AddScoped<IContainsRepository, ContainsRepository>();

/// <summary>
/// Register graph service for pathfinding and algorithms.
/// </summary>
// builder.Services.AddScoped<IGrapheService, GrapheService>();

/// <summary>
/// Allow Cross-Origin Requests from frontend during development.
/// </summary>
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});

/// <summary>
/// Add controllers for API endpoints.
/// </summary>
builder.Services.AddControllers();

/// <summary>
/// Add Swagger for API documentation and testing.
/// </summary>
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// <summary>
/// Serve production build of frontend from "frontend/build" folder.
/// </summary>
builder.Services.AddSpaStaticFiles(options =>
{
    options.RootPath = Path.Combine(builder.Environment.ContentRootPath, "frontend", "build");
});

var app = builder.Build();

app.UseCors("AllowAll");

/// <summary>
/// In development, enable Swagger UI.
/// </summary>
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    /// <summary>
    /// Proxy to frontend dev server in development mode.
    /// </summary>
    app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "frontend";
        spa.UseProxyToSpaDevelopmentServer("http://host.docker.internal:53754");
    });
}
else
{
    /// <summary>
    /// Serve static files for production.
    /// </summary>
    app.UseStaticFiles();
    app.UseSpaStaticFiles();

    app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "frontend";
    });
}

/// <summary>
/// Map controllers for API.
/// </summary>
app.MapControllers();

if (!app.Environment.IsDevelopment())
{
    /// <summary>
    /// Fallback to index.html for client-side routing in production.
    /// </summary>
    app.MapFallbackToFile("index.html");
}

await app.RunAsync();

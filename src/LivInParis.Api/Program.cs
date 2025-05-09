using DotNetEnv;
using LivInParisRoussilleTeynier.API;
using LivInParisRoussilleTeynier.Domain.Models.Maps;
using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using LivInParisRoussilleTeynier.Infrastructure.Repositories;
using LivInParisRoussilleTeynier.Services;
using LivInParisRoussilleTeynier.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SpaServices.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

Env.Load(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", ".env"));

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

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IIndividualRepository, IndividualRepository>();
builder.Services.AddScoped<IMenuProposalRepository, MenuProposalRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

/// <summary>
/// Register service layer implementations
/// </summary>
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IChefProfileService, ChefProfileService>();
builder.Services.AddScoped<ICustomerProfileService, CustomerProfileService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IGraphService, GraphService>();
builder.Services.AddScoped<IIncomingOrderService, IncomingOrderService>();
builder.Services.AddScoped<IMenuProposalService, MenuProposalService>();
builder.Services.AddScoped<IOrderLineService, OrderLineService>();
builder.Services.AddScoped<ITokenService, TokenService>();

//builder.Services.AddScoped<IReviewService, ReviewService>();
//builder.Services.AddScoped<IStatisticsService, StatisticsService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

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
builder.Services.AddControllers().AddXmlSerializerFormatters();

/// <summary>
/// Add Swagger for API documentation and testing.
/// </summary>
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// <summary>
/// Serve production build of frontend from "frontend" folder.
/// </summary>
builder.Services.AddSpaStaticFiles(options =>
{
    options.RootPath = Path.Combine(builder.Environment.ContentRootPath, "frontend");
});

var app = builder.Build();

Metro.InitializeMetro();

app.UseCors("AllowAll");

/// <summary>
/// In development, enable Swagger UI.
/// </summary>
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    /// <summary>
    /// Serve frontend in development from static files or external dev server specified in config.
    /// </summary>
    app.UseSpa(spa =>
    {
        spa.Options.SourcePath = "frontend";
        var spaUrl = builder.Configuration["SpaDevServerUrl"];
        if (!string.IsNullOrEmpty(spaUrl))
            spa.UseProxyToSpaDevelopmentServer(spaUrl);
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

if (!app.Environment.IsDevelopment())
{
    /// <summary>
    /// Fallback to index.html for client-side routing in production.
    /// </summary>
    app.MapFallbackToFile("index.html");
}

/// <summary>
/// Map controllers for API.
/// </summary>
app.MapControllers();

await app.RunAsync();

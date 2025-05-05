using LivInParisRoussilleTeynier.Domain.Models.Maps;
using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using LivInParisRoussilleTeynier.Infrastructure.Repositories;

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
//builder.Services.AddScoped<IGrapheService, GrapheService>();

/// <summary>
/// Enable Cross-Origin Requests to allow frontend (localhost:3000) to call this API.
/// </summary>
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

/// <summary>
/// Add Swagger for API documentation and testing.
/// </summary>
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/// <summary>
/// Use CORS policy globally.
/// </summary>
app.UseCors();

/// <summary>
/// In development, enable Swagger UI.
/// </summary>
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/// <summary>
/// Simple endpoint to get all customers.
/// </summary>
app.MapGet("/clients", async (ICustomerRepository repo) => await repo.GetAllAsync());

/// <summary>
/// Simple endpoint to add a new customer.
/// </summary>
app.MapPost(
    "/clients",
    async (ICustomerRepository repo, Customer customer) =>
    {
        await repo.AddAsync(customer);
        return Results.Created($"/clients/{customer.CustomerAccountId}", customer);
    }
);

/// <summary>
/// Simple endpoint to get all dishes.
/// </summary>
app.MapGet("/dishes", async (IDishRepository repo) => await repo.GetAllAsync());

/// <summary>
/// Simple endpoint to calculate shortest path between two station IDs.
/// </summary>
// app.MapGet(
//     "/graph/path/{fromId:int}/{toId:int}",
//     async (IGrapheService service, int fromId, int toId) =>
//     {
//         var result = await service.FindShortestPathAsync(fromId, toId);
//         return result is not null ? Results.Ok(result) : Results.NotFound();
//     }
// );

await app.RunAsync();

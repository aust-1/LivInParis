using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExportController : ControllerBase
{
    private readonly LivInParisContext _context;

    public ExportController(LivInParisContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Export full database in JSON format.
    /// </summary>
    [HttpGet("json")]
    [Produces("application/json")]
    public async Task<IActionResult> ExportJson()
    {
        var export = await BuildExportObjectAsync();
        return Ok(export);
    }

    /// <summary>
    /// Export full database in XML format.
    /// </summary>
    [HttpGet("xml")]
    [Produces("application/xml")]
    public async Task<IActionResult> ExportXml()
    {
        var export = await BuildExportObjectAsync();
        return Ok(export);
    }

    /// <summary>
    /// Builds a DTO containing all tables to export.
    /// </summary>
    private async Task<FullDatabaseExportDto> BuildExportObjectAsync()
    {
        var accounts = await _context.Accounts.ToListAsync();
        var chefs = await _context.Chefs.ToListAsync();
        var customers = await _context.Customers.ToListAsync();
        var individuals = await _context.Individuals.ToListAsync();
        var companies = await _context.Companies.ToListAsync();
        var transactions = await _context.OrderTransactions.ToListAsync();
        var orderLines = await _context.OrderLines.ToListAsync();
        var reviews = await _context.Reviews.ToListAsync();
        var dishes = await _context.Dishes.ToListAsync();
        var ingredients = await _context.Ingredients.ToListAsync();
        var MenuProposals = await _context.MenuProposals.ToListAsync();
        var addresses = await _context.Addresses.ToListAsync();

        return new FullDatabaseExportDto
        {
            Accounts = accounts,
            Customers = customers,
            Chefs = chefs,
            Dishes = dishes,
            Ingredients = ingredients,
            MenuProposals = MenuProposals,
            OrderLines = orderLines,
            Reviews = reviews,
            Transactions = transactions,
            Individuals = individuals,
            Companies = companies,
            Addresses = addresses,
        };
    }
}

/// <summary>
/// DTO aggregating all entities for export.
/// </summary>
public class FullDatabaseExportDto
{
    public IEnumerable<Account> Accounts { get; set; } = null!;
    public IEnumerable<Individual> Individuals { get; set; } = null!;
    public IEnumerable<Company> Companies { get; set; } = null!;
    public IEnumerable<Address> Addresses { get; set; } = null!;
    public IEnumerable<Customer> Customers { get; set; } = null!;
    public IEnumerable<Chef> Chefs { get; set; } = null!;
    public IEnumerable<Dish> Dishes { get; set; } = null!;
    public IEnumerable<OrderLine> OrderLines { get; set; } = null!;
    public IEnumerable<Review> Reviews { get; set; } = null!;
    public IEnumerable<MenuProposal> MenuProposals { get; set; } = null!;
    public IEnumerable<Ingredient> Ingredients { get; set; } = null!;
    public IEnumerable<OrderTransaction> Transactions { get; set; } = null!;
}

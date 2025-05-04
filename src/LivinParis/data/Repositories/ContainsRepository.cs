using LivInParisRoussilleTeynier.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Data.Repositories;

/// <summary>
/// Provides implementation for contains-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class ContainsRepository(LivInParisContext context)
    : Repository<Contains>(context),
        IContainsRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<Ingredient>> GetIngredientsByDishAsync(Dish dish)
    {
        var query = _context
            .Contains.Where(c => c.Dish == dish)
            .Select(c => c.Ingredient)
            .Where(i => i != null)
            .Cast<Ingredient>();

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Dish>> GetDishesByIngredientAsync(Ingredient ingredient)
    {
        var query = _context
            .Contains.Where(c => c.Ingredient == ingredient)
            .Select(c => c.Dish)
            .Cast<Dish>();

        return await query.ToListAsync();
    }
}

using LivInParisRoussilleTeynier.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Data.Repositories;

public class ContainsRepository(LivInParisContext context)
    : Repository<Contains>(context),
        IContainsRepository
{
    public async Task<IEnumerable<Ingredient>> GetIngredientsByDishAsync(Dish dish)
    {
        var query = _context
            .Contains.Where(c => c.Dish == dish)
            .Select(c => c.Ingredient)
            .ToListAsync();

        return await query;
    }

    public async Task<IEnumerable<Dish>> GetDishesByIngredientAsync(Ingredient ingredient)
    {
        var query = _context
            .Contains.Where(c => c.Ingredient == ingredient)
            .Select(c => c.Dish)
            .ToListAsync();

        return await query;
    }
}

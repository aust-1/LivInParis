using LivInParisRoussilleTeynier.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Data.Repositories;

public class ContainsRepository(LivInParisContext context)
    : Repository<Contains>(context),
        IContainsRepository
{
    public async Task<IEnumerable<Ingredient>> GetIngredientsByDishAsync(Dish dish)
    {
        return await _context
            .Contains.Where(c => c.Dish == dish)
            .Select(c => c.Ingredient)
            .ToListAsync();
    }

    public async Task<IEnumerable<Dish>> GetDishesByIngredientAsync(Ingredient ingredient)
    {
        return await _context
            .Contains.Where(c => c.Ingredient == ingredient)
            .Select(c => c.Dish)
            .ToListAsync();
    }
}

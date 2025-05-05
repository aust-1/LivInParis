using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LivInParisRoussilleTeynier.Infrastructure.Repositories;

/// <summary>
/// Provides implementation for dish-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class DishRepository(LivInParisContext context) : Repository<Dish>(context), IDishRepository
{
    /// <inheritdoc/>
    public async Task<IEnumerable<Dish>> ReadAsync(
        string? dishName = null,
        DishType? dishType = null,
        int? minExpiryTime = null,
        string? cuisineNationality = null,
        int? quantity = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? isVegetarian = null,
        bool? isVegan = null,
        bool? isGlutenFree = null,
        bool? isLactoseFree = null,
        bool? isHalal = null,
        bool? isKosher = null,
        ProductsOrigin? productsOrigin = null
    )
    {
        IQueryable<Dish> query = _dbSet.AsQueryable();

        if (!string.IsNullOrEmpty(dishName))
        {
            var pattern = "%" + string.Join("%", dishName.ToCharArray()) + "%";
            query = query.Where(d => EF.Functions.Like(d.DishName, pattern));
        }

        if (dishType.HasValue)
        {
            query = query.Where(d => d.DishType == dishType.Value);
        }

        if (minExpiryTime.HasValue)
        {
            query = query.Where(d => d.ExpiryTime >= minExpiryTime.Value);
        }

        if (!string.IsNullOrEmpty(cuisineNationality))
        {
            query = query.Where(d => d.CuisineNationality.Contains(cuisineNationality));
        }

        if (quantity.HasValue)
        {
            query = query.Where(d => d.Quantity >= quantity.Value);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(d => d.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(d => d.Price <= maxPrice.Value);
        }

        if (isVegetarian.HasValue)
        {
            query = query.Where(d =>
                d.Contains.All(c => c.Ingredient!.IsVegetarian == isVegetarian.Value)
            );
        }

        if (isVegan.HasValue)
        {
            query = query.Where(d => d.Contains.All(c => c.Ingredient!.IsVegan == isVegan.Value));
        }

        if (isGlutenFree.HasValue)
        {
            query = query.Where(d =>
                d.Contains.All(c => c.Ingredient!.IsGlutenFree == isGlutenFree.Value)
            );
        }

        if (isLactoseFree.HasValue)
        {
            query = query.Where(d =>
                d.Contains.All(c => c.Ingredient!.IsLactoseFree == isLactoseFree.Value)
            );
        }

        if (isHalal.HasValue)
        {
            query = query.Where(d => d.Contains.All(c => c.Ingredient!.IsHalal == isHalal.Value));
        }

        if (isKosher.HasValue)
        {
            query = query.Where(d => d.Contains.All(c => c.Ingredient!.IsKosher == isKosher.Value));
        }

        if (productsOrigin.HasValue)
        {
            query = query.Where(d => d.ProductsOrigin == productsOrigin.Value);
        }

        return await query.ToListAsync();
    }
}

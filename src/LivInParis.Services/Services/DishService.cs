using LivInParisRoussilleTeynier.Domain.Models.Order.Enums;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Services.Services;

/// <inheritdoc/>
/// <summary>
/// Initializes a new instance of <see cref="DishService"/>.
/// </summary>
public class DishService(IDishRepository dishRepository) : IDishService
{
    private readonly IDishRepository _dishRepository = dishRepository;

    /// <inheritdoc/>
    public async Task<IEnumerable<DishDto>> GetAllDishesAsync() =>
        (await _dishRepository.GetAllAsync()).Select(d => new DishDto
        {
            Id = d.DishId,
            Name = d.DishName,
            Type = d.DishType.ToString(),
            ExpiryTime = d.ExpiryTime,
            Cuisine = d.CuisineNationality,
            Quantity = d.Quantity,
            Price = d.Price,
            ProductsOrigin = d.ProductsOrigin.ToString(),
            IsVegetarian = d.Contains.All(c => c.Ingredient!.IsVegetarian),
            IsVegan = d.Contains.All(c => c.Ingredient!.IsVegan),
            IsGlutenFree = d.Contains.All(c => c.Ingredient!.IsGlutenFree),
            IsLactoseFree = d.Contains.All(c => c.Ingredient!.IsLactoseFree),
            IsHalal = d.Contains.All(c => c.Ingredient!.IsHalal),
            IsKosher = d.Contains.All(c => c.Ingredient!.IsKosher),
        });

    /// <inheritdoc/>
    public async Task<DishDto?> GetDishByIdAsync(int dishId)
    {
        var dish = await _dishRepository.GetByIdAsync(dishId);
        if (dish == null)
        {
            return null;
        }

        return new DishDto
        {
            Id = dish.DishId,
            Name = dish.DishName,
            Type = dish.DishType.ToString(),
            ExpiryTime = dish.ExpiryTime,
            Cuisine = dish.CuisineNationality,
            Quantity = dish.Quantity,
            Price = dish.Price,
            ProductsOrigin = dish.ProductsOrigin.ToString(),
            IsVegetarian = dish.Contains.All(c => c.Ingredient!.IsVegetarian),
            IsVegan = dish.Contains.All(c => c.Ingredient!.IsVegan),
            IsGlutenFree = dish.Contains.All(c => c.Ingredient!.IsGlutenFree),
            IsLactoseFree = dish.Contains.All(c => c.Ingredient!.IsLactoseFree),
            IsHalal = dish.Contains.All(c => c.Ingredient!.IsHalal),
            IsKosher = dish.Contains.All(c => c.Ingredient!.IsKosher),
        };
    }


    /// <inheritdoc/>
    public async Task<IEnumerable<DishDto>> SearchDishesAsync(DishSearchCriteriaDto criteria)
    {
        DishType? dishType = null;
        if (!string.IsNullOrWhiteSpace(criteria.Type))
        {
            dishType = Enum.Parse<DishType>(criteria.Type, true);
        }

        ProductsOrigin? origin = null;
        if (!string.IsNullOrWhiteSpace(criteria.ProductsOrigin))
        {
            origin = Enum.Parse<ProductsOrigin>(criteria.ProductsOrigin, true);
        }

        var dishes = await _dishRepository.ReadAsync(
            criteria.Name,
            dishType,
            criteria.MinExpiryTime,
            criteria.Cuisine,
            criteria.Quantity,
            criteria.MinPrice,
            criteria.MaxPrice,
            criteria.IsVegetarian,
            criteria.IsVegan,
            criteria.IsGlutenFree,
            criteria.IsLactoseFree,
            criteria.IsHalal,
            criteria.IsKosher,
            origin
        );

        return dishes.Select(d => new DishDto
        {
            Id = d.DishId,
            Name = d.DishName,
            Type = d.DishType.ToString(),
            ExpiryTime = d.ExpiryTime,
            Cuisine = d.CuisineNationality,
            Quantity = d.Quantity,
            Price = d.Price,
            ProductsOrigin = d.ProductsOrigin.ToString(),
            IsVegetarian = d.Contains.All(c => c.Ingredient!.IsVegetarian),
            IsVegan = d.Contains.All(c => c.Ingredient!.IsVegan),
            IsGlutenFree = d.Contains.All(c => c.Ingredient!.IsGlutenFree),
            IsLactoseFree = d.Contains.All(c => c.Ingredient!.IsLactoseFree),
            IsHalal = d.Contains.All(c => c.Ingredient!.IsHalal),
            IsKosher = d.Contains.All(c => c.Ingredient!.IsKosher),
        });
    }

}

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
    public Task<DishDto?> GetDishByIdAsync(int dishId) =>
        _dishRepository
            .GetByIdAsync(dishId)
            .ContinueWith(t =>
                t.Result is null
                    ? null
                    : new DishDto
                    {
                        Id = t.Result.DishId,
                        Name = t.Result.DishName,
                        Type = t.Result.DishType.ToString(),
                        ExpiryTime = t.Result.ExpiryTime,
                        Cuisine = t.Result.CuisineNationality,
                        Quantity = t.Result.Quantity,
                        Price = t.Result.Price,
                        ProductsOrigin = t.Result.ProductsOrigin.ToString(),
                        IsVegetarian = t.Result.Contains.All(c => c.Ingredient!.IsVegetarian),
                        IsVegan = t.Result.Contains.All(c => c.Ingredient!.IsVegan),
                        IsGlutenFree = t.Result.Contains.All(c => c.Ingredient!.IsGlutenFree),
                        IsLactoseFree = t.Result.Contains.All(c => c.Ingredient!.IsLactoseFree),
                        IsHalal = t.Result.Contains.All(c => c.Ingredient!.IsHalal),
                        IsKosher = t.Result.Contains.All(c => c.Ingredient!.IsKosher),
                    }
            );

    /// <inheritdoc/>
    public async Task<IEnumerable<DishDto>> SearchDishesAsync(DishSearchCriteriaDto criteria)
    {
        var dishes = await _dishRepository.ReadAsync(
            criteria.Name,
            Enum.Parse<DishType>(criteria.Type!),
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
            Enum.Parse<ProductsOrigin>(criteria.ProductsOrigin!)
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

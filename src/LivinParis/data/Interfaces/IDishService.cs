using LivInParisRoussilleTeynier.Models.Order.Enums;

namespace LivInParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods to manage dishes and their attributes.
/// </summary>
public interface IDishRepository : IRepository<Dish>
{
    /// <summary>
    /// Retrieves a list of dishes with optional filters.
    /// </summary>
    /// <param name="limit">The maximum number of rows to return.</param>
    /// <param name="dishName">Filter by dish name.</param>
    /// <param name="dishType">Filter by dish type.</param>
    /// <param name="minExpiryTime">Minimum expiry time filter.</param>
    /// <param name="cuisineNationality">The nationality of the cuisine.</param>
    /// <param name="quantity">The available quantity of the dish.</param>
    /// <param name="minPrice">Minimum price filter.</param>
    /// <param name="maxPrice">Maximum price filter.</param>
    /// <param name="isVegetarian">Filter for vegetarian dishes.</param>
    /// <param name="isVegan">Filter for vegan dishes.</param>
    /// <param name="isGlutenFree">Filter for gluten-free dishes.</param>
    /// <param name="isLactoseFree">Filter for lactose-free dishes.</param>
    /// <param name="isHalal">Filter for halal dishes.</param>
    /// <param name="isKosher">Filter for kosher dishes.</param>
    /// <param name="productOrigin">Filter by product origin.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of dishes.</returns>
    Task<IEnumerable<Dish>> ReadAsync(
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
        ProductsOrigin? productOrigin = null
    );
}

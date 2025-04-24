using MySql.Data.MySqlClient;

namespace LivInParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods to manage dishes and their attributes.
/// </summary>
public interface IDishService
{
    #region CRUD

    /// <summary>
    /// Creates a new dish entry.
    /// </summary>
    /// <param name="dishId">The ID of the dish.</param>
    /// <param name="dishName">The name of the dish.</param>
    /// <param name="dishType">The type of the dish.</param>
    /// <param name="expiryTime">The expiry time of the dish.</param>
    /// <param name="cuisineNationality">The nationality of the cuisine.</param>
    /// <param name="quantity">The available quantity of the dish (number of people).</param>
    /// <param name="dishPrice">The price of the dish.</param>
    /// <param name="photoPath">The path to the dish's photo.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Create(
        int? dishId,
        string dishName,
        DishType dishType,
        int expiryTime,
        string cuisineNationality,
        int quantity,
        decimal dishPrice,
        string photoPath,
        MySqlCommand? command = null
    );

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
    /// <returns>A list of lists of strings representing dish rows.</returns>
    List<List<string>> Read(
        int limit,
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
        ProductOrigin? productOrigin = null,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Updates the name of a dish.
    /// </summary>
    /// <param name="dishId">The ID of the dish.</param>
    /// <param name="dishName">The new name of the dish.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdateName(int dishId, string dishName, MySqlCommand? command = null);

    /// <summary>
    /// Updates the type of a dish.
    /// </summary>
    /// <param name="dishId">The ID of the dish.</param>
    /// <param name="dishType">The new type of the dish.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdateType(int dishId, DishType dishType, MySqlCommand? command = null);

    /// <summary>
    /// Updates the expiry time of a dish.
    /// </summary>
    /// <param name="dishId">The ID of the dish.</param>
    /// <param name="expiryTime">The new expiry time of the dish.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdateExpiryTime(int dishId, int expiryTime, MySqlCommand? command = null);

    /// <summary>
    /// Updates the cuisine nationality of a dish.
    /// </summary>
    /// <param name="dishId">The ID of the dish.</param>
    /// <param name="cuisineNationality">The nationality of the cuisine.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdateCuisineNationality(
        int dishId,
        string cuisineNationality,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Updates the available quantity of a dish.
    /// </summary>
    /// <param name="dishId">The ID of the dish.</param>
    /// <param name="quantity">The new available quantity of the dish.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdateQuantity(int dishId, int quantity, MySqlCommand? command = null);

    /// <summary>
    /// Updates the price of a dish.
    /// </summary>
    /// <param name="dishId">The ID of the dish.</param>
    /// <param name="dishPrice">The new price of the dish.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void UpdatePrice(int dishId, decimal dishPrice, MySqlCommand? command = null);

    /// <summary>
    /// Deletes a dish from the database.
    /// </summary>
    /// <param name="dishId">The ID of the dish to delete.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Delete(int dishId, MySqlCommand? command = null);

    #endregion CRUD

    #region Statistics

    /// <summary>
    /// Retrieves the most frequently ordered dishes.
    /// </summary>
    /// <param name="limit">The maximum number of dishes to return.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    List<List<string>> GetMostOrderedDishes(int limit, MySqlCommand? command = null);

    /// <summary>
    /// Retrieves how many times each cuisine was ordered by a customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="limit">The maximum number of cuisines to return.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    List<List<string>> GetCuisineStatsByCustomer(
        int customerId,
        int limit,
        MySqlCommand? command = null
    );

    #endregion Statistics
}

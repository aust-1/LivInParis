using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods to manage dishes and their attributes.
/// </summary>
public interface IDishService
{
    #region CRUD

    /// <summary>
    /// Creates a new dish entry.
    /// </summary>
    void Create(
        int dishId,
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
    void UpdateName(int dishId, string dishName, MySqlCommand? command = null);

    /// <summary>
    /// Updates the type of a dish.
    /// </summary>
    void UpdateType(int dishId, DishType dishType, MySqlCommand? command = null);

    /// <summary>
    /// Updates the expiry time of a dish.
    /// </summary>
    void UpdateExpiryTime(int dishId, int expiryTime, MySqlCommand? command = null);

    /// <summary>
    /// Updates the cuisine nationality of a dish.
    /// </summary>
    void UpdateCuisineNationality(
        int dishId,
        string cuisineNationality,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Updates the available quantity of a dish.
    /// </summary>
    void UpdateQuantity(int dishId, int quantity, MySqlCommand? command = null);

    /// <summary>
    /// Updates the price of a dish.
    /// </summary>
    void UpdatePrice(int dishId, decimal dishPrice, MySqlCommand? command = null);

    /// <summary>
    /// Deletes a dish from the database.
    /// </summary>
    void Delete(int dishId, MySqlCommand? command = null);

    #endregion CRUD

    #region Statistics

    /// <summary>
    /// Retrieves the most frequently ordered dishes.
    /// </summary>
    List<List<string>> GetMostOrderedDishes(int limit, MySqlCommand? command = null);

    /// <summary>
    /// Retrieves how many times each cuisine was ordered by a customer.
    /// </summary>
    List<List<string>> GetCuisineStatsByCustomer(
        int customerId,
        int limit,
        MySqlCommand? command = null
    );

    #endregion Statistics
}

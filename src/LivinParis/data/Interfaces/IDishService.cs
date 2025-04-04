using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface IDishService
{
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

    List<List<string>> GetMostOrderedDishes(int limit, MySqlCommand? command = null);

    List<List<string>> GetCuisineStatsByCustomer(
        int limit,
        int customerId,
        MySqlCommand? command = null
    );

    void UpdateName(int dishId, string dishName, MySqlCommand? command = null);

    void UpdateType(int dishId, DishType dishType, MySqlCommand? command = null);

    void UpdateExpiryTime(int dishId, int expiryTime, MySqlCommand? command = null);

    void UpdateCuisineNationality(
        int dishId,
        string cuisineNationality,
        MySqlCommand? command = null
    );

    void UpdateQuantity(int dishId, int quantity, MySqlCommand? command = null);

    void UpdatePrice(int dishId, decimal dishPrice, MySqlCommand? command = null);

    void Delete(int dishId, MySqlCommand? command = null);
}

//HACK: combien de fois un plat proposé

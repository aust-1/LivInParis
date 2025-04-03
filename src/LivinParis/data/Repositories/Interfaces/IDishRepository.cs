using MySql.Data.MySqlClient;

namespace LivinParis.Data;

public interface IDish
{
    void CreateDish(
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

    Dictionary<int, List<string>> GetDishes(
        int limit,
        int? chefId = null,
        int? customerId = null,
        string? dishName = null,
        DishType? dishType = null,
        int? expiryTime = null,
        string? cuisineNationality = null,
        int? quantity = null,
        decimal? priceHigherThan = null,
        decimal? priceBelow = null,
        bool? isVegetarian = null,
        bool? isVegan = null,
        bool? isGlutenFree = null,
        bool? isLactoseFree = null,
        bool? isHalal = null,
        bool? isKosher = null,
        ProductionOrigin? productionOrigin = null,
        MySqlCommand? command = null
    );

    void UpdateDishName(int dishId, string dishName, MySqlCommand? command = null);

    void UpdateDishType(int dishId, DishType dishType, MySqlCommand? command = null);

    void UpdateDishExpiryTime(int dishId, int expiryTime, MySqlCommand? command = null);

    void UpdateDishCuisineNationality(
        int dishId,
        string cuisineNationality,
        MySqlCommand? command = null
    );

    void UpdateDishQuantity(int dishId, int quantity, MySqlCommand? command = null);

    void UpdateDishPrice(int dishId, decimal dishPrice, MySqlCommand? command = null);

    void DeleteDish(int dishId, MySqlCommand? command = null);
}

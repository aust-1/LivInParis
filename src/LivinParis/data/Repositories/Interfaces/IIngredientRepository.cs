using MySql.Data.MySqlClient;

namespace LivinParis.Data;

public interface IIngredient
{
    void CreateIngredient(
        int ingredientId,
        string name,
        bool isVegetarian,
        bool isVegan,
        bool isGlutenFree,
        bool isLactoseFree,
        bool isHalal,
        bool isKosher,
        ProductionOrigin productionOrigin,
        MySqlCommand? command = null
    );

    Dictionary<int, List<string>> GetIngredients(
        int limit,
        string? name = null,
        bool? isVegetarian = null,
        bool? isVegan = null,
        bool? isGlutenFree = null,
        bool? isLactoseFree = null,
        bool? isHalal = null,
        bool? isKosher = null,
        ProductionOrigin? productionOrigin = null,
        MySqlCommand? command = null
    );

    void UpdateIngredientRestrictions(
        int ingredientId,
        bool? isVegetarian = null,
        bool? isVegan = null,
        bool? isGlutenFree = null,
        bool? isLactoseFree = null,
        bool? isHalal = null,
        bool? isKosher = null,
        MySqlCommand? command = null
    );

    void DeleteIngredient(int ingredientId, MySqlCommand? command = null);
}

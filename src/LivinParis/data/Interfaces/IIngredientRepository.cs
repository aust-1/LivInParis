using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

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
        ProductOrigin productOrigin,
        MySqlCommand? command = null
    );

    List<List<string>> GetIngredients(
        int limit,
        string? name = null,
        bool? isVegetarian = null,
        bool? isVegan = null,
        bool? isGlutenFree = null,
        bool? isLactoseFree = null,
        bool? isHalal = null,
        bool? isKosher = null,
        ProductOrigin? productOrigin = null,
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

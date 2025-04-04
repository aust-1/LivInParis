using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface IIngredientService
{
    void Create(
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

    List<List<string>> Read(
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

    void UpdateRestrictions(
        int ingredientId,
        bool? isVegetarian = null,
        bool? isVegan = null,
        bool? isGlutenFree = null,
        bool? isLactoseFree = null,
        bool? isHalal = null,
        bool? isKosher = null,
        MySqlCommand? command = null
    );

    void Delete(int ingredientId, MySqlCommand? command = null);
}

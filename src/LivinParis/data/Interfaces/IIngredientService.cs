using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface IIngredientService
{
    #region CRUD

    /// <summary>
    /// Creates a new ingredient entry in the database.
    /// </summary>
    /// <param name="ingredientId">The ID of the ingredient.</param>
    /// <param name="name">The name of the ingredient.</param>
    /// <param name="isVegetarian">Indicates if the ingredient is vegetarian.</param>
    /// <param name="isVegan">Indicates if the ingredient is vegan.</param>
    /// <param name="isGlutenFree">Indicates if the ingredient is gluten-free.</param>
    /// <param name="isLactoseFree">Indicates if the ingredient is lactose-free.</param>
    /// <param name="isHalal">Indicates if the ingredient is halal.</param>
    /// <param name="isKosher">Indicates if the ingredient is kosher.</param>
    /// <param name="productOrigin">The origin of the product.</param>
    /// /// <param name="command">An optional MySQL command to execute within a transaction.</param>
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

    /// <summary>
    /// Retrieves a list of ingredients from the database with optional filters.
    /// </summary>
    /// <param name="limit">The maximum number of rows to return.</param>
    /// <param name="name">Filter by ingredient name.</param>
    /// <param name="isVegetarian">Filter for vegetarian ingredients.</param>
    /// <param name="isVegan">Filter for vegan ingredients.</param>
    /// <param name="isGlutenFree">Filter for gluten-free ingredients.</param>
    /// <param name="isLactoseFree">Filter for lactose-free ingredients.</param>
    /// <param name="isHalal">Filter for halal ingredients.</param>
    /// <param name="isKosher">Filter for kosher ingredients.</param>
    /// <param name="productOrigin">Filter by product origin.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of lists of strings representing ingredient rows.</returns>
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

    /// <summary>
    /// Updates the restrictions of an ingredient in the database.
    /// </summary>
    /// <param name="ingredientId">The ID of the ingredient to update.</param>
    /// <param name="isVegetarian">Indicates if the ingredient is vegetarian.</param>
    /// <param name="isVegan">Indicates if the ingredient is vegan.</param>
    /// <param name="isGlutenFree">Indicates if the ingredient is gluten-free.</param>
    /// <param name="isLactoseFree">Indicates if the ingredient is lactose-free.</param>
    /// <param name="isHalal">Indicates if the ingredient is halal.</param>
    /// <param name="isKosher">Indicates if the ingredient is kosher.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
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

    /// <summary>
    /// Deletes an ingredient from the database.
    /// </summary>
    /// <param name="ingredientId">The ID of the ingredient to delete.</param>
    /// /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    void Delete(int ingredientId, MySqlCommand? command = null);

    #endregion CRUD
}

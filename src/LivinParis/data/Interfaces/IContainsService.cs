namespace LivInParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods to manage the association between dishes and ingredients.
/// </summary>
public interface IContainsService
{
    /// <summary>
    /// Gets a list of ingredient IDs for a given dish.
    /// </summary>
    /// <param name="limit">The maximum number of ingredients to return.</param>
    /// <param name="dishId">The ID of the dish.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    List<int> GetIngredientsByDishId(int dishId);

    /// <summary>
    /// Gets a list of dish IDs that use a given ingredient.
    /// </summary>
    /// <param name="limit">The maximum number of dishes to return.</param>
    /// <param name="ingredientId">The ID of the ingredient.</param>
    /// <param name="command">An optional MySQL command to execute within a transaction.</param>
    /// <returns>A list of dish IDs that use the specified ingredient.</returns>
    List<int> GetDishesByIngredientId(int ingredientId);
}

//HACK: c'est de la d là hein

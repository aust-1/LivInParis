namespace LivInParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods to manage the association between dishes and ingredients.
/// </summary>
public interface IContainsRepository : IRepository<Contains>
{
    /// <summary>
    /// Gets a list of ingredient for a given dish.
    /// </summary>
    /// <param name="dish">The dish.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of ingredient for the specified dish.</returns>
    Task<IEnumerable<Ingredient>> GetIngredientsByDishAsync(Dish dish);

    /// <summary>
    /// Gets a list of dish that use a given ingredient.
    /// </summary>
    /// <param name="ingredient">The ingredient.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of dishes that use the specified ingredient.</returns>
    Task<IEnumerable<Dish>> GetDishesByIngredientAsync(Ingredient ingredient);
}

//TODO: to implement

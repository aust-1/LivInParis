using LivinParis.Models.Order;

namespace LivinParis.Repositories;

/// <summary>
/// Defines operations for managing <see cref="Ingredient"/> entities.
/// </summary>
public interface IIngredientRepository
{
    /// <summary>
    /// Retrieves an ingredient by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the ingredient.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the ingredient if found; otherwise, null.</returns>
    Task<Ingredient> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves all ingredients.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of all ingredients.</returns>
    Task<IEnumerable<Ingredient>> GetAllAsync();

    /// <summary>
    /// Adds a new ingredient to the data store.
    /// </summary>
    /// <param name="ingredient">The ingredient to add.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the newly created ingredient.</returns>
    Task<Ingredient> CreateAsync(Ingredient ingredient);

    /// <summary>
    /// Updates an existing ingredient in the data store.
    /// </summary>
    /// <param name="ingredient">The ingredient with updated data.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated ingredient.</returns>
    Task<Ingredient> UpdateAsync(Ingredient ingredient);

    /// <summary>
    /// Deletes an ingredient by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the ingredient to delete.</param>
    /// <returns>A task representing the asynchronous operation. The task result indicates whether the operation succeeded.</returns>
    Task<bool> DeleteAsync(int id);
}

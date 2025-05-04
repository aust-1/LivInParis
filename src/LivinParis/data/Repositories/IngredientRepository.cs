using LivInParisRoussilleTeynier.Data.Interfaces;

namespace LivInParisRoussilleTeynier.Data.Repositories;

/// <summary>
/// Provides implementation for ingredient-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class IngredientRepository(LivInParisContext context)
    : Repository<Ingredient>(context),
        IIngredientRepository { }

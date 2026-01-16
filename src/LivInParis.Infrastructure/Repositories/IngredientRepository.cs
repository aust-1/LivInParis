using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Data;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;

namespace LivInParisRoussilleTeynier.Infrastructure.Repositories;

/// <summary>
/// Provides implementation for ingredient-related operations.
/// </summary>
/// <param name="context">The database context.</param>
public class IngredientRepository(LivInParisContext context)
    : Repository<Ingredient>(context),
        IIngredientRepository
{ }

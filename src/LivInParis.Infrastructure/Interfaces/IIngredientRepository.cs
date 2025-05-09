using LivInParisRoussilleTeynier.Domain.Models.Order;

namespace LivInParisRoussilleTeynier.Infrastructure.Interfaces;

/// <summary>
/// Provides methods for managing ingredient entities in the database.
/// </summary>
public interface IIngredientRepository : IRepository<Ingredient> { }

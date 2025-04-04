using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods to manage the association between dishes and ingredients.
/// </summary>
public interface IContainsService
{
    #region CRUD

    /// <summary>
    /// Creates a relation between an ingredient and a dish.
    /// </summary>
    void Create(int ingredientId, int dishId, MySqlCommand? command = null);

    /// <summary>
    /// Gets a list of ingredient IDs for a given dish.
    /// </summary>
    List<int> GetIngredientsByDishId(int limit, int dishId, MySqlCommand? command = null);

    /// <summary>
    /// Gets a list of dish IDs that use a given ingredient.
    /// </summary>
    List<int> GetDishesByIngredientId(int limit, int ingredientId, MySqlCommand? command = null);

    /// <summary>
    /// Deletes the association between a dish and an ingredient.
    /// </summary>
    void Delete(int ingredientId, int dishId, MySqlCommand? command = null);

    #endregion CRUD
}

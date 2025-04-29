using LivInParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivInParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for managing the relationship between dishes and ingredients.
/// </summary>
[ConnectionControl]
public class ContainsService : IContainsService
{
    #region CRUD

    /// <inheritdoc/>
    public virtual void Create(int? ingredientId, int dishId, MySqlCommand? command = null)
    {
        command!.CommandText =
            "INSERT INTO Contains (ingredient_id, dish_id) VALUES (@ingredient, @dish)";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@ingredient", ingredientId);
        command.Parameters.AddWithValue("@dish", dishId);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual List<int> GetIngredientsByDishId(
        int limit,
        int dishId,
        MySqlCommand? command = null
    )
    {
        List<int> ingredients = [];

        command!.CommandText =
            @"
                SELECT ingredient_id
                FROM Contains
                WHERE dish_id = @dish
                LIMIT @limit";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@dish", dishId);
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            if (!reader.IsDBNull(0))
            {
                ingredients.Add(reader.GetInt32(0));
            }
        }

        return ingredients;
    }

    /// <inheritdoc/>
    public virtual List<int> GetDishesByIngredientId(
        int limit,
        int ingredientId,
        MySqlCommand? command = null
    )
    {
        List<int> dishes = [];

        command!.CommandText =
            @"
                SELECT dish_id
                FROM Contains
                WHERE ingredient_id = @ingredient
                LIMIT @limit";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@ingredient", ingredientId);
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            if (!reader.IsDBNull(0))
            {
                dishes.Add(reader.GetInt32(0));
            }
        }

        return dishes;
    }

    /// <inheritdoc/>
    public virtual void Delete(int ingredientId, int dishId, MySqlCommand? command = null)
    {
        command!.CommandText =
            @"
                DELETE FROM Contains
                WHERE ingredient_id = @ingredient AND dish_id = @dish";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@ingredient", ingredientId);
        command.Parameters.AddWithValue("@dish", dishId);
        command.ExecuteNonQuery();
    }

    #endregion CRUD
}

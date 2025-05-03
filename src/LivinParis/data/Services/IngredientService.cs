using System.Text;
using LivInParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivInParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for ingredient-related operations in the database.
/// </summary>
[ConnectionControl]
public class IngredientService : IIngredientService
{
    #region CRUD

    /// <inheritdoc/>
    public virtual void Create(
        int? ingredientId,
        string name,
        bool isVegetarian,
        bool isVegan,
        bool isGlutenFree,
        bool isLactoseFree,
        bool isHalal,
        bool isKosher,
        ProductsOrigin productsOrigin,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                INSERT INTO Ingredient (
                    ingredient_id, ingredient_name,
                    is_vegetarian, is_vegan, is_gluten_free,
                    is_lactose_free, is_halal, is_kosher, product_origin
                )
                VALUES (
                    @id, @name,
                    @vegetarian, @vegan, @glutenFree,
                    @lactoseFree, @halal, @kosher, @origin
                )";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", ingredientId);
        command.Parameters.AddWithValue("@name", name);
        command.Parameters.AddWithValue("@vegetarian", isVegetarian);
        command.Parameters.AddWithValue("@vegan", isVegan);
        command.Parameters.AddWithValue("@glutenFree", isGlutenFree);
        command.Parameters.AddWithValue("@lactoseFree", isLactoseFree);
        command.Parameters.AddWithValue("@halal", isHalal);
        command.Parameters.AddWithValue("@kosher", isKosher);
        command.Parameters.AddWithValue("@origin", productsOrigin.ToString());
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual List<List<string>> Read(
        int limit,
        string? name = null,
        bool? isVegetarian = null,
        bool? isVegan = null,
        bool? isGlutenFree = null,
        bool? isLactoseFree = null,
        bool? isHalal = null,
        bool? isKosher = null,
        ProductsOrigin? productsOrigin = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];
        List<string> conditions = [];
        StringBuilder query = new("SELECT * FROM Ingredient");

        if (name is not null)
        {
            conditions.Add("ingredient_name LIKE @name");
        }

        if (isVegetarian is not null)
        {
            conditions.Add("is_vegetarian = @vegetarian");
        }

        if (isVegan is not null)
        {
            conditions.Add("is_vegan = @vegan");
        }

        if (isGlutenFree is not null)
        {
            conditions.Add("is_gluten_free = @glutenFree");
        }

        if (isLactoseFree is not null)
        {
            conditions.Add("is_lactose_free = @lactoseFree");
        }

        if (isHalal is not null)
        {
            conditions.Add("is_halal = @halal");
        }

        if (isKosher is not null)
        {
            conditions.Add("is_kosher = @kosher");
        }

        if (productsOrigin is not null)
        {
            conditions.Add("product_origin = @origin");
        }

        if (conditions.Count > 0)
        {
            query.Append(" WHERE " + string.Join(" AND ", conditions));
        }

        query.Append(" LIMIT @limit");

        command!.CommandText = query.ToString();
        command.Parameters.Clear();

        if (name is not null)
        {
            command.Parameters.AddWithValue("@name", $"%{name}%");
        }
        if (isVegetarian is not null)
        {
            command.Parameters.AddWithValue("@vegetarian", isVegetarian);
        }
        if (isVegan is not null)
        {
            command.Parameters.AddWithValue("@vegan", isVegan);
        }
        if (isGlutenFree is not null)
        {
            command.Parameters.AddWithValue("@glutenFree", isGlutenFree);
        }
        if (isLactoseFree is not null)
        {
            command.Parameters.AddWithValue("@lactoseFree", isLactoseFree);
        }
        if (isHalal is not null)
        {
            command.Parameters.AddWithValue("@halal", isHalal);
        }
        if (isKosher is not null)
        {
            command.Parameters.AddWithValue("@kosher", isKosher);
        }
        if (productsOrigin is not null)
        {
            command.Parameters.AddWithValue("@origin", productsOrigin.ToString());
        }

        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            List<string> row = [];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string value = reader.IsDBNull(i)
                    ? string.Empty
                    : reader.GetValue(i).ToString() ?? string.Empty;
                row.Add(value);
            }
            results.Add(row);
        }

        return results;
    }

    /// <inheritdoc/>
    public virtual void UpdateRestrictions(
        int ingredientId,
        bool? isVegetarian = null,
        bool? isVegan = null,
        bool? isGlutenFree = null,
        bool? isLactoseFree = null,
        bool? isHalal = null,
        bool? isKosher = null,
        MySqlCommand? command = null
    )
    {
        List<string> updates = [];

        if (isVegetarian is not null)
        {
            updates.Add("is_vegetarian = @vegetarian");
        }
        if (isVegan is not null)
        {
            updates.Add("is_vegan = @vegan");
        }
        if (isGlutenFree is not null)
        {
            updates.Add("is_gluten_free = @glutenFree");
        }
        if (isLactoseFree is not null)
        {
            updates.Add("is_lactose_free = @lactoseFree");
        }
        if (isHalal is not null)
        {
            updates.Add("is_halal = @halal");
        }
        if (isKosher is not null)
        {
            updates.Add("is_kosher = @kosher");
        }

        if (updates.Count == 0)
        {
            return;
        }

        StringBuilder query = new();
        query.Append("UPDATE Ingredient SET ");
        query.Append(string.Join(", ", updates));
        query.Append(" WHERE ingredient_id = @id");

        command!.CommandText = query.ToString();
        command.Parameters.Clear();

        if (isVegetarian is not null)
        {
            command.Parameters.AddWithValue("@vegetarian", isVegetarian);
        }
        if (isVegan is not null)
        {
            command.Parameters.AddWithValue("@vegan", isVegan);
        }
        if (isGlutenFree is not null)
        {
            command.Parameters.AddWithValue("@glutenFree", isGlutenFree);
        }
        if (isLactoseFree is not null)
        {
            command.Parameters.AddWithValue("@lactoseFree", isLactoseFree);
        }
        if (isHalal is not null)
        {
            command.Parameters.AddWithValue("@halal", isHalal);
        }
        if (isKosher is not null)
        {
            command.Parameters.AddWithValue("@kosher", isKosher);
        }

        command.Parameters.AddWithValue("@id", ingredientId);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void Delete(int ingredientId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM Ingredient WHERE ingredient_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", ingredientId);
        command.ExecuteNonQuery();
    }

    #endregion CRUD
}

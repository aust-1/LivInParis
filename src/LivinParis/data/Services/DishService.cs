using System.Text;
using LivinParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for dish-related operations in the database.
/// </summary>
[ConnectionControl]
public class DishService : IDishService
{
    #region CRUD

    /// <inheritdoc/>
    public virtual void Create(
        int dishId,
        string dishName,
        DishType dishType,
        int expiryTime,
        string cuisineNationality,
        int quantity,
        decimal dishPrice,
        string photoPath,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                INSERT INTO Dish (
                    dish_id, dish_name, dish_type, expiry_time, cuisine_nationality,
                    quantity, price, photo_path
                )
                VALUES (
                    @id, @name, @type, @expiry, @nationality,
                    @quantity, @price, @photo)";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", dishId);
        command.Parameters.AddWithValue("@name", dishName);
        command.Parameters.AddWithValue("@type", dishType.ToString());
        command.Parameters.AddWithValue("@expiry", expiryTime);
        command.Parameters.AddWithValue("@nationality", cuisineNationality);
        command.Parameters.AddWithValue("@quantity", quantity);
        command.Parameters.AddWithValue("@price", dishPrice);
        command.Parameters.AddWithValue("@photo", photoPath);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual List<List<string>> Read(
        int limit,
        string? dishName = null,
        DishType? dishType = null,
        int? minExpiryTime = null,
        string? cuisineNationality = null,
        int? quantity = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        bool? isVegetarian = null,
        bool? isVegan = null,
        bool? isGlutenFree = null,
        bool? isLactoseFree = null,
        bool? isHalal = null,
        bool? isKosher = null,
        ProductOrigin? productOrigin = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];
        List<string> conditions = [];
        StringBuilder query = new("SELECT DISTINCT d.* FROM Dish d");

        conditions.Add("1=1");

        if (dishName is not null)
        {
            conditions.Add("d.dish_name LIKE @name");
        }

        if (dishType is not null)
        {
            conditions.Add("d.dish_type = @type");
        }

        if (minExpiryTime is not null)
        {
            conditions.Add("d.expiry_time >= @expiry");
        }

        if (cuisineNationality is not null)
        {
            conditions.Add("d.cuisine_nationality = @nationality");
        }

        if (quantity is not null)
        {
            conditions.Add("d.quantity >= @quantity");
        }

        if (minPrice is not null)
        {
            conditions.Add("d.price >= @minPrice");
        }

        if (maxPrice is not null)
        {
            conditions.Add("d.price <= @maxPrice");
        }

        List<string> restrictionConditions = [];

        if (isVegetarian == true)
        {
            restrictionConditions.Add("i.is_vegetarian = FALSE");
        }

        if (isVegan == true)
        {
            restrictionConditions.Add("i.is_vegan = FALSE");
        }

        if (isGlutenFree == true)
        {
            restrictionConditions.Add("i.is_gluten_free = FALSE");
        }

        if (isLactoseFree == true)
        {
            restrictionConditions.Add("i.is_lactose_free = FALSE");
        }

        if (isHalal == true)
        {
            restrictionConditions.Add("i.is_halal = FALSE");
        }

        if (isKosher == true)
        {
            restrictionConditions.Add("i.is_kosher = FALSE");
        }

        if (productOrigin is not null)
        {
            restrictionConditions.Add("i.product_origin != @origin");
        }

        if (restrictionConditions.Count > 0)
        {
            conditions.Add(
                $@"
            NOT EXISTS (
                SELECT 1
                FROM Contains c
                JOIN Ingredient i ON c.ingredient_id = i.ingredient_id
                WHERE c.dish_id = d.dish_id
                AND ({string.Join(" OR ", restrictionConditions)})
            )"
            );
        }

        query.Append(" WHERE " + string.Join(" AND ", conditions));
        query.Append(" LIMIT @limit");

        command!.CommandText = query.ToString();
        command.Parameters.Clear();

        if (dishName is not null)
        {
            command.Parameters.AddWithValue("@name", $"%{dishName}%");
        }
        if (dishType is not null)
        {
            command.Parameters.AddWithValue("@type", dishType.ToString());
        }
        if (minExpiryTime is not null)
        {
            command.Parameters.AddWithValue("@expiry", minExpiryTime);
        }
        if (cuisineNationality is not null)
        {
            command.Parameters.AddWithValue("@nationality", cuisineNationality);
        }
        if (quantity is not null)
        {
            command.Parameters.AddWithValue("@quantity", quantity);
        }
        if (minPrice is not null)
        {
            command.Parameters.AddWithValue("@minPrice", minPrice);
        }
        if (maxPrice is not null)
        {
            command.Parameters.AddWithValue("@maxPrice", maxPrice);
        }
        if (productOrigin is not null)
        {
            command.Parameters.AddWithValue("@origin", productOrigin.ToString());
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
    public virtual void UpdateName(int dishId, string dishName, MySqlCommand? command = null)
    {
        command!.CommandText = "UPDATE Dish SET dish_name = @name WHERE dish_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", dishId);
        command.Parameters.AddWithValue("@name", dishName);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void UpdateType(int dishId, DishType dishType, MySqlCommand? command = null)
    {
        command!.CommandText = "UPDATE Dish SET dish_type = @type WHERE dish_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", dishId);
        command.Parameters.AddWithValue("@type", dishType.ToString());
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void UpdateExpiryTime(int dishId, int expiryTime, MySqlCommand? command = null)
    {
        command!.CommandText = "UPDATE Dish SET expiry_time = @expiry WHERE dish_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", dishId);
        command.Parameters.AddWithValue("@expiry", expiryTime);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void UpdateCuisineNationality(
        int dishId,
        string cuisineNationality,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            "UPDATE Dish SET cuisine_nationality = @nationality WHERE dish_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", dishId);
        command.Parameters.AddWithValue("@nationality", cuisineNationality);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void UpdateQuantity(int dishId, int quantity, MySqlCommand? command = null)
    {
        command!.CommandText = "UPDATE Dish SET quantity = @quantity WHERE dish_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", dishId);
        command.Parameters.AddWithValue("@quantity", quantity);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void UpdatePrice(int dishId, decimal dishPrice, MySqlCommand? command = null)
    {
        command!.CommandText = "UPDATE Dish SET price = @price WHERE dish_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", dishId);
        command.Parameters.AddWithValue("@price", dishPrice);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void Delete(int dishId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM Dish WHERE dish_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", dishId);
        command.ExecuteNonQuery();
    }

    #endregion CRUD

    #region Statistics

    /// <inheritdoc/>
    public virtual List<List<string>> GetMostOrderedDishes(int limit, MySqlCommand? command = null)
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
        SELECT d.dish_name, COUNT(*) AS order_count
        FROM OrderLine o
        JOIN MenuProposal mp ON o.account_id = mp.account_id
        JOIN Dish d ON mp.dish_id = d.dish_id
        WHERE mp.proposal_date = DATE(o.order_line_datetime)
        GROUP BY d.dish_name
        ORDER BY order_count DESC
        LIMIT @limit";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            results.Add(
                [reader[0].ToString() ?? string.Empty, reader[1].ToString() ?? string.Empty]
            );
        }

        return results;
    }

    /// <inheritdoc/>
    public virtual List<List<string>> GetCuisineStatsByCustomer(
        int limit,
        int customerId,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
        SELECT d.cuisine_nationality, COUNT(*) AS count
        FROM OrderLine ol
        JOIN OrderTransaction ot ON ol.transaction_id = ot.transaction_id
        JOIN MenuProposal mp ON mp.account_id = ol.account_id AND mp.proposal_date = DATE(ol.order_line_datetime)
        JOIN Dish d ON mp.dish_id = d.dish_id
        WHERE t.account_id = @customerId
        GROUP BY d.cuisine_nationality
        ORDER BY count DESC
        LIMIT @limit";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@customerId", customerId);
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            results.Add(
                [reader[0].ToString() ?? string.Empty, reader[1].ToString() ?? string.Empty]
            );
        }

        return results;
    }

    #endregion Statistics
}

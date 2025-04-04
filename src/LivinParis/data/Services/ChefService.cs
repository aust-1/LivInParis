using System.Text;
using LivinParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for chef-related operations in the database.
/// </summary>
[ConnectionControl]
public class ChefService : IChefService
{
    #region CRUD

    /// <inheritdoc/>
    public virtual void Create(
        int? chefAccountId,
        decimal chefRating,
        bool eatsOnSite,
        bool chefIsBanned,
        int addressId,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                INSERT INTO Chef (account_id, rating, eats_on_site, is_banned, address_id)
                VALUES (@id, @rating, @eats, @banned, @address)";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", chefAccountId);
        command.Parameters.AddWithValue("@rating", chefRating);
        command.Parameters.AddWithValue("@eats", eatsOnSite);
        command.Parameters.AddWithValue("@banned", chefIsBanned);
        command.Parameters.AddWithValue("@address", addressId);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual List<List<string>> Read(
        int limit,
        decimal? minRating = null,
        decimal? maxRating = null,
        bool? eatsOnSite = null,
        bool? chefIsBanned = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> chefs = [];
        List<string> conditions = [];
        StringBuilder query = new("SELECT * FROM Chef");

        if (minRating is not null)
        {
            conditions.Add("rating >= @minRating");
        }

        if (maxRating is not null)
        {
            conditions.Add("rating <= @maxRating");
        }

        if (eatsOnSite is not null)
        {
            conditions.Add("eats_on_site = @eatsOnSite");
        }

        if (chefIsBanned is not null)
        {
            conditions.Add("is_banned = @isBanned");
        }

        if (conditions.Count > 0)
        {
            query.Append(" WHERE ");
            query.Append(string.Join(" AND ", conditions));
        }

        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            query.Append(" ORDER BY ");
            query.Append(orderBy);
            query.Append(orderDirection == true ? " ASC" : " DESC");
        }

        query.Append(" LIMIT @limit");

        command!.CommandText = query.ToString();
        command.Parameters.Clear();

        if (minRating is not null)
        {
            command.Parameters.AddWithValue("@minRating", minRating);
        }

        if (maxRating is not null)
        {
            command.Parameters.AddWithValue("@maxRating", maxRating);
        }

        if (eatsOnSite is not null)
        {
            command.Parameters.AddWithValue("@eatsOnSite", eatsOnSite);
        }

        if (chefIsBanned is not null)
        {
            command.Parameters.AddWithValue("@isBanned", chefIsBanned);
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
            chefs.Add(row);
        }

        return chefs;
    }

    /// <inheritdoc/>
    public virtual void UpdateRating(
        int chefAccountId,
        decimal chefRating,
        MySqlCommand? command = null
    )
    {
        command!.CommandText = "UPDATE Chef SET rating = @rating WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", chefAccountId);
        command.Parameters.AddWithValue("@rating", chefRating);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void UpdateEatsOnSite(
        int chefAccountId,
        bool eatsOnSite,
        MySqlCommand? command = null
    )
    {
        command!.CommandText = "UPDATE Chef SET eats_on_site = @eats WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", chefAccountId);
        command.Parameters.AddWithValue("@eats", eatsOnSite);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void UpdateIsBanned(
        int chefAccountId,
        bool chefIsBanned,
        MySqlCommand? command = null
    )
    {
        command!.CommandText = "UPDATE Chef SET is_banned = @banned WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", chefAccountId);
        command.Parameters.AddWithValue("@banned", chefIsBanned);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void Delete(int chefAccountId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM Chef WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", chefAccountId);
        command.ExecuteNonQuery();
    }

    #endregion CRUD

    #region Statistics

    /// <inheritdoc/>
    public virtual List<List<string>> GetCustomersServedByChef(
        int limit,
        int chefId,
        DateTime? from = null,
        DateTime? to = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        StringBuilder query = new(
            @"
        SELECT DISTINCT ot.account_id
        FROM OrderLine ol
        JOIN OrderTransaction ot ON ol.transaction_id = ot.transaction_id
        WHERE ol.account_id = @chefId
    "
        );

        if (from is not null)
        {
            query.Append(" AND ol.order_line_datetime >= @from");
        }
        if (to is not null)
        {
            query.Append(" AND ol.order_line_datetime <= @to");
        }

        query.Append(" LIMIT @limit");

        command!.CommandText = query.ToString();
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@chefId", chefId);
        command.Parameters.AddWithValue("@limit", limit);
        if (from is not null)
        {
            command.Parameters.AddWithValue("@from", from);
        }
        if (to is not null)
        {
            command.Parameters.AddWithValue("@to", to);
        }

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            results.Add([reader[0]?.ToString() ?? string.Empty]);
        }

        return results;
    }

    /// <inheritdoc/>
    public virtual List<string> GetTodayDishByChef(int chefId, MySqlCommand? command = null)
    {
        List<string> result = [];

        command!.CommandText =
            @"
        SELECT d.*
        FROM MenuProposal mp
        JOIN Dish d ON mp.dish_id = d.dish_id
        WHERE mp.account_id = @chefId AND mp.proposal_date = CURDATE()";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@chefId", chefId);

        using var reader = command.ExecuteReader();

        for (int i = 0; i < reader.FieldCount; i++)
        {
            result.Add(reader[i]?.ToString() ?? string.Empty);
        }

        return result;
    }

    /// <inheritdoc/>
    public virtual List<List<string>> GetDeliveryCountByChef(
        int limit,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
        SELECT account_id, COUNT(*) AS count
        FROM OrderLine
        WHERE order_line_status = 'delivered'
        GROUP BY account_id
        ORDER BY count DESC
        LIMIT @limit";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            results.Add(
                [reader[0]?.ToString() ?? string.Empty, reader[1]?.ToString() ?? string.Empty]
            );
        }

        return results;
    }

    #endregion Statistics
}

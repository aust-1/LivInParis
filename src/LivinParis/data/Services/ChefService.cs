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
    /// <inheritdoc/>
    public virtual void Create(
        int chefAccountId,
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
}

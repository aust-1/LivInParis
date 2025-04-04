using System.Text;
using LivinParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for customer-related operations in the database.
/// </summary>
[ConnectionControl]
public class CustomerService : ICustomerService
{
    #region CRUD

    /// <inheritdoc/>
    public virtual void Create(
        int customerAccountId,
        decimal customerRating,
        LoyaltyRank loyaltyRank,
        bool customerIsBanned,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                INSERT INTO Customer (account_id, rating, loyalty_rank, is_banned)
                VALUES (@id, @rating, @rank, @banned)";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", customerAccountId);
        command.Parameters.AddWithValue("@rating", customerRating);
        command.Parameters.AddWithValue("@rank", loyaltyRank.ToString());
        command.Parameters.AddWithValue("@banned", customerIsBanned);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual List<List<string>> Read(
        int limit,
        decimal? minRating = null,
        decimal? maxRating = null,
        LoyaltyRank? loyaltyRank = null,
        bool? customerIsBanned = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> customers = [];
        List<string> conditions = [];
        StringBuilder query = new("SELECT * FROM Customer");

        if (minRating is not null)
        {
            conditions.Add("rating >= @minRating");
        }

        if (maxRating is not null)
        {
            conditions.Add("rating <= @maxRating");
        }

        if (loyaltyRank is not null)
        {
            conditions.Add("loyalty_rank = @rank");
        }

        if (customerIsBanned is not null)
        {
            conditions.Add("is_banned = @banned");
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

        if (loyaltyRank is not null)
        {
            command.Parameters.AddWithValue("@rank", loyaltyRank.ToString());
        }

        if (customerIsBanned is not null)
        {
            command.Parameters.AddWithValue("@banned", customerIsBanned);
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
            customers.Add(row);
        }

        return customers;
    }

    /// <inheritdoc/>
    public virtual void UpdateRating(
        int customerAccountId,
        decimal customerRating,
        MySqlCommand? command = null
    )
    {
        command!.CommandText = "UPDATE Customer SET rating = @rating WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", customerAccountId);
        command.Parameters.AddWithValue("@rating", customerRating);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void UpdateLoyaltyRank(
        int customerAccountId,
        LoyaltyRank loyaltyRank,
        MySqlCommand? command = null
    )
    {
        command!.CommandText = "UPDATE Customer SET loyalty_rank = @rank WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", customerAccountId);
        command.Parameters.AddWithValue("@rank", loyaltyRank.ToString());
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void UpdateBanStatus(
        int customerAccountId,
        bool customerIsBanned,
        MySqlCommand? command = null
    )
    {
        command!.CommandText = "UPDATE Customer SET is_banned = @banned WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", customerAccountId);
        command.Parameters.AddWithValue("@banned", customerIsBanned);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void Delete(int customerAccountId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM Customer WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", customerAccountId);
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

    #endregion Statistics
}

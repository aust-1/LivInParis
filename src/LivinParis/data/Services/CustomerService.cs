using System.Text;
using LivInParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivInParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for customer-related operations in the database.
/// </summary>
[ConnectionControl]
public class CustomerService : ICustomerService
{
    #region CRUD

    /// <inheritdoc/>
    public virtual void Create(
        int? customerAccountId,
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
    public virtual List<List<string>> GetCustomersByOrderCount(
        int limit,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
        SELECT account_id, COUNT(*) AS command_count
        FROM OrderTransaction
        GROUP BY account_id
        ORDER BY command_count DESC
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
    public virtual List<List<string>> GetCustomersBySpending(
        int limit,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
        SELECT ot.account_id, SUM(d.price) AS total_spent
        FROM OrderTransaction ot
        JOIN OrderLine ol ON ot.transaction_id = ol.transaction_id
        JOIN MenuProposal mp ON ol.account_id = mp.account_id
        JOIN Dish d ON mp.dish_id = d.dish_id
        WHERE mp.proposal_date = DATE(ol.order_line_datetime)
        GROUP BY ot.account_id
        ORDER BY total_spent DESC
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
    public virtual decimal GetAveragePricePerCustomerOrder(MySqlCommand? command = null)
    {
        command!.CommandText =
            @"
        SELECT AVG(order_total) AS average_price
        FROM (
            SELECT o.order_line_id, SUM(d.price) AS order_total
            FROM OrderLine o
            JOIN MenuProposal mp ON o.account_id = mp.account_id AND mp.proposal_date = DATE(o.order_line_datetime)
            JOIN Dish d ON mp.dish_id = d.dish_id
            GROUP BY o.order_line_id
        ) AS order_totals";
        command.Parameters.Clear();

        using var reader = command.ExecuteReader();
        if (reader.Read() && !reader.IsDBNull(0))
        {
            return reader.GetDecimal(0);
        }

        return 0;
    }

    /// <inheritdoc/>
    public virtual List<List<string>> GetCustomerOrdersByNationalityAndPeriod(
        int limit,
        int customerId,
        string cuisineNationality,
        DateTime from,
        DateTime to,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
        SELECT ol.*
        FROM OrderLine ol
        JOIN OrderTransaction ot ON ol.transaction_id = ot.transaction_id
        JOIN MenuProposal mp ON ol.account_id = mp.account_id AND mp.proposal_date = DATE(ol.order_line_datetime)
        JOIN Dish d ON mp.dish_id = d.dish_id
        WHERE ot.account_id = @customerId
        AND d.cuisine_nationality = @nationality
        AND ol.order_line_datetime BETWEEN @from AND @to
        LIMIT @limit";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@customerId", customerId);
        command.Parameters.AddWithValue("@nationality", cuisineNationality);
        command.Parameters.AddWithValue("@from", from);
        command.Parameters.AddWithValue("@to", to);
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            List<string> row = [];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                row.Add(reader[i]?.ToString() ?? string.Empty);
            }
            results.Add(row);
        }

        return results;
    }

    #endregion Statistics
}

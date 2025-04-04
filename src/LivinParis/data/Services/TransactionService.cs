using System.Text;
using LivinParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for transaction-related operations in the database.
/// </summary>
[ConnectionControl]
public class TransactionService : ITransactionService
{
    /// <inheritdoc/>
    public virtual void Create(
        int transactionId,
        DateTime transactionDate,
        int customerAccountId,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                INSERT INTO Transaction (
                    transaction_id,
                    transaction_datetime,
                    account_id
                )
                VALUES (
                    @id,
                    @date,
                    @account
                )";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", transactionId);
        command.Parameters.AddWithValue("@date", transactionDate);
        command.Parameters.AddWithValue("@account", customerAccountId);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual List<List<string>> Read(
        int limit,
        DateTime? transactionDate = null,
        int? customerAccountId = null,
        decimal? minTotalPrice = null,
        decimal? maxTotalPrice = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];
        List<string> conditions = [];
        StringBuilder query = new(
            @"
                SELECT *
                FROM Transaction t
            "
        );

        if (minTotalPrice is not null || maxTotalPrice is not null)
        {
            query.Append(
                @"
                    JOIN OrderLine ol ON t.transaction_id = ol.transaction_id
                    JOIN MenuProposal mp ON ol.account_id = mp.account_id
                    JOIN Dish d ON mp.dish_id = d.dish_id
                "
            );
        }

        if (transactionDate is not null)
        {
            conditions.Add("t.transaction_datetime = @date");
        }

        if (customerAccountId is not null)
        {
            conditions.Add("t.account_id = @account");
        }

        if (minTotalPrice is not null || maxTotalPrice is not null)
        {
            conditions.Add("d.price IS NOT NULL");
        }

        if (conditions.Count > 0)
        {
            query.Append(" WHERE " + string.Join(" AND ", conditions));
        }

        if (minTotalPrice is not null || maxTotalPrice is not null)
        {
            query.Append(" GROUP BY t.transaction_id HAVING ");
            List<string> having = [];

            if (minTotalPrice is not null)
            {
                having.Add("SUM(d.price) >= @minTotal");
            }

            if (maxTotalPrice is not null)
            {
                having.Add("SUM(d.price) <= @maxTotal");
            }

            query.Append(string.Join(" AND ", having));
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

        if (transactionDate is not null)
        {
            command.Parameters.AddWithValue("@date", transactionDate);
        }
        if (customerAccountId is not null)
        {
            command.Parameters.AddWithValue("@account", customerAccountId);
        }
        if (minTotalPrice is not null)
        {
            command.Parameters.AddWithValue("@minTotal", minTotalPrice);
        }
        if (maxTotalPrice is not null)
        {
            command.Parameters.AddWithValue("@maxTotal", maxTotalPrice);
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
    public virtual List<List<string>> GetTopCustomersByOrderCount(
        int limit,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
        SELECT account_id, COUNT(*) AS command_count
        FROM Transaction
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
    public virtual List<List<string>> GetTopCustomersBySpending(
        int limit,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
        SELECT t.account_id, SUM(d.price) AS total_spent
        FROM Transaction t
        JOIN OrderLine o ON t.transaction_id = o.transaction_id
        JOIN MenuProposal mp ON o.account_id = mp.account_id
        JOIN Dish d ON mp.dish_id = d.dish_id
        WHERE mp.proposal_date = DATE(o.order_line_datetime)
        GROUP BY t.account_id
        ORDER BY total_spent DESC
        LIMIT @limit";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            results.Add(
                new() { reader[0].ToString() ?? string.Empty, reader[1].ToString() ?? string.Empty }
            );
        }

        return results;
    }

    /// <inheritdoc/>
    public virtual void Delete(int transactionId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM Transaction WHERE transaction_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", transactionId);
        command.ExecuteNonQuery();
    }
}

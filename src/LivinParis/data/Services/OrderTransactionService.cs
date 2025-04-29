using System.Text;
using LivInParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivInParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for transaction-related operations in the database.
/// </summary>
[ConnectionControl]
public class OrderTransactionService : IOrderTransactionService
{
    #region CRUD

    /// <inheritdoc/>
    public virtual void Create(
        int? transactionId,
        DateTime transactionDate,
        int customerAccountId,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                INSERT INTO OrderTransaction (
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
                FROM OrderTransaction ot
            "
        );

        if (minTotalPrice is not null || maxTotalPrice is not null)
        {
            query.Append(
                @"
                    JOIN OrderLine ol ON ot.transaction_id = ol.transaction_id
                    JOIN MenuProposal mp ON ol.account_id = mp.account_id
                    JOIN Dish d ON mp.dish_id = d.dish_id
                "
            );
        }

        if (transactionDate is not null)
        {
            conditions.Add("ot.transaction_datetime = @date");
        }

        if (customerAccountId is not null)
        {
            conditions.Add("ot.account_id = @account");
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
            query.Append(" GROUP BY ot.transaction_id HAVING ");
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
    public virtual void Delete(int transactionId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM OrderTransaction WHERE transaction_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", transactionId);
        command.ExecuteNonQuery();
    }

    #endregion CRUD

    #region Statistics

    public virtual decimal GetOrderTotalPrice(int transactionId, MySqlCommand? command = null)
    {
        command!.CommandText =
            @"
        SELECT SUM(d.price) AS total
        FROM OrderTransaction ot
        JOIN OrderLine ol ON ot.transaction_id = ol.transaction_id
        JOIN MenuProposal mp ON ol.account_id = mp.account_id AND mp.proposal_date = DATE(ol.order_line_datetime)
        JOIN Dish d ON mp.dish_id = d.dish_id
        WHERE ot.transaction_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", transactionId);

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
        }
        return 0;
    }

    #endregion Statistics
}

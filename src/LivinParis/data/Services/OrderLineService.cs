using System.Text;
using LivInParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivInParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for order line operations in the database.
/// </summary>
[ConnectionControl]
public class OrderLineService : IOrderLineService
{
    #region CRUD

    /// <inheritdoc/>
    public virtual void Create(
        int? orderLineId,
        DateTime orderLineDate,
        int duration,
        OrderLineStatus orderLineStatus,
        bool isEatIn,
        int addressId,
        int transactionId,
        int chefAccountId,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            @"
                INSERT INTO OrderLine (
                    order_line_id, order_line_datetime, duration, order_line_status,
                    is_eat_in, address_id, transaction_id, account_id
                )
                VALUES (
                    @id, @date, @duration, @status,
                    @eatIn, @address, @transaction, @chef
                )";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", orderLineId);
        command.Parameters.AddWithValue("@date", orderLineDate);
        command.Parameters.AddWithValue("@duration", duration);
        command.Parameters.AddWithValue("@status", orderLineStatus.ToString());
        command.Parameters.AddWithValue("@eatIn", isEatIn);
        command.Parameters.AddWithValue("@address", addressId);
        command.Parameters.AddWithValue("@transaction", transactionId);
        command.Parameters.AddWithValue("@chef", chefAccountId);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual List<List<string>> Read(
        int limit,
        DateTime? orderLineDate = null,
        int? duration = null,
        OrderLineStatus? orderLineStatus = null,
        bool? isEatIn = null,
        int? addressId = null,
        int? transactionId = null,
        int? chefAccountId = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];
        List<string> conditions = [];
        StringBuilder query = new("SELECT * FROM OrderLine");

        if (orderLineDate is not null)
        {
            conditions.Add("order_line_datetime = @date");
        }
        if (duration is not null)
        {
            conditions.Add("duration = @duration");
        }
        if (orderLineStatus is not null)
        {
            conditions.Add("order_line_status = @status");
        }
        if (isEatIn is not null)
        {
            conditions.Add("is_eat_in = @eatIn");
        }
        if (addressId is not null)
        {
            conditions.Add("address_id = @address");
        }
        if (transactionId is not null)
        {
            conditions.Add("transaction_id = @transaction");
        }
        if (chefAccountId is not null)
        {
            conditions.Add("account_id = @chef");
        }

        if (conditions.Count > 0)
        {
            query.Append(" WHERE " + string.Join(" AND ", conditions));
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

        if (orderLineDate is not null)
        {
            command.Parameters.AddWithValue("@date", orderLineDate);
        }
        if (duration is not null)
        {
            command.Parameters.AddWithValue("@duration", duration);
        }
        if (orderLineStatus is not null)
        {
            command.Parameters.AddWithValue("@status", orderLineStatus.ToString());
        }
        if (isEatIn is not null)
        {
            command.Parameters.AddWithValue("@eatIn", isEatIn);
        }
        if (addressId is not null)
        {
            command.Parameters.AddWithValue("@address", addressId);
        }
        if (transactionId is not null)
        {
            command.Parameters.AddWithValue("@transaction", transactionId);
        }
        if (chefAccountId is not null)
        {
            command.Parameters.AddWithValue("@chef", chefAccountId);
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
    public virtual void UpdateStatus(
        int orderLineId,
        OrderLineStatus orderLineStatus,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            "UPDATE OrderLine SET order_line_status = @status WHERE order_line_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@status", orderLineStatus.ToString());
        command.Parameters.AddWithValue("@id", orderLineId);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void Delete(int orderLineId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM OrderLine WHERE order_line_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", orderLineId);
        command.ExecuteNonQuery();
    }

    #endregion CRUD

    #region Statistics

    /// <inheritdoc/>
    public virtual List<List<string>> GetCommandCountByStreet(
        int limit,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
                SELECT street, COUNT(*) AS command_count
                FROM OrderLine
                JOIN Address ON Address.address_id = OrderLine.address_id
                GROUP BY street
                ORDER BY command_count DESC
                LIMIT @limit";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            results.Add(
                [
                    reader.IsDBNull(0) ? string.Empty : reader.GetString(0),
                    reader.IsDBNull(1)
                        ? string.Empty
                        : reader.GetValue(1).ToString() ?? string.Empty,
                ]
            );
        }

        return results;
    }

    /// <inheritdoc/>
    public virtual List<List<string>> GetCommandCountByPostalCode(
        int limit,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
                SELECT postal_code, COUNT(*) AS command_count
                FROM OrderLine
                JOIN Address ON OrderLine.address_id = Address.address_id
                GROUP BY postal_code
                ORDER BY command_count DESC
                LIMIT @limit";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@limit", limit);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            results.Add(
                [
                    reader.IsDBNull(0)
                        ? string.Empty
                        : reader.GetValue(0).ToString() ?? string.Empty,
                    reader.IsDBNull(1)
                        ? string.Empty
                        : reader.GetValue(1).ToString() ?? string.Empty,
                ]
            );
        }

        return results;
    }

    /// <inheritdoc/>
    public virtual List<List<string>> GetTotalOrderValueByStreet(
        int limit,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
        SELECT a.street, SUM(d.price) AS total_value
        FROM OrderLine ol
        JOIN Address a ON ol.address_id = a.address_id
        JOIN MenuProposal mp ON mp.account_id = ol.account_id AND mp.proposal_date = DATE(ol.order_line_datetime)
        JOIN Dish d ON mp.dish_id = d.dish_id
        GROUP BY a.street
        ORDER BY total_value DESC
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
    public virtual List<List<string>> GetTotalOrderValueByPostalCode(
        int limit,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
        SELECT a.postal_code, SUM(d.price) AS total_value
        FROM OrderLine o
        JOIN Address a ON o.address_id = a.address_id
        JOIN MenuProposal mp ON mp.account_id = ol.account_id AND mp.proposal_date = DATE(ol.order_line_datetime)
        JOIN Dish d ON mp.dish_id = d.dish_id
        GROUP BY a.postal_code
        ORDER BY total_value DESC
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
    public virtual List<List<string>> GetMostOrderedHours(int limit, MySqlCommand? command = null)
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
        SELECT HOUR(order_line_datetime) AS hour_slot, COUNT(*) AS count
        FROM OrderLine
        GROUP BY hour_slot
        ORDER BY count DESC
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
    public virtual List<List<string>> GetMostOrderedWeekdays(
        int limit,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
        SELECT DAYNAME(order_line_datetime) AS weekday, COUNT(*) AS count
        FROM OrderLine
        GROUP BY weekday
        ORDER BY count DESC
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
    public virtual List<List<string>> GetAverageOrderDuration(
        int limit,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
        SELECT o.account_id, AVG(o.duration) AS average_duration
        FROM OrderLine o
        GROUP BY o.account_id
        ORDER BY average_duration DESC
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
    public virtual List<List<string>> GetOrdersByPeriod(
        int limit,
        DateTime from,
        DateTime to,
        MySqlCommand? command = null
    )
    {
        List<List<string>> results = [];

        command!.CommandText =
            @"
        SELECT *
        FROM OrderLine
        WHERE order_line_datetime BETWEEN @from AND @to
        ORDER BY order_line_datetime DESC
        LIMIT @limit";
        command.Parameters.Clear();
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

    /// <inheritdoc/>
    public virtual decimal GetAverageOrderPrice(MySqlCommand? command = null)
    {
        command!.CommandText =
            @"
        SELECT AVG(d.price)
        FROM OrderLine o
        JOIN MenuProposal mp ON o.account_id = mp.account_id AND mp.proposal_date = DATE(o.order_line_datetime)
        JOIN Dish d ON mp.dish_id = d.dish_id";
        command.Parameters.Clear();

        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
        }
        return 0;
    }

    #endregion Statistics
}

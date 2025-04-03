using MySql.Data.MySqlClient;

namespace LivinParis.Data;

[ConnectionControl]
public class OrderLineRepository : IOrderLine
{
    public virtual void CreateOrderLine(
        int orderLineId,
        DateTime orderLineDate,
        int duration,
        string status,
        bool itsEatIn,
        int adressId,
        int transactionId,
        int accountId,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            "INSERT INTO OrderLine VALUES (@io, @da, @du, @s, @e, @adressId, @transactionId, @accountId)";
        command.Parameters.AddWithValue("@io", orderLineId);
        command.Parameters.AddWithValue("@da", orderLineDate);
        command.Parameters.AddWithValue("@du", duration);
        command.Parameters.AddWithValue("@s", status);
        command.Parameters.AddWithValue("@e", itsEatIn);
        command.Parameters.AddWithValue("@adressId", adressId);
        command.Parameters.AddWithValue("@transactionId", transactionId);
        command.Parameters.AddWithValue("@accountId", accountId);
        command.ExecuteNonQuery();
    }

    public virtual List<List<string>> GetOrderLines(int limit, MySqlCommand? command = null)
    {
        command!.CommandText = "SELECT * FROM OrderLine LIMIT @l";
        command.Parameters.AddWithValue("@l", limit);

        using var reader = command.ExecuteReader();
        List<List<string>> orderlines = [];
        while (reader.Read())
        {
            List<string> orderline = [];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string value = reader[i]?.ToString() ?? string.Empty;
                orderline.Add(value);
            }

            orderlines.Add(orderline);
        }

        return orderlines;
    }

    public virtual void UpdateStatusOrderLine(
        int orderLineId,
        string status,
        MySqlCommand? command = null
    )
    {
        command!.CommandText = "UPDATE OrderLine SET status = @s WHERE order_line_id = @i";
        command.Parameters.AddWithValue("@s", status);
        command.Parameters.AddWithValue("@i", orderLineId);
        command.ExecuteNonQuery();
    }

    public virtual void DeleteOrderLine(int orderLineId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM OrderLine WHERE order_line_id = @i";
        command.Parameters.AddWithValue("@i", orderLineId);
        command.ExecuteNonQuery();
    }
}

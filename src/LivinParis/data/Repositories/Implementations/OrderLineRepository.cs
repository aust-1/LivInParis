using System.ComponentModel.DataAnnotations;

namespace LivinParis.Data;
using MySql.Data.MySqlClient;

public static class OrderLineRepository : IOrderLine
{
    private static MySqlConnection? s_connection;

    public static bool InitConnection()
    {
        try
        {
            string connection = "SERVER=localhost;PORT=3306;DATABASE=PSI;UID=eliottfrancois;PASSWORD=PSI";
            s_connection = new MySqlConnection(connection);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static void OpenConnection()
    {
        s_connection!.Open();
    }

    public static void CloseConnection()
    {
        s_connection!.Close();
    }

    ///CRUD

    public static void CreateOrderLine(int orderLineId, DateTime orderLineDate, int duration, string status,
        bool ItsEatIn, int number, string street, int transactionId, int accountId)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText =
            "INSERT INTO OrderLine VALUES (@io, @da, @du, @s, @e, @number, @street, @transactionId, @accountId)";
        command.Parameters.AddWithValue("@io", orderLineId);
        command.Parameters.AddWithValue("@da", orderLineDate);
        command.Parameters.AddWithValue("@du", duration);
        command.Parameters.AddWithValue("@s", status);
        command.Parameters.AddWithValue("@e", ItsEatIn);
        command.Parameters.AddWithValue("@number", number);
        command.Parameters.AddWithValue("@street", street);
        command.Parameters.AddWithValue("@transactionId", transactionId);
        command.Parameters.AddWithValue("@accountId", accountId);
        command.ExecuteNonQuery();
        CloseConnection();
    }

    public static List<List<string>> GetOrderLine(int limit)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "SELECT * FROM OrderLine LIMIT @l";
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

    public static void UpdateType(string reviewType, int reviewId)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "UPDATE Review SET review_type = @t WHERE review_id = @i";
        command.Parameters.AddWithValue("@t", reviewType);
        command.Parameters.AddWithValue("@i", reviewId);
        command.ExecuteNonQuery();
        CloseConnection();
    }

    public static void UpdateRating(decimal reviewRating, int reviewId)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "UPDATE Review SET review_rating = @r WHERE review_id = @i";
        command.Parameters.AddWithValue("@r", reviewRating);
        command.Parameters.AddWithValue("@i", reviewId);
        command.ExecuteNonQuery();
        CloseConnection();
    }

    public static void UpdateComment(string comment, int reviewId)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "UPDATE Review SET comment = @c WHERE review_id = @i";
        command.Parameters.AddWithValue("@c", comment);
        command.Parameters.AddWithValue("@i", reviewId);
        command.ExecuteNonQuery();
        CloseConnection();
    }

    public static void UpdateDate(DateTime reviewDate, int reviewId)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "UPDATE Review SET review_date = @d WHERE review_id = @i";
        command.Parameters.AddWithValue("@d", reviewDate);
        command.Parameters.AddWithValue("@i", reviewId);
        command.ExecuteNonQuery();
    }

    public static void DeleteOrderLine(int orderLineId)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "DELETE FROM OrderLine WHERE order_line_id = @i";
        command.Parameters.AddWithValue("@i", orderLineId);
        command.ExecuteNonQuery();
        CloseConnection();
    }
}
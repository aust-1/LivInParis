using System.ComponentModel.DataAnnotations;

namespace LivinParis.Data;
using MySql.Data.MySqlClient;

public static class ReviewRepository : IReview
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

    public static void CreateReview(int reviewId, string reviewType, decimal reviewRating, string comment, DateTime reviewDate)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "INSERT INTO account VALUES (@i, @t, @r, @c, @d)";
        command.Parameters.AddWithValue("@i", reviewId);
        command.Parameters.AddWithValue("@t", reviewType);
        command.Parameters.AddWithValue("@r", reviewRating);
        command.Parameters.AddWithValue("@c", comment);
        command.Parameters.AddWithValue("@d", reviewDate);
        command.ExecuteNonQuery();
        CloseConnection();
    }

    public static List<List<string>> GetReviews(int limit)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "SELECT * FROM Review LIMIT @l";
        command.Parameters.AddWithValue("@l", limit);
        
        using var reader = command.ExecuteReader();
        List<List<string>> reviews = [];
        while (reader.Read())
        {
            List<string> review = [];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string value = reader[i]?.ToString() ?? string.Empty;
                review.Add(value);
            }
            reviews.Add(review);
        }
        return reviews;

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
        CloseConnection();
    }
    public static void DeleteReview(string reviewId)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "DELETE FROM Review WHERE review_id = @i";
        command.Parameters.AddWithValue("@i", reviewId);
        command.ExecuteNonQuery();
        CloseConnection();
    }
    
    
    
    
}
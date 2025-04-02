using System.ComponentModel.DataAnnotations;

namespace LivinParis.Data;
using MySql.Data.MySqlClient;

public static class AdressRepository : IAdress
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

    public static void CreateAdress(int idAdress, int number, string street, string nearestMetro)
    {
        OpenConnection();
        using var command = s_connection!.CreateCommand();
        command.CommandText = "INSERT INTO Adress VALUES (@i, @n, @s, @m) ";
        command.Parameters.AddWithValue("@i", idAdress);
        command.Parameters.AddWithValue("@n", number);
        command.Parameters.AddWithValue("@s", street);
        command.Parameters.AddWithValue("@m", nearestMetro);
        command.ExecuteNonQuery();
        CloseConnection();
    }

    public static List<List<string>> GetAdresse(int limit)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "SELECT * FROM Adress LIMIT @l";
        command.Parameters.AddWithValue("@l", limit);
        
        using var reader = command.ExecuteReader();
        List<List<string>> adresses = [];
        while (reader.Read())
        {
            List<string> adress = [];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string value = reader[i]?.ToString() ?? string.Empty;
                adress.Add(value);
            }
            adresses.Add(adress);
        }
        return adresses;

    }

    public static void UpdateNearestMetro(int idAdress, string nearestMetro)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "UPDATE Adress SET nearestMetro = @m WHERE adress_id = @a";
        command.Parameters.AddWithValue("@m", nearestMetro);
        command.Parameters.AddWithValue("@a", idAdress);
        command.ExecuteNonQuery();
        CloseConnection();
    }
    

    public static void DeleteAdress(string idAdress)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "DELETE FROM Adress WHERE account_id = @a";
        command.Parameters.AddWithValue("@a", idAdress);
        command.ExecuteNonQuery();
        CloseConnection();
    }
    
    
    
    
}
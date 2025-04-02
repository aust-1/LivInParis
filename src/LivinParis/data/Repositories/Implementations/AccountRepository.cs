using System.ComponentModel.DataAnnotations;

namespace LivinParis.Data;
using MySql.Data.MySqlClient;

public static class AccountRepository : IAccount
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

    public static void CreateAccount(int accountId, string email,string password)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "INSERT INTO Account VALUES (@a,@e, @p)";
        command.Parameters.AddWithValue("@e", email);
        command.Parameters.AddWithValue("@a", accountId);
        command.Parameters.AddWithValue("@p", password);
        command.ExecuteNonQuery();
        CloseConnection();
    }

    public static List<List<string>> GetAccounts(int limit)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "SELECT * FROM Account LIMIT @l";
        command.Parameters.AddWithValue("@l", limit);
        
        using var reader = command.ExecuteReader();
        List<List<string>> accounts = [];
        while (reader.Read())
        {
            List<string> account = [];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string value = reader[i]?.ToString() ?? string.Empty;
                account.Add(value);
            }
            accounts.Add(account);
        }
        return accounts;

    }

    public static void UpdateEmail(int accountId, string email)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "UPDATE Account SET email = @e WHERE account_id = @a";
        command.Parameters.AddWithValue("@e", email);
        command.Parameters.AddWithValue("@a", accountId);
        command.ExecuteNonQuery();
        CloseConnection();
    }

    public static void UpdatePassword(int accountId, string password)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "UPDATE Account SET password = @p WHERE account_id = @a";
        command.Parameters.AddWithValue("@e", password);
        command.Parameters.AddWithValue("@p", accountId);
        command.ExecuteNonQuery();
        CloseConnection();
    }

    public static void DeleteAccount(int accountId)
    {
        OpenConnection();
        using var command = new MySqlCommand();
        command.CommandText = "DELETE FROM account WHERE account_id = @a";
        command.Parameters.AddWithValue("@a", accountId);
        command.ExecuteNonQuery();
        CloseConnection();
    }
    
    
    
    
}

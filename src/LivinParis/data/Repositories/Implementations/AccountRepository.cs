using MySql.Data.MySqlClient;

namespace LivinParis.Data;

public class AccountRepository : IAccount
{
    [ConnectionControl]
    public virtual void CreateAccount(int accountId, string email, string password)
    {
        using var command = new MySqlCommand();
        command.CommandText = "INSERT INTO Account VALUES (@a,@e, @p)";
        command.Parameters.AddWithValue("@e", email);
        command.Parameters.AddWithValue("@a", accountId);
        command.Parameters.AddWithValue("@p", password);
        command.ExecuteNonQuery();
    }

    [ConnectionControl]
    public virtual List<List<string>> GetAccounts(int limit)
    {
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

    [ConnectionControl]
    public virtual void UpdateEmail(int accountId, string email)
    {
        using var command = new MySqlCommand();
        command.CommandText = "UPDATE Account SET email = @e WHERE account_id = @a";
        command.Parameters.AddWithValue("@e", email);
        command.Parameters.AddWithValue("@a", accountId);
        command.ExecuteNonQuery();
    }

    [ConnectionControl]
    public virtual void UpdatePassword(int accountId, string password)
    {
        using var command = new MySqlCommand();
        command.CommandText = "UPDATE Account SET password = @p WHERE account_id = @a";
        command.Parameters.AddWithValue("@e", password);
        command.Parameters.AddWithValue("@p", accountId);
        command.ExecuteNonQuery();
    }

    [ConnectionControl]
    public virtual void DeleteAccount(int accountId)
    {
        using var command = new MySqlCommand();
        command.CommandText = "DELETE FROM account WHERE account_id = @a";
        command.Parameters.AddWithValue("@a", accountId);
        command.ExecuteNonQuery();
    }
}

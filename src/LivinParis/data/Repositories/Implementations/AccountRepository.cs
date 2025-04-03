using MySql.Data.MySqlClient;

namespace LivinParis.Data;

[ConnectionControl]
public class AccountRepository : IAccount
{
    public virtual void CreateAccount(
        int accountId,
        string email,
        string password,
        MySqlCommand? command = null
    )
    {
        command!.CommandText = "INSERT INTO Account VALUES (@a,@e, @p)";
        command.Parameters.AddWithValue("@e", email);
        command.Parameters.AddWithValue("@a", accountId);
        command.Parameters.AddWithValue("@p", password);
        command.ExecuteNonQuery();
    }

    public virtual List<List<string>> GetAccounts(int limit, MySqlCommand? command = null)
    {
        command!.CommandText = "SELECT * FROM Account LIMIT @l";
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

    public virtual void UpdateEmail(int accountId, string email, MySqlCommand? command = null)
    {
        command!.CommandText = "UPDATE Account SET email = @e WHERE account_id = @a";
        command.Parameters.AddWithValue("@e", email);
        command.Parameters.AddWithValue("@a", accountId);
        command.ExecuteNonQuery();
    }

    public virtual void UpdatePassword(int accountId, string password, MySqlCommand? command = null)
    {
        command!.CommandText = "UPDATE Account SET password = @p WHERE account_id = @a";
        command.Parameters.AddWithValue("@e", password);
        command.Parameters.AddWithValue("@p", accountId);
        command.ExecuteNonQuery();
    }

    public virtual void DeleteAccount(int accountId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM account WHERE account_id = @a";
        command.Parameters.AddWithValue("@a", accountId);
        command.ExecuteNonQuery();
    }
}

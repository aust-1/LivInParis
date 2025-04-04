using LivinParisRoussilleTeynier.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Services;

/// <summary>
/// Provides implementation for account-related operations.
/// </summary>
[ConnectionControl]
public class AccountService : IAccountService
{
    /// <inheritdoc/>
    public virtual void Create(
        int accountId,
        string accountEmail,
        string password,
        MySqlCommand? command = null
    )
    {
        command!.CommandText =
            "INSERT INTO Account (account_id, account_email, password) VALUES (@id, @mail, @pwd)";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", accountId);
        command.Parameters.AddWithValue("@mail", accountEmail);
        command.Parameters.AddWithValue("@pwd", password);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual List<List<string>> Read(int limit, MySqlCommand? command = null)
    {
        command!.CommandText = "SELECT * FROM Account LIMIT @limit";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@limit", limit);

        List<List<string>> accounts = [];

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            List<string> account = [];
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string value = reader.IsDBNull(i)
                    ? string.Empty
                    : reader.GetValue(i).ToString() ?? string.Empty;
                account.Add(value);
            }
            accounts.Add(account);
        }
        return accounts;
    }

    /// <inheritdoc/>
    public virtual void UpdateEmail(
        int accountId,
        string accountEmail,
        MySqlCommand? command = null
    )
    {
        command!.CommandText = "UPDATE Account SET account_email = @mail WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", accountId);
        command.Parameters.AddWithValue("@mail", accountEmail);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void UpdatePassword(int accountId, string password, MySqlCommand? command = null)
    {
        command!.CommandText = "UPDATE Account SET password = @pwd WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", accountId);
        command.Parameters.AddWithValue("@pwd", password);
        command.ExecuteNonQuery();
    }

    /// <inheritdoc/>
    public virtual void Delete(int accountId, MySqlCommand? command = null)
    {
        command!.CommandText = "DELETE FROM Account WHERE account_id = @id";
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@id", accountId);
        command.ExecuteNonQuery();
    }
}

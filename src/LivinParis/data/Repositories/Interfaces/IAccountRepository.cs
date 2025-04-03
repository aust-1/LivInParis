using MySql.Data.MySqlClient;

namespace LivinParis.Data;

public interface IAccount
{
    void CreateAccount(int accountId, string email, string password, MySqlCommand? command = null);

    Dictionary<int, List<string>> GetAccounts(int limit, MySqlCommand? command = null);

    void UpdateEmail(int accountId, string email, MySqlCommand? command = null);

    void UpdatePassword(int accountId, string password, MySqlCommand? command = null);

    void DeleteAccount(int accountId, MySqlCommand? command = null);
}

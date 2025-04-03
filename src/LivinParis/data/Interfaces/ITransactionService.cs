using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface ITransaction
{
    void CreateTransaction(
        int transactionId,
        DateTime transactionDate,
        int customerAccountId,
        MySqlCommand? command = null
    );

    List<List<string>> GetTransactions(
        int limit,
        DateTime? transactionDate = null,
        int? customerAccountId = null,
        decimal? totalPriceHigherThan = null,
        decimal? totalPriceBelow = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    void DeleteTransaction(int transactionId, MySqlCommand? command = null);
}

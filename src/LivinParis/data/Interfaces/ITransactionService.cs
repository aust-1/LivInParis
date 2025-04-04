using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface ITransactionService
{
    void Create(
        int transactionId,
        DateTime transactionDate,
        int customerAccountId,
        MySqlCommand? command = null
    );

    List<List<string>> Read(
        int limit,
        DateTime? transactionDate = null,
        int? customerAccountId = null,
        decimal? minTotalPrice = null,
        decimal? maxTotalPrice = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    List<List<string>> GetTopCustomersByOrderCount(int limit, MySqlCommand? command = null);

    List<List<string>> GetTopCustomersBySpending(int limit, MySqlCommand? command = null);

    void Delete(int transactionId, MySqlCommand? command = null);
}

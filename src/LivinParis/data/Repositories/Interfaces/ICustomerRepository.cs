using MySql.Data.MySqlClient;

namespace LivinParis.Data;

public interface ICustomer
{
    void CreateCustomer(
        int accountId,
        decimal customerRating,
        LoyaltyRank loyaltyRank,
        bool customerIsBanned,
        MySqlCommand? command = null
    );

    Dictionary<int, List<string>> GetCustomers(
        int limit,
        decimal ratingHigherThan = 0m,
        decimal ratingBelow = 5m,
        LoyaltyRank? loyaltyRank = null,
        bool? customerIsBanned = null,
        string? orderBy = null,
        MySqlCommand? command = null
    );

    void UpdateCustomerRating(int accountId, decimal customerRating, MySqlCommand? command = null);
    void UpdateCustomerLoyaltyRank(
        int accountId,
        LoyaltyRank loyaltyRank,
        MySqlCommand? command = null
    );
    void UpdateCustomerBanStatus(
        int accountId,
        bool customerIsBanned,
        MySqlCommand? command = null
    );
    void DeleteCustomer(int accountId, MySqlCommand? command = null);
}

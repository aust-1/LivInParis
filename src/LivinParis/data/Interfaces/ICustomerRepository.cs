using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface ICustomer
{
    void CreateCustomer(
        int customerAccountId,
        decimal customerRating,
        LoyaltyRank loyaltyRank,
        bool customerIsBanned,
        MySqlCommand? command = null
    );

    List<List<string>> GetCustomers(
        int limit,
        decimal? ratingHigherThan = null,
        decimal? ratingBelow = null,
        LoyaltyRank? loyaltyRank = null,
        bool? customerIsBanned = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    void UpdateCustomerRating(
        int customerAccountId,
        decimal customerRating,
        MySqlCommand? command = null
    );
    void UpdateCustomerLoyaltyRank(
        int customerAccountId,
        LoyaltyRank loyaltyRank,
        MySqlCommand? command = null
    );
    void UpdateCustomerBanStatus(
        int customerAccountId,
        bool customerIsBanned,
        MySqlCommand? command = null
    );
    void DeleteCustomer(int customerAccountId, MySqlCommand? command = null);
}

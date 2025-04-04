using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

public interface ICustomerService
{
    void Create(
        int customerAccountId,
        decimal customerRating,
        LoyaltyRank loyaltyRank,
        bool customerIsBanned,
        MySqlCommand? command = null
    );

    List<List<string>> Read(
        int limit,
        decimal? minRating = null,
        decimal? maxRating = null,
        LoyaltyRank? loyaltyRank = null,
        bool? customerIsBanned = null,
        string? orderBy = null,
        bool? orderDirection = null,
        MySqlCommand? command = null
    );

    void UpdateRating(int customerAccountId, decimal customerRating, MySqlCommand? command = null);
    void UpdateLoyaltyRank(
        int customerAccountId,
        LoyaltyRank loyaltyRank,
        MySqlCommand? command = null
    );
    void UpdateBanStatus(
        int customerAccountId,
        bool customerIsBanned,
        MySqlCommand? command = null
    );
    void Delete(int customerAccountId, MySqlCommand? command = null);
}

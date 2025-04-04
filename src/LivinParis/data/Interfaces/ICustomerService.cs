using MySql.Data.MySqlClient;

namespace LivinParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Provides methods for managing customer data in the system.
/// </summary>
public interface ICustomerService
{
    #region CRUD

    /// <summary>
    /// Creates a new customer account.
    /// </summary>
    void Create(
        int customerAccountId,
        decimal customerRating,
        LoyaltyRank loyaltyRank,
        bool customerIsBanned,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Retrieves a list of customers with optional filters.
    /// </summary>
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

    /// <summary>
    /// Updates the rating of a customer.
    /// </summary>
    void UpdateRating(int customerAccountId, decimal customerRating, MySqlCommand? command = null);

    /// <summary>
    /// Updates the loyalty rank of a customer.
    /// </summary>
    void UpdateLoyaltyRank(
        int customerAccountId,
        LoyaltyRank loyaltyRank,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Updates the ban status of a customer.
    /// </summary>
    void UpdateBanStatus(
        int customerAccountId,
        bool customerIsBanned,
        MySqlCommand? command = null
    );

    /// <summary>
    /// Deletes a customer from the system.
    /// </summary>
    void Delete(int customerAccountId, MySqlCommand? command = null);

    #endregion CRUD
}
